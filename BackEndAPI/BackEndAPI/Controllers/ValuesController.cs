using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IDistributedCache _cache;


        public ValuesController(
            IMediator mediator,
            IDistributedCache cache)
        {
            _mediator = mediator;
            _cache = cache;
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
            var person = new { Id = 1, Name = "John Doe" };
            var json = JsonSerializer.Serialize(person);
            var bytes = Encoding.UTF8.GetBytes(json);

            await _cache.SetAsync("person:1", bytes, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Cache na 10 minut
            });

            return Ok("Dane zapisane w Redis");
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetCache()
        {
            var bytes = await _cache.GetAsync("person:1");

            if (bytes == null)
                return NotFound("Brak danych w Redis");

            var json = Encoding.UTF8.GetString(bytes);
            var person = JsonSerializer.Deserialize<object>(json);

            return Ok(person);
        }
    }
}
