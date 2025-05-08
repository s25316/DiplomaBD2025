using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using MediatR;
using UseCase.MongoDb;
using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileRestore.Request;
// Ignore Spelling: Mongo, Dto
using UseCase.Roles.Users.Repositories;

namespace UseCase.Roles.Users.Commands.ProfileCommands.UserProfileRestore
{
    public class UserProfileRestoreHandler : IRequestHandler<UserProfileRestoreRequest, ProfileCommandResponse>
    {
        // Properties
        private readonly IMediator _mediator;
        private readonly IMongoDbService _mongo;
        private readonly IPersonRepository _personRepository;


        // Constructor
        public UserProfileRestoreHandler(
            IMediator mediator,
            IPersonRepository personRepository,
            IMongoDbService mongo)
        {
            _mediator = mediator;
            _mongo = mongo;
            _personRepository = personRepository;
        }


        // Methods
        public async Task<ProfileCommandResponse> Handle(UserProfileRestoreRequest request, CancellationToken cancellationToken)
        {
            var removedDto = await _mongo.GeUserRemovedAsync(
                request.UrlSegment1,
                request.UrlSegment2,
                cancellationToken);
            if (removedDto.Removed == null ||
                removedDto.Removed.ValidTo <= CustomTimeProvider.Now ||
                removedDto.Removed.IsDeactivated)
            {
                return PrepareInvalid();
            }

            var selectResult = await _personRepository.GetAsync(
                removedDto.Removed.UserId,
                cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareInvalid();
            }
            var doaminPerson = selectResult.Item;


            doaminPerson.Restore();
            Console.WriteLine($"Data {doaminPerson.Removed}");
            Console.WriteLine(doaminPerson.DomainEvents.Count);
            foreach (var @event in doaminPerson.DomainEvents)
            {
                await _mediator.Publish(@event, cancellationToken);
            }
            await _personRepository.UpdateAsync(doaminPerson, cancellationToken);
            return PrepareValid();
        }

        //Static Methods
        public static ProfileCommandResponse PrepareValid()
        {
            return ProfileCommandResponse.PrepareResponse(HttpCode.Ok);
        }

        public static ProfileCommandResponse PrepareInvalid()
        {
            return ProfileCommandResponse.PrepareResponse(HttpCode.BadRequest);
        }
    }
}
