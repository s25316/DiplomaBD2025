using MediatR;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Commands.CompanyCreate.Request;
using UseCase.Roles.CompanyUser.Commands.CompanyCreate.Response;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Services.Time;

namespace UseCase.Roles.CompanyUser.Commands.CompanyCreate
{
    public class CompanyCreateHandler : IRequestHandler<CompanyCreateRequest, CompanyCreateResponse>
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly ITimeService _timeService;
        private readonly IAuthenticationInspectorService _authenticationInspector;

        // Constructor
        public CompanyCreateHandler(
            DiplomaBdContext context,
            ITimeService timeService,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _timeService = timeService;
            _authenticationInspector = authenticationInspector;
        }

        // Methods
        async Task<CompanyCreateResponse> IRequestHandler<CompanyCreateRequest, CompanyCreateResponse>.Handle(CompanyCreateRequest request, CancellationToken cancellationToken)
        {
            var now = _timeService.GetNow();
            var userId = _authenticationInspector.GetClaimsName(request.Metadata.Claims);
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new CompanyCreateResponse();
            }

            var databaseCompany = new Company
            {
                Name = request.Command.Name,
                Description = request.Command.Description,
                Regon = request.Command.Regon,
                Nip = request.Command.Nip,
                Krs = request.Command.Krs,
                WebsiteUrl = request.Command.WebsiteUrl,
                Created = now,
            };
            var databaseRole = new CompanyPerson
            {
                Company = databaseCompany,
                PersonId = Guid.Parse(userId),
                RoleId = 1,
                Grant = now,
            };
            await _context.Companies.AddAsync(databaseCompany, cancellationToken);
            await _context.CompanyPeople.AddAsync(databaseRole, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new CompanyCreateResponse
            {
                CompanyId = databaseCompany.CompanyId,
            };
        }
    }
}
