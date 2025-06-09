using AutoMapper;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.Administrators.Queries.AdministratorGetFAQ.Request;
using UseCase.Shared.Dictionaries.GetFaqs.Response;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.ExtensionMethods.EF.Faqs;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Administrators.Queries.AdministratorGetFAQ
{
    public class AdministratorGetFaqHandler : IRequestHandler<AdministratorGetFaqRequest, ItemsResponse<FaqDto>>
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;


        // Constructor
        public AdministratorGetFaqHandler(
            IMapper mapper,
            DiplomaBdContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        // Methods
        public async Task<ItemsResponse<FaqDto>> Handle(AdministratorGetFaqRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Faqs
                .ShowRemoved(request.ShowRemoved)
                .Paginate(request.PaginationQueryParameters);

            query = request.Ascending
                ? query.OrderBy(ex => ex.Created)
                : query.OrderByDescending(ex => ex.Created);

            var items = await query
                .Select(item => new
                {
                    Item = item,
                    TotalCount = _context.Faqs.Count(),
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (!items.Any())
            {
                return PrepareResponse(HttpCode.Ok, [], 0);
            }

            var exceptions = items.Select(item => item.Item).ToList();
            var totalCount = items.FirstOrDefault()?.TotalCount ?? 0;

            return PrepareResponse(
                HttpCode.Ok,
                _mapper.Map<IEnumerable<FaqDto>>(exceptions),
                totalCount);
        }

        // Static Methods
        private static ItemsResponse<FaqDto> PrepareResponse(
            HttpCode code,
            IEnumerable<FaqDto> items,
            int totalCount)
        {
            return ItemsResponse<FaqDto>.PrepareResponse(code, items, totalCount);
        }
    }
}
