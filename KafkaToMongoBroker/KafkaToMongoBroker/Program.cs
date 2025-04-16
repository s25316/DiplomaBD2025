using Confluent.Kafka;

namespace KafkaToMongoBroker
{
    internal class Program
    {
        private static ManualResetEvent _exitEvent = new ManualResetEvent(false);
        private static string _kafka = null!;
        private static string _mongo = null!;
        private static IEnumerable<string> _topics = [];
        private static int _kafkaMaxBatchSiseBytes = 1048576;
        private static int _kafkaMinBatchSiseBytes = 1;
        private static int _kafkaMaxPollInterval = 30_000;

        static async Task Main(string[] args)
        {
            ConfigureData();
            var inputParamiters = new InputParameters();
            using (var cancellationToken = new CancellationTokenSource())
            {
                Console.CancelKeyPress += (sender, e) =>
                {
                    Console.WriteLine("\nGot signal Ctrl+C. Closing...");
                    cancellationToken.Cancel();
                    e.Cancel = true;
                };

                await RunTopicsAsync(_topics, cancellationToken.Token);
                _exitEvent.WaitOne();
            }
        }

        //Private Static Methods
        private static void ConfigureData()
        {
            //Connection with Services
            _kafka = Environment.GetEnvironmentVariable("Kafka") ??
                throw new Exception("Not configured connection string to: Kafka");
            _mongo = Environment.GetEnvironmentVariable("Mongo") ??
                throw new Exception("Not configured connection string to: Mongo");

            //Get Topics from string and Parse
            var topicsString = Environment.GetEnvironmentVariable("Topics") ??
                throw new Exception("Not configured Topics to: Mongo");

            _topics = topicsString
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(topic => topic.Trim());

            //Kafka Max Batch Size MB
            var kafkaMaxBatchSizeMbString = Environment.GetEnvironmentVariable("KafkaMaxBatchSizeMB");
            if (
                !string.IsNullOrWhiteSpace(kafkaMaxBatchSizeMbString) &&
                int.TryParse(kafkaMaxBatchSizeMbString, out var kafkaMaxBatchSizeMb)
                )
            {
                _kafkaMaxBatchSiseBytes = kafkaMaxBatchSizeMb * 1024 * 1024;
            }

            //Kafka Min Batch Size MB
            var kafkaMinBatchSizeMbString = Environment.GetEnvironmentVariable("KafkaMinBatchSizeMB");
            if (
                !string.IsNullOrWhiteSpace(kafkaMinBatchSizeMbString) &&
                int.TryParse(kafkaMinBatchSizeMbString, out var kafkaMinBatchSizeMb))
            {
                _kafkaMinBatchSiseBytes = kafkaMinBatchSizeMb * 1024 * 1024;
            }

            //Kafka Min Batch Size MB
            var kafkaMaxPollIntervalSecondsString = Environment.GetEnvironmentVariable("KafkaMaxPollIntervalSeconds");
            if (
                !string.IsNullOrWhiteSpace(kafkaMaxPollIntervalSecondsString) &&
                int.TryParse(kafkaMaxPollIntervalSecondsString, out var kafkaMaxPollIntervalSeconds))
            {
                _kafkaMaxPollInterval = kafkaMaxPollIntervalSeconds * 1000;
            }

            Console.WriteLine(_kafkaMaxBatchSiseBytes);
            Console.WriteLine(_kafkaMaxPollInterval);
        }

        private static async Task RunTopicsAsync(
            IEnumerable<string> topics,
            CancellationToken cancellationToken)
        {
            var tasks = topics.Select(x => RunTopic(x, cancellationToken)).ToList();
            await Task.WhenAll(tasks);
            Console.WriteLine("\nApplication successfully completed.");
        }

        private static async Task RunTopic(string topic, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Start topic: {topic}");
            var configuration = new ConsumerConfig
            {
                BootstrapServers = _kafka,
                GroupId = $"consumer-of-topic-{topic}",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                FetchMaxBytes = _kafkaMaxBatchSiseBytes,
                FetchMinBytes = _kafkaMinBatchSiseBytes,
                SessionTimeoutMs = _kafkaMaxPollInterval / 2,       // 45 sekund (maksymalny czas oczekiwania na heartbeat)
                MaxPollIntervalMs = _kafkaMaxPollInterval,      // 60 sekund (maksymalny czas na przetwarzanie wiadomości)
                //HeartbeatIntervalMs = 15000,     // 15 sekund (co 15s wysyłamy heartbeat)
            };

            try
            {
                using (var consumer = new ConsumerBuilder<Null, string>(configuration).Build())
                {
                    consumer.Subscribe(topic);
                    var batch = new List<ConsumeResult<Null, string>>();
                    var batchStartTime = DateTime.UtcNow;

                    try
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            var consumeResult = consumer.Consume(TimeSpan.FromMilliseconds(100));

                            if (consumeResult != null)
                            {
                                batch.Add(consumeResult);
                            }

                            for (int i = 0; i < batch.Count; i++)
                            {
                                Console.WriteLine($"Receive message: '{batch[i].Message.Value}'");
                                if (batch.Count - 1 == i)
                                {
                                    consumer.Commit(batch[i]);
                                }
                            }
                            batch.Clear();
                        }
                        consumer.Close();
                    }
                    catch (ConsumeException ex)
                    {
                        Console.WriteLine($"Consumer {topic} \t error: {ex.Error.Reason}");
                        consumer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Consumer {topic} \t error: {ex}");
            }
            Console.WriteLine($"Completed topic: {topic}");
        }
    }
}
