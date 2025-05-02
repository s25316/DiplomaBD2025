// Ignore Spelling: Mongo
using Domain.Features.People.Aggregates;
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using Domain.Shared.ValueObjects.Emails;
using MediatR;
using UseCase.MongoDb;
using UseCase.MongoDb.UserLogs.DTOs;
using UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.Request.UserAuthorizationLoginIn;
using UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.Response;
using UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.Response.UserAuthorizationLoginIn;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Services.Authentication.Generators;

namespace UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage
{
    public class UserAuthorizationLoginInHandler : IRequestHandler<UserAuthorizationLoginInRequest, UserAuthorizationLoginInResponse>
    {
        // Properties
        private static readonly int _countDaysFor2Stage = 30;
        private static readonly int _countMinutesCodeValid = 5;

        private readonly IAuthenticationGeneratorService _authenticationGenerator;
        private readonly IPersonRepository _personRepository;
        private readonly IMongoDbService _mongoDbService;
        private readonly IMediator _mediator;


        // Constructor
        public UserAuthorizationLoginInHandler(
            IAuthenticationGeneratorService authenticationGenerator,
            IPersonRepository personRepository,
            IMongoDbService mongoDbService,
            IMediator mediator)
        {
            _mediator = mediator;
            _mongoDbService = mongoDbService;
            _personRepository = personRepository;
            _authenticationGenerator = authenticationGenerator;
        }


        // Methods
        public async Task<UserAuthorizationLoginInResponse> Handle(UserAuthorizationLoginInRequest request, CancellationToken cancellationToken)
        {
            // MSSQL Part
            Email login = (Email?)request.Command.Login
                ?? throw new KeyNotFoundException("Always Should be not empty login");

            var selectResult = await _personRepository.GetAsync(login, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return InvalidResponse();
            }

            var domainPerson = selectResult.Item;
            var personId = domainPerson.Id?.Value
                ?? throw new KeyNotFoundException("Problem with mapping from DB");

            if (!IsValidPassword(domainPerson, request.Command.Password))
            {
                domainPerson.RaiseAuthorizationInvalidEvent(Messages.Entity_Person_Account_InvalidPassword);
                await PublishAsync(domainPerson, cancellationToken);
                return InvalidResponse();
            }


            // MongoDB Part
            var personMongoDto = await _mongoDbService
                .GetLoginInDataAsync(personId, cancellationToken);

            if (!IsAllowedLoginIn(domainPerson, personMongoDto))
            {
                await PublishAsync(domainPerson, cancellationToken);
                return InvalidResponse();
            }

            if (domainPerson.HasTwoFactorAuthentication ||
                personMongoDto.LastHandStage == null ||
                CustomTimeProvider.GetDays(personMongoDto.LastHandStage.Value) > _countDaysFor2Stage)
            {
                var urlSegment = _authenticationGenerator.GenerateUrlSegment();
                var code = _authenticationGenerator.GenerateCode();

                var codeValidTo = CustomTimeProvider.Now
                    .AddMinutes(_countMinutesCodeValid);

                domainPerson.RaiseAuthorization2StageEvent(
                    urlSegment,
                    code,
                    codeValidTo);


                await PublishAsync(domainPerson, cancellationToken);
                return IsNeed2StageResponse(new UserAuthorizationLoginIn2StageDto
                {
                    UrlSegmentPart1 = personId,
                    UrlSegmentPart2 = urlSegment,
                    ValidTo = codeValidTo,
                });

            }

            var (jwt, jwtValidTo) = _authenticationGenerator.GenerateJwt(personId.ToString(), []);
            var (refreshToken, refreshTokenValidTo) = _authenticationGenerator.GenerateRefreshToken();
            domainPerson.RaiseAuthorizationLoginInEvent(
                jwt.ToString(),
                refreshToken,
                refreshTokenValidTo);

            await PublishAsync(domainPerson, cancellationToken);
            return AuthorizationResponse(new UserLoginInDataDto
            {
                Jwt = jwt.ToString(),
                JwtValidTo = CustomTimeProvider.ConvertToPoland(jwtValidTo),
                RefreshToken = refreshToken,
                RefreshTokenValidTo = refreshTokenValidTo,
            });
        }

        // Private Static Methods
        private static UserAuthorizationLoginInResponse InvalidResponse()
        {
            return UserAuthorizationLoginInResponse.InvalidResponse();
        }

        private static UserAuthorizationLoginInResponse IsNeed2StageResponse(UserAuthorizationLoginIn2StageDto dto)
        {
            return UserAuthorizationLoginInResponse.IsNeed2StageResponse(dto);
        }

        private static UserAuthorizationLoginInResponse AuthorizationResponse(UserLoginInDataDto dto)
        {
            return UserAuthorizationLoginInResponse.AuthorizationResponse(dto);
        }

        private static bool IsAllowedLoginIn(Person domain, UserLoginInMongoDbDto mongo)
        {
            if (domain.HasBlocked ||
                domain.HasRemoved ||
                mongo.HasBlocked ||
                mongo.HasRemoved ||
                !mongo.HasActivated)
            {
                string reson;

                if (domain.HasBlocked || mongo.HasBlocked)
                {
                    reson = Messages.Entity_Person_Account_Blocked;
                }
                else if (domain.HasRemoved || mongo.HasRemoved)
                {
                    reson = Messages.Entity_Person_Account_Removed;
                }
                else
                {
                    reson = Messages.Entity_Person_Account_NotActivated;
                }
                domain.RaiseAuthorizationInvalidEvent(reson);
                return false;
            }
            return true;
        }

        // Private Non static Methods
        private bool IsValidPassword(Person domain, string password)
        {
            var hashedPassword = _authenticationGenerator.HashPassword(
                domain.Salt,
                password);
            return hashedPassword == domain.Password;
        }

        private async Task PublishAsync(Person person, CancellationToken cancellationToken)
        {
            foreach (var @event in person.DomainEvents)
            {
                await _mediator.Publish(@event, cancellationToken);
            }
        }
    }
}
