// Ignore Spelling: Kafka, Mongo, KafkaToMongoBroker, Finalizer
using Confluent.Kafka;

namespace KafkaToMongoBroker
{
    public class Kafka : IDisposable
    {
        // Static Properties
        public static string ConnectionString { get; private set; }
        public static IEnumerable<string> Topics { get; private set; } = [];
        public static int MaxBatchSizeBytes { get; private set; } = 1048576;
        public static int MinBatchSizeBytes { get; private set; } = 1;
        public static int MaxPollInterval { get; private set; } = 30_000;
        public static int IntervalOperationMs { get; private set; } = 1000;
        private static int _maxBatchWaitTimeMs = 100;
        public static TimeSpan MaxBatchWaitTimeMs => TimeSpan.FromMilliseconds(_maxBatchWaitTimeMs);

        // Non Static Properties
        private readonly string _topicName;
        private readonly IConsumer<Null, string> _consumer;
        private readonly List<ConsumeResult<Null, string>> _batch = [];
        private bool _disposed = false;


        // Constructor
        static Kafka()
        {
            ConfigureConnectionString("KafkaConnectionString");
            ConfigureTopics("KafkaTopics");
            ConfigureMaxBatchSizeBytes("KafkaMaxBatchSizeMB");
            ConfigureMinBatchSizeBytes("KafkaMinBatchSizeMB");
            ConfigureMaxPollInterval("KafkaMaxPollIntervalSeconds");

            Console.WriteLine($"Kafka {nameof(ConnectionString)}: {ConnectionString}");
            Console.WriteLine($"Kafka {nameof(Topics)}: {string.Join(",", Topics)}");
            Console.WriteLine($"Kafka {nameof(MaxBatchSizeBytes)}: {MaxBatchSizeBytes}");
            Console.WriteLine($"Kafka {nameof(MinBatchSizeBytes)}: {MinBatchSizeBytes}");
            Console.WriteLine($"Kafka {nameof(MaxPollInterval)}: {MaxPollInterval}");
        }

        public Kafka(string topic)
        {
            _topicName = topic;
            _consumer = new ConsumerBuilder<Null, string>(PrepareConsumerConfig(_topicName))
                .Build();
            _consumer.Subscribe(_topicName);
        }


        // Public Non Static Methods
        public async Task ConsumeAsync(
            MongoDb mongo,
            CancellationToken cancellationToken)
        {
            try
            {
                var startTime = DateTime.Now;
                while (!cancellationToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(Kafka.MaxBatchWaitTimeMs);

                    if (consumeResult != null)
                    {
                        _batch.Add(consumeResult);
                    }

                    var now = DateTime.Now;
                    if ((now - startTime).TotalMilliseconds > IntervalOperationMs && _batch.Any())
                    {
                        var items = _batch.Select(pair => pair.Message.Value);
                        await mongo.SaveAsync(items);

                        CommitAndCearBatch();
                        startTime = DateTime.Now;
                    }
                }
            }
            catch (ConsumeException ex)
            {
                Console.WriteLine($"Consumer {_topicName} \t error: {ex.Error.Reason}");
            }
        }

        private void CommitAndCearBatch()
        {
            if (_batch.Any())
            {
                _consumer.Commit(_batch[_batch.Count - 1]);
                _batch.Clear();
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            Console.WriteLine($"Kafka Resources Disposed for topic: {_topicName}");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                _consumer.Close();
                _consumer.Dispose();
            }
            _disposed = true;
        }

        /// <summary>
        /// Finalizer:  Releases unmanaged resources if Dispose() was not called. 
        /// </summary>
        ~Kafka()
        {
            Dispose(false); // Dispose only unmanaged resources.
        }

        // Private Static Methods
        private static ConsumerConfig PrepareConsumerConfig(string topic)
        {
            return new ConsumerConfig
            {
                BootstrapServers = ConnectionString,
                GroupId = $"consumer-of-topic-{topic}",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,
                FetchMaxBytes = MaxBatchSizeBytes,
                FetchMinBytes = MinBatchSizeBytes,
                SessionTimeoutMs = MaxPollInterval / 2,     // 45 sekund (maksymalny czas oczekiwania na heartbeat)
                MaxPollIntervalMs = MaxPollInterval,        // 60 sekund (maksymalny czas na przetwarzanie wiadomości)
                //HeartbeatIntervalMs = 15000,              // 15 sekund (co 15s wysyłamy heartbeat)
            };
        }

        private static string? GetConfigureProperty(string propertyName)
        {
            return Environment.GetEnvironmentVariable(propertyName);
        }

        private static void ConfigureConnectionString(string propertyName)
        {
            ConnectionString = GetConfigureProperty(propertyName) ??
                throw new KeyNotFoundException("Kafka connection string: Not configured");
        }

        private static void ConfigureTopics(string propertyName)
        {
            var topicsString = GetConfigureProperty(propertyName) ??
                throw new KeyNotFoundException("Kafka Topics: Not configured");

            Topics = topicsString
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(topic => topic.Trim());
        }

        private static void ConfigureMaxBatchSizeBytes(string propertyName)
        {
            var maxBatchSizeMbString = GetConfigureProperty(propertyName);

            if (!string.IsNullOrWhiteSpace(maxBatchSizeMbString) &&
                int.TryParse(maxBatchSizeMbString, out var kafkaMaxBatchSizeMb))
            {
                MaxBatchSizeBytes = kafkaMaxBatchSizeMb * 1024 * 1024;
            }
        }

        private static void ConfigureMinBatchSizeBytes(string propertyName)
        {
            var minBatchSizeMbString = GetConfigureProperty(propertyName);

            if (!string.IsNullOrWhiteSpace(minBatchSizeMbString) &&
                int.TryParse(minBatchSizeMbString, out var kafkaMinBatchSizeMb))
            {
                MinBatchSizeBytes = kafkaMinBatchSizeMb * 1024 * 1024;
            }
        }

        private static void ConfigureMaxPollInterval(string propertyName)
        {
            var maxPollIntervalSecondsString = GetConfigureProperty(propertyName);

            if (!string.IsNullOrWhiteSpace(maxPollIntervalSecondsString) &&
                int.TryParse(maxPollIntervalSecondsString, out var kafkaMaxPollIntervalSeconds))
            {
                MaxPollInterval = kafkaMaxPollIntervalSeconds * 1000;
            }
        }

    }
}
