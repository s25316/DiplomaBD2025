using MediatR;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConnectionMultiplexer _redis;
        private readonly IConfiguration _configuration;
        private static int _counter = 1;

        public ValuesController(
            IMediator mediator,
            IConnectionMultiplexer redis,
            IConfiguration configuration)
        {
            _mediator = mediator;
            _redis = redis;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UseCase.Features.Roles.People.Queries.MyQuery());
            return Ok(result);
        }

        [HttpGet("set")]
        public async Task<IActionResult> SetCache()
        {
            var db = _redis.GetDatabase();
            var person = new { Id = _counter, Name = "John Doe" };
            var json = JsonSerializer.Serialize(person);
            var bytes = Encoding.UTF8.GetBytes(json);

            await db.StringSetAsync($"person:{_counter++}", bytes, TimeSpan.FromMinutes(10)); // Cache na 10 minut

            return Ok("Dane zapisane w Redis");
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetCache()
        {
            var redisString = _configuration.GetSection("ConnectionStrings")["Redis"];
            var host = redisString.Split(",")[0].Split(":")[0];
            var port = int.Parse(redisString.Split(",")[0].Split(":")[1]);

            var db = _redis.GetDatabase();

            // Pobranie serwera, na którym będziemy szukać kluczy
            var server = _redis.GetServer(host, port); // Ustaw odpowiedni adres i port

            // Pobranie wszystkich kluczy pasujących do wzorca "person:*"
            var keys = server.Keys(pattern: "person:*").ToList();

            if (!keys.Any())
                return NotFound("Brak danych w Redis");

            var list = new List<object>();

            // Iteracja po kluczach
            foreach (var key in keys)
            {
                // Pobranie wartości skojarzonej z kluczem
                var bytes = await db.StringGetAsync(key);

                if (bytes.IsNullOrEmpty)
                    continue; // Jeśli nie ma danych, przejdź do kolejnego klucza

                var json = Encoding.UTF8.GetString(bytes); // Dekodowanie bajtów na JSON
                var person = JsonSerializer.Deserialize<object>(json); // Deserializacja JSON do obiektu
                list.Add(person);
            }

            return Ok(list);
        }
    }
}
