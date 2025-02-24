// Ignore Spelling: redis

using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.Guests.Queries.GetWorkModes.Request;
using UseCase.Shared.DTOs.Responses;
using UseCase.Shared.Interfaces;

namespace UseCase.Roles.Guests.Queries.GetWorkModes
{
    class GetWorkModesHandler : IRequestHandler<GetWorkModesRequest, IEnumerable<WorkModeDto>>
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IRedisService _redisService;


        // Constructor
        public GetWorkModesHandler(DiplomaBdContext context, IRedisService redisService)
        {
            _context = context;
            _redisService = redisService;
        }


        // Methods
        public async Task<IEnumerable<WorkModeDto>> Handle(GetWorkModesRequest request, CancellationToken cancellationToken)
        {
            var redisList = await _redisService.GetAsync<WorkModeDto>();
            if (redisList.Any())
            {
                return redisList.OrderBy(workModes => workModes.WorkModeId);
            }

            var databaseList = await _context.WorkModes.ToListAsync(cancellationToken);
            var result = databaseList.Select(dbWorkModel => new WorkModeDto
            {
                WorkModeId = dbWorkModel.WorkModeId,
                Name = dbWorkModel.Name,
            });

            var dictionary = result.ToDictionary(
                item => $"{item.WorkModeId}",
                item => (object)item);
            await _redisService.SetAsync(dictionary);

            return result;
        }
    }
}
