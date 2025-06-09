using Domain.Shared.Enums;
using MediatR;
using UseCase.Roles.Administrators.Commands.AdministrationCommands.AdministratorGrandAdministrator.Request;
using UseCase.Shared.Repositories.People;
using UseCase.Shared.Responses.ItemResponse;

namespace UseCase.Roles.Administrators.Commands.AdministrationCommands.AdministratorGrandAdministrator
{
    public class AdministratorGrandAdministratorHandler : IRequestHandler<AdministratorGrandAdministratorRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IPersonRepository _personRepository;


        // Constructor
        public AdministratorGrandAdministratorHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(AdministratorGrandAdministratorRequest request, CancellationToken cancellationToken)
        {
            var selectResult = await _personRepository
                .GetAsync(request.PersonId, cancellationToken);
            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareResponse(selectResult.Code, selectResult.Metadata.Message);
            }
            var domainPerson = selectResult.Item;

            if (domainPerson.IsAdministrator)
            {
                return PrepareResponse(HttpCode.Conflict, "Person already Administrator");
            }
            domainPerson.GrandAdministrator();

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
