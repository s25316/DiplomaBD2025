// Ignore Spelling: mongo
using Domain.Shared.Enums;
using MediatR;
using UseCase.MongoDb;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileActivate.Request;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileActivate.Response;
using UseCase.Roles.Users.Repositories;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileActivate
{
    public class UserProfileActivateHandler : IRequestHandler<UserProfileActivateRequest, UserProfileActivateResponse>
    {
        // Properties
        private readonly IMediator _mediator;
        private readonly IMongoDbService _mongo;
        private readonly IPersonRepository _personRepository;


        // Constructor
        public UserProfileActivateHandler(
            IMediator mediator,
            IMongoDbService mongo,
            IPersonRepository personRepository)
        {
            _mediator = mediator;
            _mongo = mongo;
            _personRepository = personRepository;
        }

        public async Task<UserProfileActivateResponse> Handle(UserProfileActivateRequest request, CancellationToken cancellationToken)
        {
            // MongoDB Part
            var userId = request.Command.UserId;
            var activationData = await _mongo.GetUserProfileActivationData(
                userId,
                cancellationToken);

            if (!activationData.HasCreated)
            {
                return PrepareResponse(HttpCode.NotFound);
            }
            if (activationData.ActivationUrlSegment !=
                request.Command.ActivationUrlSegment)
            {
                return PrepareResponse(HttpCode.BadRequest);
            }
            if (activationData.HasActivated)
            {
                return PrepareResponse(HttpCode.Ok);
            }

            // MSSQL Part
            var selectData = await _personRepository
                .GetAsync(userId, cancellationToken);
            if (selectData.Code != HttpCode.Ok)
            {
                return PrepareResponse(selectData.Code);
            }

            // Domain Event Handling
            var domainPerson = selectData.Item;
            domainPerson.RaiseProfileActivatedEvent();

            foreach (var @event in domainPerson.DomainEvents)
            {
                await _mediator.Publish(@event);
            }
            return PrepareResponse(HttpCode.Ok);
        }

        public static UserProfileActivateResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            return UserProfileActivateResponse.PrepareResponse(code, message);
        }
    }
}
