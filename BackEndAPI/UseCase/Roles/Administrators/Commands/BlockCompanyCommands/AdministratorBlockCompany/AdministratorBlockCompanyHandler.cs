using AutoMapper;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.Administrators.Commands.BlockCompanyCommands.AdministratorBlockCompany.Request;
using UseCase.Shared.Responses.ItemResponse;
using DomainCompany = Domain.Features.Companies.Entities.Company;

namespace UseCase.Roles.Administrators.Commands.BlockCompanyCommands.AdministratorBlockCompany
{
    public class AdministratorBlockCompanyHandler : IRequestHandler<AdministratorBlockCompanyRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly DiplomaBdContext _context;


        // Constructor
        public AdministratorBlockCompanyHandler(
            IMapper mapper,
            IMediator mediator,
            DiplomaBdContext context)
        {
            _mapper = mapper;
            _mediator = mediator;
            _context = context;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(AdministratorBlockCompanyRequest request, CancellationToken cancellationToken)
        {
            var dbItem = await _context.Companies
                .Where(company => company.CompanyId == request.CompanyId)
                .FirstOrDefaultAsync(cancellationToken);

            if (dbItem == null)
            {
                return PrepareResponse(HttpCode.NotFound);
            }
            if (dbItem.Removed.HasValue)
            {
                return PrepareResponse(HttpCode.Gone);
            }
            if (dbItem.Blocked.HasValue)
            {
                return PrepareResponse(HttpCode.Conflict);
            }


            var domainItem = _mapper.Map<DomainCompany>(dbItem);
            domainItem.Block(request.Command.Message);

            foreach (var @event in domainItem.DomainEvents)
            {
                await _mediator.Publish(@event, cancellationToken);
            }
            domainItem.ClearEvents();
            dbItem.Blocked = domainItem.Blocked;
            await _context.SaveChangesAsync(cancellationToken);
            return PrepareResponse(HttpCode.Ok);
        }

        // Static Methods
        public static ResultMetadataResponse PrepareResponse(
            HttpCode code,
            string? message = null)
        {
            return ResultMetadataResponse.PrepareResponse(code, message);
        }
    }
}
