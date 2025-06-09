using AutoMapper;
using Domain.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UseCase.RelationalDatabase;
using UseCase.Roles.Administrators.Queries.AdministratorGetExceptions.Request;
using UseCase.Shared.ExtensionMethods.EF;
using UseCase.Shared.Responses.BaseResponses.Administrator;
using UseCase.Shared.Responses.ItemsResponse;

namespace UseCase.Roles.Administrators.Queries.AdministratorGetExceptions
{
    public class AdministratorGetExceptionsHandler : IRequestHandler<AdministratorGetExceptionsRequest, ItemsResponse<ExceptionDto>>
    {
        // Properties
        private readonly IMapper _mapper;
        private readonly DiplomaBdContext _context;


        // Constructor
        public AdministratorGetExceptionsHandler(
            IMapper mapper,
            DiplomaBdContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        // Methods
        public async Task<ItemsResponse<ExceptionDto>> Handle(AdministratorGetExceptionsRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Exs
                .Paginate(request.PaginationQueryParameters);

            query = request.Ascending
                ? query.OrderBy(ex => ex.Created)
                : query.OrderByDescending(ex => ex.Created);

            var items = await query
                .Select(item => new
                {
                    Item = item,
                    TotalCount = _context.Exs.Count(),
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
                _mapper.Map<IEnumerable<ExceptionDto>>(exceptions),
                totalCount);
        }

        // Static Methods
        private static ItemsResponse<ExceptionDto> PrepareResponse(
            HttpCode code,
            IEnumerable<ExceptionDto> items,
            int totalCount)
        {
            return ItemsResponse<ExceptionDto>.PrepareResponse(code, items, totalCount);
        }
    }
}
