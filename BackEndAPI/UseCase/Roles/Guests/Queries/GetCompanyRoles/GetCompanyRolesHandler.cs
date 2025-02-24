using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.Guests.Queries.GetCompanyRoles.Request;
using UseCase.Shared.DTOs.Responses;
using UseCase.Shared.Interfaces;

namespace UseCase.Roles.Guests.Queries.GetCompanyRoles
{
    public class GetCompanyRolesHandler : IRequestHandler<GetCompanyRolesRequest, IEnumerable<CompanyRoleDto>>
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IRedisService _redisService;


        // Constructor
        public GetCompanyRolesHandler(DiplomaBdContext context, IRedisService redisService)
        {
            _context = context;
            _redisService = redisService;
        }


        // Methods
        public async Task<IEnumerable<CompanyRoleDto>> Handle(GetCompanyRolesRequest request, CancellationToken cancellationToken)
        {
            var redisList = await _redisService.GetAsync<CompanyRoleDto>();
            if (redisList.Any())
            {
                return redisList.OrderBy(role => role.RoleId);
            }

            var databaseList = await _context.Roles.ToListAsync(cancellationToken);
            var result = databaseList.Select(dbRole => new CompanyRoleDto
            {
                RoleId = dbRole.RoleId,
                Name = dbRole.Name,
                Description = dbRole.Description,
            });

            var dictionary = result.ToDictionary(
                item => $"{item.RoleId}",
                item => (object)item);
            await _redisService.SetAsync(dictionary);

            return result;
        }
    }
}
