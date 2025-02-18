using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMediator _mediator;


        public ValuesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UseCase.Features.Roles.People.Queries.MyQuery());
            return Ok(result);
        }
    }
}
