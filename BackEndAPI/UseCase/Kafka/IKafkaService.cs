using UseCase.MongoDb;

namespace UseCase.Kafka
{
    public interface IKafkaService
    {
        Task SendUserLogAsync(BaseLogMongoDb item, CancellationToken cancellationToken);
    }
}
