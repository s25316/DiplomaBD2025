namespace UseCase.Kafka.Models
{
    public abstract class UserKafkaEventTemplate : KafkaEventTemplate
    {
        public required Guid UserId { get; init; }
    }
}
