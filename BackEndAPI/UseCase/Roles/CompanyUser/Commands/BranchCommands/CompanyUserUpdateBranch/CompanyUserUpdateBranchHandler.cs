﻿using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using Domain.Shared.ValueObjects.Ids;
using MediatR;
using UseCase.Roles.CompanyUser.Commands.BranchCommands.CompanyUserUpdateBranch.Request;
using UseCase.Roles.CompanyUser.Repositories.Branches;
using UseCase.Shared.Repositories.Addresses;
using UseCase.Shared.Requests.DTOs;
using UseCase.Shared.Responses.ItemResponse;
using UseCase.Shared.Services.Authentication.Inspectors;
using DomainBranch = Domain.Features.Branches.Entities.Branch;

namespace UseCase.Roles.CompanyUser.Commands.BranchCommands.CompanyUserUpdateBranch
{
    public class CompanyUserUpdateBranchHandler : IRequestHandler<CompanyUserUpdateBranchRequest, CommandResponse<CompanyUserUpdateBranchCommand>>
    {
        // Properties
        private readonly IAuthenticationInspectorService _inspectorService;
        private readonly IBranchRepository _branchRepository;
        private readonly IAddressRepository _addressRepository;


        // Constructor
        public CompanyUserUpdateBranchHandler(
            IAuthenticationInspectorService inspectorService,
            IBranchRepository branchRepository,
            IAddressRepository addressRepository)
        {
            _inspectorService = inspectorService;
            _addressRepository = addressRepository;
            _branchRepository = branchRepository;
        }


        // Methods
        public async Task<CommandResponse<CompanyUserUpdateBranchCommand>> Handle(CompanyUserUpdateBranchRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);

            // Get from DB
            var selectResult = await _branchRepository.GetAsync(
                personId,
                request.BranchId,
                cancellationToken);

            if (selectResult.Code != HttpCode.Ok)
            {
                return PrepareResponse(
                    selectResult.Code,
                    selectResult.Metadata.Message,
                    request);
            }

            // Update in Domain
            var updater = await PrepareUpdater(selectResult.Item, request.Command);
            if (updater.HasErrors())
            {
                return PrepareResponse(
                        HttpCode.BadRequest,
                        updater.GetErrors(),
                        request);
            }

            // Update in DB
            var updateResult = await _branchRepository.UpdateAsync(
                personId,
                updater.Build(),
                cancellationToken);

            // If is OK or NOT
            return PrepareResponse(
                    updateResult.Code,
                    updateResult.Metadata.Message,
                    request);
        }

        // Private Static Methods
        private static CommandResponse<CompanyUserUpdateBranchCommand> PrepareResponse(
            HttpCode code,
            string? message,
            CompanyUserUpdateBranchRequest request)
        {
            return CommandResponse<CompanyUserUpdateBranchCommand>.PrepareResponse(
                code,
                request.Command,
                message);
        }


        // Private Non Static Methods
        private PersonId GetPersonId(CompanyUserUpdateBranchRequest request)
        {
            return _inspectorService.GetPersonId(request.Metadata.Claims);
        }

        private async Task<AddressId> CreateAddressAsync(AddressRequestDto request)
        {
            var id = await _addressRepository.CreateAddressAsync(request);
            return (AddressId)id;
        }

        private async Task<DomainBranch.Updater> PrepareUpdater(
            DomainBranch item,
            CompanyUserUpdateBranchCommand command)
        {
            var updater = new DomainBranch.Updater(item)
                .SetDescription(command.Description);

            if (command.Address != null)
            {
                var addressId = await CreateAddressAsync(command.Address);
                updater.SetAddressId(addressId.Value);
            }
            if (!string.IsNullOrWhiteSpace(command.Name))
            {
                updater.SetName(command.Name);
            }
            return updater;
        }
    }
}
