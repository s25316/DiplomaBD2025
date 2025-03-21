using UseCase.Shared.DTOs.QueryParameters;

namespace UseCase.Shared.ExtensionMethods.EF
{
    public static class EFGenericsExtensionMethods
    {
        public static IQueryable<T> Paginate<T>(
            this IQueryable<T> query,
            PaginationQueryParametersDto pagination) where T : class
        {
            return query.Paginate(pagination.Page, pagination.ItemsPerPage);
        }

        public static IQueryable<T> Paginate<T>(
            this IQueryable<T> query,
            int page,
            int itemsPerPage) where T : class
        {
            return query
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage);
        }
    }
}
