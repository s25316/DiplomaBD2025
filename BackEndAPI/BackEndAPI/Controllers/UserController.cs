using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileActivate.Request;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileCreate.Request;
using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginInAnd2Stage.Request.UserAuthorization2Stage;
using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginInAnd2Stage.Request.UserAuthorizationLoginIn;
using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationRefreshToken.Request;
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
            UserProfileCreateCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserProfileCreateRequest
            {
                Command = command,
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [HttpPost("activation/{urlSegmentPart1:guid}/{urlSegmentPart2}")]
        public async Task<IActionResult> UserProfileActivateAsync(
            Guid urlSegmentPart1,
            string urlSegmentPart2,
            CancellationToken cancellationToken)
        {
            var request = new UserProfileActivateRequest
            {
                Command = new UserProfileActivateCommand
                {
                    UserId = urlSegmentPart1,
                    ActivationUrlSegment = urlSegmentPart2,
                },
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserAuthorizationLoginInAsync(
            UserAuthorizationLoginInCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserAuthorizationLoginInRequest
            {
                Command = command,
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [HttpPost("handPart/{urlSegmentPart1}/{urlSegmentPart2}")]
        public async Task<IActionResult> UserAuthorization2StageAsync(
            Guid urlSegmentPart1,
            string urlSegmentPart2,
            UserAuthorization2StageCodeDto code,
            CancellationToken cancellationToken)
        {
            var request = new UserAuthorization2StageRequest
            {
                Command = new UserAuthorization2StageCommand
                {
                    UrlSegmentPart1 = urlSegmentPart1,
                    UrlSegmentPart2 = urlSegmentPart2,
                    CodeDto = code,
                },
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [HttpPost("refreshToken")]
        public async Task<IActionResult> UserAuthorizationRefreshTokenAsync(
            UserAuthorizationRefreshTokenCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserAuthorizationRefreshTokenRequest
            {
                Command = command,
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }
    }
}
