// Ignore Spelling: Mongo, Dto
using Domain.Shared.CustomProviders;
using Domain.Shared.Enums;
using MediatR;
using UseCase.MongoDb;
using UseCase.Roles.Users.Commands.RemovingCommands.UserRestorePerson.Request;
using UseCase.Shared.Exceptions;
using UseCase.Shared.Repositories.People;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Users.Commands.RemovingCommands.UserRestorePerson
{
    public class UserRestorePersonHandler : IRequestHandler<UserRestorePersonRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IMongoDbService _mongo;
        private readonly IPersonRepository _personRepository;


        // Constructor
        public UserRestorePersonHandler(
            IPersonRepository personRepository,
            IMongoDbService mongo)
        {
            _mongo = mongo;
            _personRepository = personRepository;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(UserRestorePersonRequest request, CancellationToken cancellationToken)
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


            var updateResult = await _personRepository.UpdateAsync(
                doaminPerson,
                cancellationToken);
            if (updateResult.Code != HttpCode.Ok)
            {
                throw new UseCaseLayerException(updateResult.Metadata.Message);
            }

            return PrepareValid();
        }

        //Static Methods
        public static ResultMetadataResponse PrepareValid()
        {
            return ResultMetadataResponse.PrepareResponse(HttpCode.Ok);
        }

        public static ResultMetadataResponse PrepareInvalid()
        {
            return ResultMetadataResponse.PrepareResponse(HttpCode.BadRequest);
        }
    }
}
