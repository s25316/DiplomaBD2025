// Ignore Spelling: Redis
namespace UseCase.Shared.Interfaces
{
    public interface IRedisService
    {
        Task SetAsync(Dictionary<string, object> dictionary);
        Task<IEnumerable<T>> GetAsync<T>() where T : class;
    }
}
