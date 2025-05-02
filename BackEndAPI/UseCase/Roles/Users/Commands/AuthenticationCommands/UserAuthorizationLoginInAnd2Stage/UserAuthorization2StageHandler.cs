// Ignore Spelling: Mongo 

using Domain.Features.People.Aggregates;
using Domain.Shared.CustomProviders;
using MediatR;
using UseCase.MongoDb;
using UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.Request.UserAuthorization2Stage;
using UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.Response;
using UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage.Response.UserAuthorization2Stage;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Services.Authentication.Generators;

namespace UseCase.Roles.Users.Commands.AuthenticationCommands.UserAuthorizationLoginInAnd2Stage
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

            Console.WriteLine(request.Command.UrlSegmentPart1);
            Console.WriteLine(request.Command.UrlSegmentPart2);
            Console.WriteLine(request.Command.CodeDto.Code);
            var mongoDto = await _mongoDbService.Get2StageAuthorizationAsync(
                request.Command.UrlSegmentPart1,
                request.Command.UrlSegmentPart2,
                request.Command.CodeDto.Code,
                cancellationToken);
            if (mongoDto.Item == null)
            {
                return InvalidResponse();
            }
            if (mongoDto.Item.CodeValidTo < CustomTimeProvider.Now)
            {
                return InvalidResponse();
            }


            var personId = mongoDto.Item.UserId;
            var selectResult = await _personRepository.GetAsync(
                personId,
                cancellationToken);
            var domainPerson = selectResult.Item;



            var (jwt, jwtValidTo) = _authenticationGenerator.GenerateJwt(personId.ToString(), []);
            var (refreshToken, refreshTokenValidTo) = _authenticationGenerator.GenerateRefreshToken();
            domainPerson.RaiseAuthorizationLoginInEvent(
                jwt.ToString(),
                refreshToken,
                refreshTokenValidTo);

            await PublishAsync(domainPerson, cancellationToken);
            return ValidResponse(new UserLoginInDataDto
            {
                Jwt = jwt.ToString(),
                JwtValidTo = CustomTimeProvider.ConvertToPoland(jwtValidTo),
                RefreshToken = refreshToken,
                RefreshTokenValidTo = refreshTokenValidTo,
            });
        }
        // Private Static Methods
        public static UserAuthorization2StageResponse InvalidResponse()
        {
            return UserAuthorization2StageResponse.InvalidResponse();
        }

        public static UserAuthorization2StageResponse ValidResponse(
            UserLoginInDataDto item)
        {
            return UserAuthorization2StageResponse.ValidResponse(item);
        }
        // Private Static Methods
        private async Task PublishAsync(Person person, CancellationToken cancellationToken)
        {
            foreach (var @event in person.DomainEvents)
            {
                await _mediator.Publish(@event, cancellationToken);
            }
        }
    }
}
