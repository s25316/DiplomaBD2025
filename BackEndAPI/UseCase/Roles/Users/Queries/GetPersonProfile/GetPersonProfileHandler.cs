using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.Users.Queries.GetPersonProfile.Request;
using UseCase.Shared.DTOs.Responses.People.FullProfile;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.Users.Queries.GetPersonProfile
{
    public class GetPersonProfileHandler : IRequestHandler<GetPersonProfileRequest, FullPersonProfile>
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor
        public GetPersonProfileHandler(
            IMapper mapper,
            DiplomaBdContext context,
            IAuthenticationInspectorService authenticationInspector)
        {
            _mapper = mapper;
            _context = context;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<FullPersonProfile> Handle(GetPersonProfileRequest request, CancellationToken cancellationToken)
        {
            var personId = GetPersonId(request);
            var dbPerson = await _context.People
                .Where(p => p.PersonId == personId.Value)
                .FirstAsync(cancellationToken);
            return _mapper.Map<FullPersonProfile>(dbPerson);
        }

        // Non Static Methods
        private PersonId GetPersonId(GetPersonProfileRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
