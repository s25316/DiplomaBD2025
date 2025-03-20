using UseCase.Shared.ValidationAttributes.PaginationAttributes;

namespace BackEndAPI.QueryParameters
{
    public sealed class PaginationQueryParameters
    {
        [Page]
        public int Page { get; init; }


        [ItemsPerPage]
        public int ItemsPerPage { get; init; }
    }
}
