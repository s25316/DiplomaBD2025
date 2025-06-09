using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.Administrators.Commands.BlockPersonCommands.AdministratorBlockPerson.Request;
using UseCase.Shared.Repositories.People;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Administrators.Commands.BlockPersonCommands.AdministratorBlockPerson
{
    public class AdministratorBlockPersonHandler : IRequestHandler<AdministratorBlockPersonRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IPersonRepository _personRepository;


        // Constructor
        public AdministratorBlockPersonHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(AdministratorBlockPersonRequest request, CancellationToken cancellationToken)
        {
            var selectResult = await _personRepository.GetAsync(request.PersonId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareResponse(selectResult.Code);
            }
            var domainPerson = selectResult.Item;

            if (domainPerson.HasBlocked)
            {
                return PrepareResponse(HttpCode.Conflict, "Person already Blocked");
            }
            domainPerson.Block(request.Command.Message);

            var updateResult = await _personRepository.UpdateAsync(domainPerson, cancellationToken);
            return PrepareResponse(updateResult.Code, updateResult.Metadata.Message);
        }

        // Static Methods
        private static ResultMetadataResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            return ResultMetadataResponse.PrepareResponse(code, message);
        }
    }
}
