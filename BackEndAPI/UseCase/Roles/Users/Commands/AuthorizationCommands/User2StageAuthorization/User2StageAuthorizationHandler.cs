// Ignore Spelling: Mongo 

using Domain.Shared.CustomProviders;
using MediatR;
using UseCase.MongoDb;
using UseCase.Roles.Users.Commands.AuthorizationCommands.SharedResponses;
using UseCase.Roles.Users.Commands.AuthorizationCommands.User2StageAuthorization.Request;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Repositories.People;
using UseCase.Shared.Services.Authentication.Generators;
using DomainPerson = Domain.Features.People.Aggregates.Person;

namespace UseCase.Roles.Users.Commands.AuthorizationCommands.User2StageAuthorization
{
    public class User2StageAuthorizationHandler : IRequestHandler<User2StageAuthorizationRequest, UserAuthorizationResponse>
    {
        // Properties
        private readonly IAuthenticationGeneratorService _authenticationGenerator;
        private readonly IPersonRepository _personRepository;
        private readonly IMongoDbService _mongoDbService;
        private readonly IMediator _mediator;


        // Constructor
        public User2StageAuthorizationHandler(
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
        public async Task<UserAuthorizationResponse> Handle(User2StageAuthorizationRequest request, CancellationToken cancellationToken)
        {
            // Get Data From MongoDb
            var mongo2StageAuthorizationDto = await _mongoDbService.GetUser2StageDataAsync(
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
                ?? throw new UseCaseLayerException("Problem with mapping from DB");
        }

        public static UserAuthorizationResponse InvalidResponse()
        {
            return UserAuthorizationResponse.InvalidResponse();
        }

        public static UserAuthorizationResponse ValidResponse(
            UserAuthorizationDataDto item)
        {
            return UserAuthorizationResponse.ValidResponse(item);
        }

        // Private Non Static Methods
        private async Task PublishAsync(DomainPerson person, CancellationToken cancellationToken)
        {
            foreach (var @event in person.DomainEvents)
            {
                await _mediator.Publish(@event, cancellationToken);
            }
            person.ClearEvents();
        }

        private async Task<UserAuthorizationResponse> HandleRegularAuthorizationAsync(
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
            return ValidResponse(new UserAuthorizationDataDto
            {
                Jwt = jwt,
                JwtValidTo = CustomTimeProvider.ConvertToPoland(jwtValidTo),
                RefreshToken = refreshToken,
                RefreshTokenValidTo = refreshTokenValidTo,
            });
        }
    }
}
