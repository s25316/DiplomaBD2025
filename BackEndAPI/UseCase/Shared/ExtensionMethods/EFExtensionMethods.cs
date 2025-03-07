namespace UseCase.Shared.ExtensionMethods
{
    public static class EFExtensionMethods
    {
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
