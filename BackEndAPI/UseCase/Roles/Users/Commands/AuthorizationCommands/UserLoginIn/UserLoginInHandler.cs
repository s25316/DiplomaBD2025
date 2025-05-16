// Ignore Spelling: Mongo
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using Domain.Shared.ValueObjects.Emails;
using MediatR;
using UseCase.MongoDb;
using UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos;
using UseCase.Roles.Users.Commands.AuthorizationCommands.SharedResponses;
using UseCase.Roles.Users.Commands.AuthorizationCommands.UserLoginIn.Request;
using UseCase.Roles.Users.Commands.AuthorizationCommands.UserLoginIn.Response;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Services.Authentication.Generators;
using DomainPerson = Domain.Features.People.Aggregates.Person;

namespace UseCase.Roles.Users.Commands.AuthorizationCommands.UserLoginIn
{
    public class UserLoginInHandler : IRequestHandler<UserLoginInRequest, UserLoginInResponse>
    {
        // Properties
        private static readonly int _countDaysFor2Stage = 30;
        private static readonly int _countMinutesCodeValid = 5;

        private readonly IAuthenticationGeneratorService _authenticationGenerator;
        private readonly IPersonRepository _personRepository;
        private readonly IMongoDbService _mongoDbService;
        private readonly IMediator _mediator;


        // Constructor
        public UserLoginInHandler(
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
        public async Task<UserLoginInResponse> Handle(UserLoginInRequest request, CancellationToken cancellationToken)
        {
            // MSSQL Part
            var login = GetLogin(request);
            var selectResult = await _personRepository.GetAsync(login, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return InvalidResponse();
            }

            // Check Password
            var domainPerson = selectResult.Item;
            if (!IsValidPassword(domainPerson, request.Command.Password))
            {
                domainPerson.RaiseAuthorizationInvalidEvent(Messages.Entity_Person_Account_InvalidPassword);
                await PublishAsync(domainPerson, cancellationToken);
                return InvalidResponse();
            }

            // Check Is Blocked, Removed, Activated
            var personId = GetPersonId(domainPerson);
            var personLoginInMongoDto = await _mongoDbService.GetUserLoginInDataAsync(personId, cancellationToken);
            if (!IsAllowedLoginIn(domainPerson, personLoginInMongoDto))
            {
                await PublishAsync(domainPerson, cancellationToken);
                return InvalidResponse();
            }

            // Invoke 2Stage Verification
            if (domainPerson.HasTwoFactorAuthentication ||
                personLoginInMongoDto.LastHandStage == null ||
                CustomTimeProvider.GetDays(personLoginInMongoDto.LastHandStage.Value) > _countDaysFor2Stage)
            {
                return await Handle2StageAsync(domainPerson, cancellationToken);
            }

            // Return Authorization Data
            return await HandleRegularAuthorizationAsync(domainPerson, cancellationToken);
        }

        // Private Static Methods
        private static Email GetLogin(UserLoginInRequest request)
        {
            return (Email?)request.Command.Login
                ?? throw new UseCaseLayerException("Always Should be not empty login");
        }
        private static Guid GetPersonId(DomainPerson item)
        {
            return item.Id?.Value
                ?? throw new UseCaseLayerException("Problem with mapping from DB");
        }

        private static bool IsAllowedLoginIn(DomainPerson domain, UserLoginInMongoDbDto mongo)
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

        private static UserLoginInResponse InvalidResponse()
        {
            return UserLoginInResponse.InvalidResponse();
        }

        private static UserLoginInResponse IsNeed2StageResponse(UserLoginIn2StageDto dto)
        {
            return UserLoginInResponse.IsNeed2StageResponse(dto);
        }

        private static UserLoginInResponse AuthorizationResponse(UserAuthorizationDataDto dto)
        {
            return UserLoginInResponse.AuthorizationResponse(dto);
        }

        // Private Non static Methods
        private bool IsValidPassword(DomainPerson domain, string password)
        {
            var hashedPassword = _authenticationGenerator.HashPassword(
                domain.Salt,
                password);
            return hashedPassword == domain.Password;
        }

        private async Task PublishAsync(DomainPerson person, CancellationToken cancellationToken)
        {
            foreach (var @event in person.DomainEvents)
            {
                await _mediator.Publish(@event, cancellationToken);
            }
        }

        private async Task<UserLoginInResponse> Handle2StageAsync(
            DomainPerson domainPerson,
            CancellationToken cancellationToken)
        {
            var urlSegment = _authenticationGenerator.GenerateUrlSegment();
            var code = _authenticationGenerator.GenerateCode();

            var codeValidTo = CustomTimeProvider.Now.AddMinutes(_countMinutesCodeValid);
            domainPerson.RaiseAuthorization2StageEvent(urlSegment, code, codeValidTo);

            await PublishAsync(domainPerson, cancellationToken);
            return IsNeed2StageResponse(new UserLoginIn2StageDto
            {
                UrlSegmentPart1 = GetPersonId(domainPerson),
                UrlSegmentPart2 = urlSegment,
                ValidTo = codeValidTo,
            });
        }

        private async Task<UserLoginInResponse> HandleRegularAuthorizationAsync(
            DomainPerson domainPerson,
            CancellationToken cancellationToken)
        {
            var personId = GetPersonId(domainPerson).ToString();
            var (jwt, jwtValidTo) = _authenticationGenerator.GenerateJwt(personId, []);
            var (refreshToken, refreshTokenValidTo) = _authenticationGenerator.GenerateRefreshToken();
            domainPerson.RaiseAuthorizationLoginInEvent(jwt, refreshToken, refreshTokenValidTo);

            await PublishAsync(domainPerson, cancellationToken);
            return AuthorizationResponse(new UserAuthorizationDataDto
            {
                Jwt = jwt,
                JwtValidTo = CustomTimeProvider.ConvertToPoland(jwtValidTo),
                RefreshToken = refreshToken,
                RefreshTokenValidTo = refreshTokenValidTo,
            });
        }
    }
}
