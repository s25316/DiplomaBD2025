using Domain.Features.People.ValueObjects;
using Domain.Shared.Enums;
using MediatR;
using System.Data;
using UseCase.Roles.CompanyUser.Commands.BranchCreate.Request;
using UseCase.Roles.CompanyUser.Commands.BranchCreate.Response;
using UseCase.Roles.CompanyUser.Commands.Repositories.Branches;
using UseCase.Shared.Repositories.Addresses;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Templates.Response;
using DomainBranch = Domain.Features.Branches.Entities.Branch;

namespace UseCase.Roles.CompanyUser.Commands.BranchCreate
{
    public class BranchCreateHandler : IRequestHandler<BranchCreateRequest, BranchCreateResponse>
    {
        // Properties
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IAddressRepository _addressRepository;
        private readonly IBranchRepository _branchRepository;


        // Constructor
        public BranchCreateHandler(
            IAuthenticationInspectorService authenticationInspector,
            IAddressRepository addressRepository,
            IBranchRepository branchRepository)
        {
            _authenticationInspector = authenticationInspector;
            _addressRepository = addressRepository;
            _branchRepository = branchRepository;
        }


        // Methods
        public async Task<BranchCreateResponse> Handle(BranchCreateRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var operationIsAllowed = await _branchRepository.HasAccessToCompany(
                personId,
                request.CompanyId,
                cancellationToken);

            if (!operationIsAllowed)
            {
                return new BranchCreateResponse
                {
                    Commands = request.Commands.Select(cmd => new ResponseItemTemplate<BranchCreateCommand>
                    {
                        Item = cmd,
                        IsCorrect = false,
                        Message = HttpCode.Forbidden.Description(),
                    }),
                    HttpCode = HttpCode.Forbidden,
                    IsCorrect = false,
                };
            }

            var builders = await GetBuildersAsync(request);
            var isAllValid = true;
            foreach (var pair in builders.Values)
            {
                if (pair.HasErrors())
                {
                    isAllValid = false;
                    break;
                }
            }

            if (!isAllValid)
            {
                return new BranchCreateResponse
                {
                    Commands = builders.Select(pair => new ResponseItemTemplate<BranchCreateCommand>
                    {
                        Item = pair.Key,
                        IsCorrect = !pair.Value.HasErrors(),
                        Message = pair.Value.HasErrors() ?
                            pair.Value.GetErrors() :
                            HttpCode.Correct.Description(),
                    }),
                    HttpCode = HttpCode.BadRequest,
                    IsCorrect = false
                };
            }

            var domainBraches = builders.Select(pair => pair.Value.Build());
            await _branchRepository.CreateAsync(domainBraches, cancellationToken);

            return new BranchCreateResponse
            {
                Commands = request.Commands.Select(
                    command => new ResponseItemTemplate<BranchCreateCommand>
                    {
                        Item = command,
                        IsCorrect = true,
                        Message = HttpCode.Created.Description(),
                    }),
                HttpCode = HttpCode.Created,
                IsCorrect = true,
            };
        }

        // Private methods
        private PersonId GetPersonId(BranchCreateRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }


        private async Task<Dictionary<BranchCreateCommand, DomainBranch.Builder>> GetBuildersAsync(
            BranchCreateRequest request)
        {
            if (!request.Commands.Any())
            {
                return [];
            }

            var dictionary = new Dictionary<BranchCreateCommand, DomainBranch.Builder>();
            foreach (var command in request.Commands)
            {
                if (dictionary.ContainsKey(command))
                {
                    continue;
                }
                var addressId = await _addressRepository.CreateAddressAsync(command.Address);
                dictionary[command] = new DomainBranch.Builder()
                    .SetAddressId(addressId)
                    .SetCompanyId(request.CompanyId)
                    .SetName(command.Name)
                    .SetDescription(command.Description);
            }
            return dictionary;
        }
    }
}
