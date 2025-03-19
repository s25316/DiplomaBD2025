using Domain.Features.People.ValueObjects;
using Domain.Shared.Enums;
using Domain.Shared.ValueObjects;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.BranchesCreate.Repositories;
using UseCase.Roles.CompanyUser.Commands.BranchesCreate.Request;
using UseCase.Roles.CompanyUser.Commands.BranchesCreate.Response;
using UseCase.Shared.DTOs.Requests;
using UseCase.Shared.Repositories.Addresses;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Templates.Response.Commands;
using DomainBranch = Domain.Features.Branches.Entities.Branch;

namespace UseCase.Roles.CompanyUser.Commands.BranchesCreate
{
    public class BranchesCreateHandler : IRequestHandler<BranchesCreateRequest, BranchesCreateResponse>
    {
        // Properties
        private readonly IAuthenticationInspectorService _inspectorService;
        private readonly IBranchRepository _branchRepository;
        private readonly IAddressRepository _addressRepository;


        // Constructor
        public BranchesCreateHandler(
            IAuthenticationInspectorService inspectorService,
            IBranchRepository branchRepository,
            IAddressRepository addressRepository)
        {
            _inspectorService = inspectorService;
            _addressRepository = addressRepository;
            _branchRepository = branchRepository;
        }


        // Methods
        public async Task<BranchesCreateResponse> Handle(BranchesCreateRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var addressDictionary = await CreateAddressesAsync(request);
            var buildResult = Build(request, addressDictionary);

            // Domain Part
            if (!buildResult.IsValid)
            {
                return new BranchesCreateResponse
                {
                    Result = request.Commands
                        .Select(cmd => new ResponseCommandTemplate<BranchCreateCommand>
                        {
                            Item = cmd,
                            IsCorrect = string.IsNullOrWhiteSpace(buildResult.Dictionary[cmd].Error),
                            Message = buildResult.Dictionary[cmd].Error ?? string.Empty,
                        }),
                    HttpCode = HttpCode.BadRequest,
                };
            }

            var dictionary = buildResult.Dictionary
                .ToDictionary(
                val => val.Key,
                val => val.Value.Branch);
            var repositoryResult = await _branchRepository.CreateAsync(
                personId,
                dictionary.Values,
                cancellationToken);

            return new BranchesCreateResponse
            {
                Result = dictionary
                        .Select(cmd => new ResponseCommandTemplate<BranchCreateCommand>
                        {
                            Item = cmd.Key,
                            IsCorrect = repositoryResult.Dictionary[cmd.Value].IsCorrect,
                            Message = repositoryResult.Dictionary[cmd.Value].Message,
                        }),
                HttpCode = repositoryResult.Code,
            };
        }

        // Private Static Methods
        private sealed class BulderResult
        {
            public required Dictionary<BranchCreateCommand, BranchAndErrorMessage> Dictionary { get; init; }
            public required bool IsValid { get; init; }
        }

        private sealed class BranchAndErrorMessage
        {
            public required DomainBranch Branch { get; init; }
            public required string? Error { get; init; }
        }

        private static BulderResult Build(
            BranchesCreateRequest request,
            Dictionary<AddressRequestDto, AddressId> addressDictionary)
        {
            var dictionary = new Dictionary<BranchCreateCommand, BranchAndErrorMessage>();
            var isValid = true;
            foreach (var command in request.Commands)
            {
                var builder = new DomainBranch.Builder()
                    .SetCompanyId(request.CompanyId)
                    .SetName(command.Name)
                    .SetDescription(command.Description)
                    .SetAddressId(addressDictionary[command.Address]);

                if (isValid && builder.HasErrors())
                {
                    isValid = false;
                }
                dictionary.Add(command, new BranchAndErrorMessage
                {
                    Branch = builder.Build(),
                    Error = builder.GetErrors(),
                });
            }
            return new BulderResult
            {
                Dictionary = dictionary,
                IsValid = isValid,
            };
        }

        // Private Non Static Methods
        private PersonId GetPersonId(BranchesCreateRequest request)
        {
            return _inspectorService.GetPersonId(request.Metadata.Claims);
        }

        private async Task<AddressId> CreateAddressAsync(AddressRequestDto request)
        {
            var id = await _addressRepository.CreateAddressAsync(request);
            return (AddressId)id;
        }

        private async Task<Dictionary<AddressRequestDto, AddressId>> CreateAddressesAsync(
            BranchesCreateRequest request)
        {
            var addressDtoSet = request.Commands.Select(req => req.Address).ToHashSet();
            var dictionary = new Dictionary<AddressRequestDto, AddressId>();
            foreach (var dto in addressDtoSet)
            {
                dictionary[dto] = await CreateAddressAsync(dto);
            }
            return dictionary;
        }
    }
}
