using Domain.Shared.Enums;
using UseCase.Shared.Templates.Response.Commands;

namespace UseCase.Shared.Templates.Repositories
{
    public sealed class RepositoryCreateResponse<T> where T : class, new()
    {
        // Properties
        public required HttpCode Code { get; init; }
        public required Dictionary<T, ResponseCommandMetadata> Dictionary { get; init; }


        // Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns>Created</returns>
        public static RepositoryCreateResponse<T> ValidResponse(
            IEnumerable<T> items)
        {
            return PrepareResponse(HttpCode.Created, null, items);
        }

        public static RepositoryCreateResponse<T> PrepareResponse(
            HttpCode code,
            Dictionary<T, ResponseCommandMetadata> dictionary)
        {
            return new RepositoryCreateResponse<T>
            {
                Code = code,
                Dictionary = dictionary,
            };
        }

        public static RepositoryCreateResponse<T> PrepareResponse(
            HttpCode code,
            string? description,
            IEnumerable<T> items)
        {
            var intCode = (int)code;
            return new RepositoryCreateResponse<T>
            {
                Code = code,
                Dictionary = items.ToDictionary(
                    item => item,
                    item => new ResponseCommandMetadata
                    {
                        IsCorrect = intCode >= 200 && intCode < 300,
                        Message = description ?? code.Description(),
                    }),
            };
        }
    }
}
