// Ignore Spelling: Mongo, Dto
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using MediatR;
using UseCase.MongoDb;
using UseCase.Roles.Users.Commands.ProfileCommands.Response;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileRestore.Request;
using UseCase.Roles.Users.Repositories;
using UseCase.Shared.Exceptions;

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
            foreach (var @event in doaminPerson.DomainEvents)
            {
                await _mediator.Publish(@event, cancellationToken);
            }

            var updateResult = await _personRepository.UpdateAsync(doaminPerson, cancellationToken);
            if (updateResult.Code != HttpCode.Ok)
            {
                throw new UseCaseLayerException(updateResult.Metadata.Message);
            }

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
