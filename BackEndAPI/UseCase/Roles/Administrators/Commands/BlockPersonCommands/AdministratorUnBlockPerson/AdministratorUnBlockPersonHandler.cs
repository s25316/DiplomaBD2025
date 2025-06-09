using AutoMapper;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.Administrators.Commands.BlockPersonCommands.AdministratorUnBlockPerson.Request;
using UseCase.Shared.Responses.ItemResponse;
using DomainPerson = Domain.Features.People.Aggregates.Person;

namespace UseCase.Roles.Administrators.Commands.BlockPersonCommands.AdministratorUnBlockPerson
{
    public class AdministratorUnBlockPersonHandler : IRequestHandler<AdministratorUnBlockPersonRequest, ResultMetadataResponse>
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly DiplomaBdContext _context;


        // Constructor
        public AdministratorUnBlockPersonHandler(
            IMapper mapper,
            IMediator mediator,
            DiplomaBdContext context)
        {
            _mapper = mapper;
            _mediator = mediator;
            _context = context;
        }


        // Methods
        public async Task<ResultMetadataResponse> Handle(AdministratorUnBlockPersonRequest request, CancellationToken cancellationToken)
        {
            var dbPerson = await _context.People
                .Where(person => person.PersonId == request.PersonId)
                .FirstOrDefaultAsync(cancellationToken);
            if (dbPerson == null)
            {
                return PrepareResponse(HttpCode.NotFound);
            }
            if (dbPerson.Removed.HasValue)
            {
                return PrepareResponse(HttpCode.Gone);
            }


            var domainPerson = _mapper.Map<DomainPerson>(dbPerson);
            if (!domainPerson.HasBlocked)
            {
                return PrepareResponse(HttpCode.Conflict, "Person is not Blocked");
            }
            domainPerson.UnBlock();

            // Updating
            foreach (var @event in domainPerson.DomainEvents)
            {
                await _mediator.Publish(@event, cancellationToken);
            }
            domainPerson.ClearEvents();

            dbPerson.Blocked = domainPerson.Blocked;
            await _context.SaveChangesAsync(cancellationToken);
            return PrepareResponse(HttpCode.Ok);
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
