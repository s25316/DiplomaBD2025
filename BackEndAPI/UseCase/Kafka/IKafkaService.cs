using UseCase.Kafka.Models;

namespace UseCase.Kafka
{
    public interface IKafkaService
    {
        Task SendUserLogAsync(KafkaEventTemplate item, CancellationToken cancellationToken);
    }
}
