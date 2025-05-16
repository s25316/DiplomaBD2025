using UseCase.MongoDb;

namespace UseCase.Shared.Interfaces
{
    public interface IKafkaService
    {
        Task SendUserLogAsync(BaseLogMongoDb item, CancellationToken cancellationToken);
    }
}
