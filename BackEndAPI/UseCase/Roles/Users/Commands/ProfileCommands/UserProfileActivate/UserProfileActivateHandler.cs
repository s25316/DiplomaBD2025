// Ignore Spelling: mongo
using Domain.Shared.Enums;
using MediatR;
using UseCase.MongoDb;
using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileActivate.Request;
using UseCase.Roles.Users.Repositories;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileActivate
{
    public class UserProfileActivateHandler : IRequestHandler<UserProfileActivateRequest, ProfileCommandResponse>
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


        // Methods
        public async Task<ProfileCommandResponse> Handle(UserProfileActivateRequest request, CancellationToken cancellationToken)
        {
            // MongoDB Part
            var userId = request.Command.UserId;
            var activationData = await _mongo.GetUserActivationDataAsync(
                userId,
                cancellationToken);

            if (!activationData.HasCreated)
            {
                return PrepareResponse(HttpCode.NotFound);
            }
            if (activationData.ActivationUrlSegment != request.Command.ActivationUrlSegment)
            {
                return PrepareResponse(HttpCode.BadRequest);
            }
            if (activationData.HasActivated)
            {
                return PrepareResponse(HttpCode.Ok);
            }

            // MSSQL Part
            var selectData = await _personRepository.GetAsync(userId, cancellationToken);
            if (selectData.Code != HttpCode.Ok)
            {
                return PrepareResponse(selectData.Code);
            }
            var domainPerson = selectData.Item;


            // Domain Event Handling
            domainPerson.RaiseProfileActivatedEvent();
            foreach (var @event in domainPerson.DomainEvents)
            {
                await _mediator.Publish(@event);
            }
            return PrepareResponse(HttpCode.Ok);
        }

        // Static Methods
        private static ProfileCommandResponse PrepareResponse(HttpCode code, string? message = null)
        {
            return ProfileCommandResponse.PrepareResponse(code, message);
        }
    }
}
