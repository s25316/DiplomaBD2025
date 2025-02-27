using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Commands.BranchCreate.Request;
using UseCase.Roles.CompanyUser.Commands.BranchCreate.Response;
using UseCase.Shared.Repositories.Addresses;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Services.Time;
using UseCase.Shared.Templates.Response;

namespace UseCase.Roles.CompanyUser.Commands.BranchCreate
{
    public class BranchCreateHandler : IRequestHandler<BranchCreateRequest, BranchCreateResponse>
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly ITimeService _timeService;
        private readonly IAuthenticationInspectorService _authenticationInspector;
        private readonly IAddressRepository _addressRepository;

        // Constructor
        public BranchCreateHandler(
            DiplomaBdContext context,
            ITimeService timeService,
            IAuthenticationInspectorService authenticationInspector,
            IAddressRepository addressRepository)
        {
            _context = context;
            _timeService = timeService;
            _authenticationInspector = authenticationInspector;
            _addressRepository = addressRepository;
        }

        // Methods
        public async Task<BranchCreateResponse> Handle(BranchCreateRequest request, CancellationToken cancellationToken)
        {
            var now = _timeService.GetNow();
            var userId = _authenticationInspector.GetPersonId(request.Metadata.Claims);
            var companyRolesCount = await _context.CompanyPeople
                .Where(conn =>
                    conn.PersonId == userId.Value &&
                    conn.CompanyId == request.CompanyId)
                .CountAsync(cancellationToken);

            if (companyRolesCount == 0)
            {
                return new BranchCreateResponse
                {
                    Commands = request.Commands.Select(
                    command => new BaseResponseGeneric<BranchCreateCommand>
                    {
                        Item = command,
                        IsSuccess = false,
                        Message = "Forbidden"
                    })
                };
            }

            var branches = new List<Branch>();
            foreach (var command in request.Commands)
            {
                var addressId = await _addressRepository
                    .CreateAddressAsync(command.Address);
                branches.Add(new Branch
                {
                    CompanyId = request.CompanyId,
                    Name = command.Name,
                    Description = command.Description,
                    AddressId = addressId,
                    Created = now,
                });
            }

            await _context.Branches.AddRangeAsync(branches, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new BranchCreateResponse
            {
                Commands = request.Commands.Select(
                    command => new BaseResponseGeneric<BranchCreateCommand>
                    {
                        Item = command,
                        IsSuccess = true,
                        Message = "Success"
                    })
            };
        }

    }
}
