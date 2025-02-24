// Ignore Spelling: redis

using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.Guests.Queries.GetEmploymentTypes.Request;
using UseCase.Shared.DTOs.Responses;
using UseCase.Shared.Interfaces;

namespace UseCase.Roles.Guests.Queries.GetEmploymentTypes
{
    class GetEmploymentTypesHandler : IRequestHandler<GetEmploymentTypesRequest, IEnumerable<EmploymentTypeDto>>
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IRedisService _redisService;


        // Constructor
        public GetEmploymentTypesHandler(DiplomaBdContext context, IRedisService redisService)
        {
            _context = context;
            _redisService = redisService;
        }


        // Methods
        public async Task<IEnumerable<EmploymentTypeDto>> Handle(GetEmploymentTypesRequest request, CancellationToken cancellationToken)
        {
            var redisList = await _redisService.GetAsync<EmploymentTypeDto>();
            if (redisList.Any())
            {
                return redisList.OrderBy(employmentType => employmentType.EmploymentTypeId);
            }

            var databaseList = await _context.EmploymentTypes.ToListAsync(cancellationToken);
            var result = databaseList.Select(dbEmploymentType => new EmploymentTypeDto
            {
                EmploymentTypeId = dbEmploymentType.EmploymentTypeId,
                Name = dbEmploymentType.Name,
            });

            var dictionary = result.ToDictionary(
                item => $"{item.EmploymentTypeId}",
                item => (object)item);
            await _redisService.SetAsync(dictionary);

            return result;
        }
    }
}
