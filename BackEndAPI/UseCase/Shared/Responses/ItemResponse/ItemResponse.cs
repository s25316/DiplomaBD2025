using Domain.Shared.Enums;

namespace UseCase.Shared.Responses.ItemResponse
{
    public class ItemResponse<TItem> : ResponseMetaData where TItem : class
    {
        // Properties
        public required TItem? Result { get; init; }


        // Methods
        public static ItemResponse<TItem> PrepareResponse(
            HttpCode code,
            TItem? item = null)
        {
            return new ItemResponse<TItem>
            {
                HttpCode = code,
                Result = item,
            };
        }
    }
}

