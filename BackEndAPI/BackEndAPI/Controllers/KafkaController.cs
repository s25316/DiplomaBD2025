using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaController : ControllerBase
    {
        private readonly IProducer<Null, string> _producer;
        private const string TopicName = "user-logs";

        public KafkaController(IProducer<Null, string> producer)
        {
            _producer = producer;
        }

        [HttpPost]
        public async Task<IActionResult> Post(string message)
        {
            var result = await _producer.ProduceAsync(
                TopicName,
                new Message<Null, string> { Value = message }
                );

            return Ok();
        }

    }
}
