namespace UseCase.Kafka.Models.UserActions
{
    public class UserCreatedKafkaEvent : KafkaEventTemplate
    {


        public override string ToJson()
        {
            return ToJson(this);
        }
    }
}
