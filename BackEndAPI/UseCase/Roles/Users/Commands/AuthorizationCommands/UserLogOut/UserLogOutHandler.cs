// Ignore Spelling: Mongo
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using UseCase.MongoDb;
using UseCase.Roles.Users.Commands.AuthorizationCommands.UserLogOut.Request;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainPerson = Domain.Features.People.Aggregates.Person;

namespace UseCase.Roles.Users.Commands.AuthorizationCommands.UserLogOut
{
    public class UserLogOutHandler : IRequestHandler<UserLogOutRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IPersonRepository _personRepository;
        private readonly IMongoDbService _mongoDbService;
        private readonly IMediator _mediator;


        // Constructor
        public UserLogOutHandler(
            IAuthenticationInspectorService authenticationInspector,
            IPersonRepository personRepository,
            IMongoDbService mongoDbService,
            IMediator mediator)
        {
            _authenticationInspector = authenticationInspector;
            _personRepository = personRepository;
            _mongoDbService = mongoDbService;
            _mediator = mediator;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(UserLogOutRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Metadata.Authorization))
            {
                return InvalidResponse();
            }
            var personId = GetPersonId(request);

            // MongoDb Part
            var logOutData = await _mongoDbService.GetUserLogOutDataAsync(
                personId,
                request.Metadata.Authorization,
                cancellationToken);
            if (logOutData.AuthorizationData == null)
            {
                return InvalidResponse();
            }
            if (logOutData.HasLogOut)
            {
                return ValidResponse();
            }

            // MsSql Part
            var selectResult = await _personRepository.GetAsync(personId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return InvalidResponse();
            }
            var domainPreson = selectResult.Item;
            domainPreson.RaiseAuthorizationLogOutEvent(
                logOutData.AuthorizationData.Jwt,
                logOutData.AuthorizationData.RefreshToken,
                logOutData.AuthorizationData.RefreshTokenValidTo);

            await PublishAsync(domainPreson, cancellationToken);
            return ValidResponse();
        }

        // Static Methods
        private PersonId GetPersonId(UserLogOutRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }

        private static ResultMetadataResponse ValidResponse()
        {
            return ResultMetadataResponse.PrepareResponse(HttpCode.Ok);
        }

        private static ResultMetadataResponse InvalidResponse()
        {
            return ResultMetadataResponse.PrepareResponse(HttpCode.Unauthorized);
        }

        // Non Static Methods
        private async Task PublishAsync(DomainPerson person, CancellationToken cancellationToken)
        {
            Console.WriteLine(person.DomainEvents.Count);
            foreach (var @event in person.DomainEvents)
            {
                await _mediator.Publish(@event, cancellationToken);
            }
        }
    }
}
