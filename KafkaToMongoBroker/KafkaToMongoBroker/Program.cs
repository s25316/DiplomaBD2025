// Ignore Spelling: Ctrl, Mongo, kafka
using MongoDB.Driver;

namespace KafkaToMongoBroker
{
    internal class Program
    {
        // Properties
        private static ManualResetEvent _exitEvent = new ManualResetEvent(false);
        private static int _timeDelayingRunApp = 2000;

        // Main
        static async Task Main(string[] args)
        {
            // This Need for running Kafka and Mongo
            await Task.Delay(_timeDelayingRunApp);
            _ = MongoDb.ConnectionString;

            using (var cancellationToken = new CancellationTokenSource())
            {
                Console.CancelKeyPress += (sender, e) =>
                {
                    Console.WriteLine("\nGot signal Ctrl+C. Closing...");
                    cancellationToken.Cancel();
                    e.Cancel = true;
                };

                await RunTopicsAsync(Kafka.Topics, cancellationToken.Token);
                _exitEvent.WaitOne();
            }
        }


        //Private Static Methods
        private static async Task RunTopicsAsync(
            IEnumerable<string> topics,
            CancellationToken cancellationToken)
        {
            var tasks = topics.Select(x => RunTopicAsync(x, cancellationToken)).ToList();
            await Task.WhenAll(tasks);
            Console.WriteLine("\nApplication successfully completed.");
        }

        private static async Task RunTopicAsync(
            string topicName,
            CancellationToken cancellationToken)
        {
            Console.WriteLine($"Start topic: {topicName}");
            try
            {
                using (var kafka = new Kafka(topicName))
                {
                    using (var mongo = new MongoDb(topicName))
                    {
                        await kafka.ConsumeAsync(mongo, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Consumer {topicName} \t error: {ex}");
            }
            Console.WriteLine($"Completed topic: {topicName}");
        }

    }
}
