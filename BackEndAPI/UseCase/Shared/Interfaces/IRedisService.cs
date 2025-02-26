// Ignore Spelling: Redis
namespace UseCase.Shared.Interfaces
{
    public interface IRedisService
    {
        Task SetAsync<TKey, TDto>(Dictionary<TKey, TDto> dictionary)
           where TKey : notnull
           where TDto : class;
        Task<Dictionary<TKey, TDto>> GetAsync<TKey, TDto>(
            Func<TDto, TKey> keySelector)
            where TKey : notnull
            where TDto : class;
    }
}
