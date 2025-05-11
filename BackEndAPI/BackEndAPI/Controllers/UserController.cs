using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileActivate.Request;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileCreate.Request;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileRemove.Request;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordAuthorize.Request;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordInitiate.Request;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileResetPasswordUnAuthorize.Request;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileRestore.Request;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetBaseData.Request;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileSetRegularData.Request;
using UseCase.Roles.Users.Commands.ProfileCommands.UserProfileUpdateLogin.Request;
using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorization2Stage.Request;
using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationLoginIn.Request;
using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserAuthorizationRefreshToken.Request;
using UseCase.Roles.Users.Commands.UserAuthorizationCommands.UserLogOut.Request;
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

        [Authorize]
        [HttpPost("logOut")]
        public async Task<IActionResult> UserLogOutAsync(
            CancellationToken cancellationToken)
        {
            var request = new UserLogOutRequest
            {
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode);
        }


        [HttpPut("password/initiate")]
        public async Task<IActionResult> UserAuthorizationRefreshTokenAsync(
            UserProfileResetPasswordInitiateCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserProfileResetPasswordInitiateRequest
            {
                Command = command,
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode);
        }

        [HttpPut("password/{urlSegmentPart1:guid}/{urlSegmentPart2}")]
        public async Task<IActionResult> UserAuthorizationRefreshTokenAsync(
            Guid urlSegmentPart1,
            string urlSegmentPart2,
            UserProfileResetPasswordUnAuthorizeCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserProfileResetPasswordUnAuthorizeRequest
            {
                UrlSegment1 = urlSegmentPart1,
                UrlSegment2 = urlSegmentPart2,
                Command = command,
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode);
        }

        [HttpPut("password")]
        public async Task<IActionResult> UserAuthorizationRefreshTokenAsync(
            UserProfileResetPasswordAuthorizeCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserProfileResetPasswordAuthorizeRequest
            {
                Command = command,
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode);
        }

        [Authorize]
        [HttpDelete()]
        public async Task<IActionResult> UserProfileRemoveAsync(
            CancellationToken cancellationToken)
        {
            var request = new UserProfileRemoveRequest
            {
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode);
        }

        [HttpPost("restore/{urlSegmentPart1:guid}/{urlSegmentPart2}")]
        public async Task<IActionResult> UserProfileRemoveAsync(
            Guid urlSegmentPart1,
            string urlSegmentPart2,
            CancellationToken cancellationToken)
        {
            var request = new UserProfileRestoreRequest
            {
                UrlSegment1 = urlSegmentPart1,
                UrlSegment2 = urlSegmentPart2,
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode);
        }

        [Authorize]
        [HttpPut("login")]
        public async Task<IActionResult> UserProfileRemoveAsync(
            UserProfileUpdateLoginCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserProfileUpdateLoginRequest
            {
                Command = command,
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpPut("baseData")]
        public async Task<IActionResult> UserProfileSetBaseDataAsync(
            UserProfileSetBaseDataCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserProfileSetBaseDataRequest
            {
                Command = command,
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [Authorize]
        [HttpPut("regularData")]
        public async Task<IActionResult> UserProfileSetRegularDataAsync(
            UserProfileSetRegularDataCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserProfileSetRegularDataRequest
            {
                Command = command,
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }
    }
}
