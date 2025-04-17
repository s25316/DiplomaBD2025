// Ignore spelling: Json

using Domain.Shared.Enums;
using UseCase.Kafka.Enums;

namespace UseCase.Kafka.Models.ServiceActions
{
    public class ApplicationRunKafkaEvent : KafkaEventTemplate
    {
        // Public Static Methods
        public static ApplicationRunKafkaEvent Prepare()
        {
            return new ApplicationRunKafkaEvent
            {
                ActionId = KafkaMongoAction.ApplicationRun,
                Description = KafkaMongoAction.ApplicationRun.Description(),
            };
        }

        // Public Non Static Methods
        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
