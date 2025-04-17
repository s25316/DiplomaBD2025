// Ignore Spelling: Json, ToJson
using Domain.Shared.CustomProviders;
using System.Text.Json;
using UseCase.Kafka.Enums;

namespace UseCase.Kafka.Models
{
    public abstract class KafkaEventTemplate
    {
        // Properties
        public required KafkaMongoAction ActionId { get; init; }
        public required string Description { get; init; } = null!;
        public DateTime Created { get; private init; } = CustomTimeProvider.Now;


        // Methods
        public abstract string ToJson();
        protected static string ToJson(object item)
        {
            return JsonSerializer.Serialize(item);
        }
    }
}
