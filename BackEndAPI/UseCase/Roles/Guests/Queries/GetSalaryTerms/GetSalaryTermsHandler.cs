// Ignore Spelling: redis

using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.Guests.Queries.GetSalaryTerms.Request;
using UseCase.Shared.DTOs.Responses;
using UseCase.Shared.Interfaces;

namespace UseCase.Roles.Guests.Queries.GetSalaryTerms
{
    public class GetSalaryTermsHandler : IRequestHandler<GetSalaryTermsRequest, IEnumerable<SalaryTermDto>>
    {
        // Properties
        private readonly DiplomaBdContext _context;
        private readonly IRedisService _redisService;


        // Constructor
        public GetSalaryTermsHandler(DiplomaBdContext context, IRedisService redisService)
        {
            _context = context;
            _redisService = redisService;
        }


        // Methods
        public async Task<IEnumerable<SalaryTermDto>> Handle(GetSalaryTermsRequest request, CancellationToken cancellationToken)
        {
            var redisList = await _redisService.GetAsync<SalaryTermDto>();
            if (redisList.Any())
            {
                return redisList.OrderBy(salaryTerms => salaryTerms.SalaryTermId);
            }

            var databaseList = await _context.SalaryTerms.ToListAsync(cancellationToken);
            var result = databaseList.Select(dbSalaryTerm => new SalaryTermDto
            {
                SalaryTermId = dbSalaryTerm.SalaryTermId,
                Name = dbSalaryTerm.Name,
            });

            var dictionary = result.ToDictionary(
            item => $"{item.SalaryTermId}",
                item => (object)item);
            await _redisService.SetAsync(dictionary);

            return result;
        }
    }
}
