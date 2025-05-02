using Confluent.Kafka;
using Infrastructure.Exceptions;
using UseCase.Kafka;
using UseCase.MongoDb;

namespace Infrastructure.Kafka
{
    public class KafkaService : IKafkaService
    {
        // Properties
        private static string _topicUserLogs = UseCase.Configuration.KafkaTopicUserLogs;
        private readonly IProducer<Null, string> _kafkaProducer;


        // Constructor
        public KafkaService(IProducer<Null, string> kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }


        // Methods
        public async Task SendUserLogAsync(BaseLogMongoDb item, CancellationToken cancellationToken)
        {
            try
            {
                var message = new Message<Null, string>
                {
                    Value = item.ToJson(),
                };

                _ = await _kafkaProducer.ProduceAsync(_topicUserLogs, message, cancellationToken);
            }
            catch (ProduceException<Null, string> ex)
            {
                throw new InfrastructureLayerException(ex.ToString());
            }
        }
    }
}
