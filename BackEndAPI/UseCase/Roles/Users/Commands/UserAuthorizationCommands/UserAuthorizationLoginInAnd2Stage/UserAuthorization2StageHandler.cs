// Ignore Spelling: Mongo 

using Domain.Features.People.Aggregates;
using Domain.Shared.CustomProviders;
using MediatR;
using UseCase.MongoDb;
using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginInAnd2Stage.Request.UserAuthorization2Stage;
using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginInAnd2Stage.Response;
using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginInAnd2Stage.Response.UserAuthorization2Stage;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Services.Authentication.Generators;
using DomainPerson = Domain.Features.People.Aggregates.Person;

namespace UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginInAnd2Stage
{
    public class UserAuthorization2StageHandler : IRequestHandler<UserAuthorization2StageRequest, UserAuthorization2StageResponse>
    {
        // Properties
        private readonly IAuthenticationGeneratorService _authenticationGenerator;
        private readonly IPersonRepository _personRepository;
        private readonly IMongoDbService _mongoDbService;
        private readonly IMediator _mediator;


        // Constructor
        public UserAuthorization2StageHandler(
            IAuthenticationGeneratorService authenticationGenerator,
            IPersonRepository personRepository,
            IMongoDbService mongoDbService,
            IMediator mediator)
        {
            _authenticationGenerator = authenticationGenerator;
            _personRepository = personRepository;
            _mongoDbService = mongoDbService;
            _mediator = mediator;
        }

        // Methods
        public async Task<UserAuthorization2StageResponse> Handle(UserAuthorization2StageRequest request, CancellationToken cancellationToken)
        {
            // Get Data From MongoDb
            var mongo2StageAuthorizationDto = await _mongoDbService.Get2StageAuthorizationAsync(
                request.Command.UrlSegmentPart1,
                request.Command.UrlSegmentPart2,
                request.Command.CodeDto.Code,
                cancellationToken);

            // If Not Found Or Expired
            if (!mongo2StageAuthorizationDto.HasValue ||
                mongo2StageAuthorizationDto.HasExpired ||
                mongo2StageAuthorizationDto.UserId == null)
            {
                return InvalidResponse();
            }

            // Get From MSSQL
            var selectResult = await _personRepository.GetAsync(
                mongo2StageAuthorizationDto.UserId.Value,
                cancellationToken);

            // return Authorization Data
            return await HandleRegularAuthorizationAsync(
                selectResult.Item,
                cancellationToken);
        }

        // Private Static Methods
        private static Guid GetPersonId(DomainPerson item)
        {
            return item.Id?.Value
                ?? throw new KeyNotFoundException("Problem with mapping from DB");
        }

        public static UserAuthorization2StageResponse InvalidResponse()
        {
            return UserAuthorization2StageResponse.InvalidResponse();
        }

        public static UserAuthorization2StageResponse ValidResponse(
            UserLoginInDataDto item)
        {
            return UserAuthorization2StageResponse.ValidResponse(item);
        }

        // Private Non Static Methods
        private async Task PublishAsync(DomainPerson person, CancellationToken cancellationToken)
        {
            foreach (var @event in person.DomainEvents)
            {
                await _mediator.Publish(@event, cancellationToken);
            }
        }

        private async Task<UserAuthorization2StageResponse> HandleRegularAuthorizationAsync(
           DomainPerson domainPerson,
           CancellationToken cancellationToken)
        {
            var personId = GetPersonId(domainPerson).ToString();
            var (jwt, jwtValidTo) = _authenticationGenerator.GenerateJwt(personId, []);
            var (refreshToken, refreshTokenValidTo) = _authenticationGenerator.GenerateRefreshToken();
            domainPerson.RaiseAuthorizationLoginInEvent(
                jwt,
                refreshToken,
                refreshTokenValidTo);

            await PublishAsync(domainPerson, cancellationToken);
            return ValidResponse(new UserLoginInDataDto
            {
                Jwt = jwt,
                JwtValidTo = CustomTimeProvider.ConvertToPoland(jwtValidTo),
                RefreshToken = refreshToken,
                RefreshTokenValidTo = refreshTokenValidTo,
            });
        }
    }
}
