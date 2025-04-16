namespace KafkaToMongoBroker
{
    public class InputParameters
    {
        public static string KafkaConnetionString { get; private set; }
        public static IEnumerable<string> KafkaTopics { get; private set; } = [];
        public static string MongoConnetionString { get; private set; }

        public InputParameters()
        {
            KafkaConnetionString = Environment.GetEnvironmentVariable("Kafka") ??
                throw new Exception("Not configured connection string to: Kafka");

            MongoConnetionString = Environment.GetEnvironmentVariable("Mongo") ??
                throw new Exception("Not configured connection string to: Mongo");

            Console.WriteLine($"Inside{KafkaConnetionString}");
            Console.WriteLine($"Inside{MongoConnetionString}");
        }
    }
}
