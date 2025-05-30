using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.MongoDb;
using UseCase.MongoDb.Enums;
using UseCase.RelationalDatabase;
using UseCase.Roles.Guests.Queries.GuestGetCompanyLogo.Request;
using UseCase.Shared.Responses.ItemResponse.FileResponses;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.Guests.Queries.GuestGetCompanyLogo
{
    class GuestGetCompanyLogoHandler : IRequestHandler<GuestGetCompanyLogoRequest, FileResponse>
    {
        // Properties 
        private readonly DiplomaBdContext _context;
        private readonly IMongoDbFileService _mongoDbFileService;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor 
        public GuestGetCompanyLogoHandler(
            DiplomaBdContext context,
            IMongoDbFileService mongoDbFileService,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _mongoDbFileService = mongoDbFileService;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<FileResponse> Handle(GuestGetCompanyLogoRequest request, CancellationToken cancellationToken)
        {
            var selectResult = await _context.Companies
                .Where(company =>
                    company.CompanyId == request.CompanyId &&
                    company.Removed == null &&
                    company.Blocked == null)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (selectResult == null || string.IsNullOrWhiteSpace(selectResult.Logo))
            {
                return PrepareResponse(HttpCode.NotFound);
            }

            var fileDto = await _mongoDbFileService.GetAsync(selectResult.Logo, MongoDbCollection.CompanyLogo, cancellationToken);

            if (fileDto == null)
            {
                return PrepareResponse(HttpCode.NotFound);
            }
            return PrepareResponse(HttpCode.Ok, fileDto);
        }

        // Static Methods
        private static FileResponse PrepareResponse(HttpCode code, FileDto? item = null)
        {
            return FileResponse.PrepareResponse(code, item);
        }
    }
}
