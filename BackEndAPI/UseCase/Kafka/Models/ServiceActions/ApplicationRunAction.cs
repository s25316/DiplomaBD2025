// Ignore spelling: Json

using Domain.Shared.Enums;
using UseCase.Kafka.Enums;

namespace UseCase.Kafka.Models.ServiceActions
{
    public class ApplicationRunAction : ActionTemplate
    {
        // Public Static Methods
        public static ApplicationRunAction Prepare()
        {
            return new ApplicationRunAction
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
