using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.Users.Commands.UserLoginIn.Request;
using UseCase.Roles.Users.Commands.UserRegistration.Request;
using UseCase.Shared.Templates.Requests;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> PersonRegistrationAsync(
            UserRegistrationCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserRegistrationRequest
            {
                Command = command,
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return result.IsCreated ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> PersonLoginInAsync(
            UserLoginInCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserLoginInRequest
            {
                Command = command,
                Metadata = HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return result.IsSuccess ? Ok(new { result.Token, result.ValidTo }) : StatusCode(401);
        }

    }
}
