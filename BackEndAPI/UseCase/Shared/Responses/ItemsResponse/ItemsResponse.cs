using Domain.Shared.Enums;

namespace UseCase.Shared.Responses.ItemsResponse
{
    public class ItemsResponse<TItem> : ResponseMetaData
    {
        // Properties
        public required ItemsResult<TItem> Result { get; init; }


        // Methods
        public static ItemsResponse<TItem> PrepareResponse(
            HttpCode code,
            IEnumerable<TItem> items,
            int totalCount)
        {
            return new ItemsResponse<TItem>
            {
                HttpCode = code,
                Result = new ItemsResult<TItem>
                {
                    Items = items,
                    TotalCount = totalCount,
                }
            };
        }
    }
}
