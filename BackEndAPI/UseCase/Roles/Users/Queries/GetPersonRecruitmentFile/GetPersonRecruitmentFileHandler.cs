using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.MongoDb;
using UseCase.RelationalDatabase;
using UseCase.Roles.Users.Queries.GetPersonRecruitmentFile.Request;
using UseCase.Shared.Responses.ItemResponse.FileResponses;
using UseCase.Shared.Services.Authentication.Inspectors;

namespace UseCase.Roles.Users.Queries.GetPersonRecruitmentFile
{
    public class GetPersonRecruitmentFileHandler : IRequestHandler<GetPersonRecruitmentFileRequest, FileResponse>
    {
        // Properties 
        private readonly DiplomaBdContext _context;
        private readonly IMongoDbFileService _mongoDbFileService;
        private readonly IAuthenticationInspectorService _authenticationInspector;


        // Constructor 
        public GetPersonRecruitmentFileHandler(
            DiplomaBdContext context,
            IMongoDbFileService mongoDbFileService,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _mongoDbFileService = mongoDbFileService;
            _authenticationInspector = authenticationInspector;
        }


        // Methods
        public async Task<FileResponse> Handle(GetPersonRecruitmentFileRequest request, CancellationToken cancellationToken)
        {
            var peronId = GetPersonId(request);

            var selectResult = await _context.HrProcesses
                .Where(recruitment => recruitment.ProcessId == request.RecruitmentId)
                .FirstOrDefaultAsync(cancellationToken);

            if (selectResult == null)
            {
                return PrepareResponse(HttpCode.NotFound);
            }
            if (selectResult.PersonId != peronId.Value)
            {
                return PrepareResponse(HttpCode.Forbidden);
            }

            var fileDto = await _mongoDbFileService.GetAsync(selectResult.File, cancellationToken);

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

        // Non Static Methods
        private PersonId GetPersonId(GetPersonRecruitmentFileRequest request)
        {
            return _authenticationInspector.GetPersonId(request.Metadata.Claims);
        }
    }
}
