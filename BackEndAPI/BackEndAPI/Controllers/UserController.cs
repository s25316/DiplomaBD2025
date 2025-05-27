using Domain.Features.Recruitments.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.MongoDb;
using UseCase.Roles.Users.Commands.AuthorizationCommands.User2StageAuthorization.Request;
using UseCase.Roles.Users.Commands.AuthorizationCommands.UserLoginIn.Request;
using UseCase.Roles.Users.Commands.AuthorizationCommands.UserLogOut.Request;
using UseCase.Roles.Users.Commands.AuthorizationCommands.UserRefreshToken.Request;
using UseCase.Roles.Users.Commands.RecruitmentCommands.UserRecruitsOffer.Request;
using UseCase.Roles.Users.Commands.RegistrationCommands.UserActivatePerson.Request;
using UseCase.Roles.Users.Commands.RegistrationCommands.UserCreatePerson.Request;
using UseCase.Roles.Users.Commands.RemovingCommands.UserRemovePerson.Request;
using UseCase.Roles.Users.Commands.RemovingCommands.UserRestorePerson.Request;
using UseCase.Roles.Users.Commands.UpdatePasswordCommands.UserInitiateUpdatePersonPassword.Request;
using UseCase.Roles.Users.Commands.UpdatePasswordCommands.UserUpdatePersonPasswordAuthorize.Request;
using UseCase.Roles.Users.Commands.UpdatePasswordCommands.UserUpdatePersonPasswordUnAuthorize.Request;
using UseCase.Roles.Users.Commands.UpdatingCommands.UserSetBasePersonData.Request;
using UseCase.Roles.Users.Commands.UpdatingCommands.UserUpdatePersonData.Request;
using UseCase.Roles.Users.Commands.UpdatingCommands.UserUpdatePersonLogin.Request;
using UseCase.Roles.Users.Queries.GetPersonProfile.Request;
using UseCase.Roles.Users.Queries.GetPersonRecruitments.Request;
using UseCase.Shared.Requests;
using UseCase.Shared.Requests.QueryParameters;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMongoDbFileService _mongoDbFileService;

        public UserController(IMediator mediator, IMongoDbFileService mongoDbFileService)
        {
            _mediator = mediator;
            _mongoDbFileService = mongoDbFileService;
        }


        [HttpPost("registration")]
        public async Task<IActionResult> PersonRegistrationAsync(
            UserCreatePersonCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserCreatePersonRequest
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
            var request = new UserActivatePersonRequest
            {
                Command = new UserActivatePersonCommand
                {
                    UserId = urlSegmentPart1,
                    ActivationUrlSegment = Map(urlSegmentPart2),
                },
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserAuthorizationLoginInAsync(
            UserLoginInCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserLoginInRequest
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
            User2StageCodeDto code,
            CancellationToken cancellationToken)
        {
            var request = new User2StageAuthorizationRequest
            {
                Command = new User2StageAuthorizationCommand
                {
                    UrlSegmentPart1 = urlSegmentPart1,
                    UrlSegmentPart2 = Map(urlSegmentPart2),
                    CodeDto = code,
                },
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [HttpPost("refreshToken")]
        public async Task<IActionResult> UserAuthorizationRefreshTokenAsync(
            UserRefreshTokenCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserRefreshTokenRequest
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
            UserInitiateUpdatePersonPasswordCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserInitiateUpdatePersonPasswordRequest
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
            UserUpdatePersonPasswordUnAuthorizeCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserUpdatePersonPasswordUnAuthorizeRequest
            {
                UrlSegment1 = urlSegmentPart1,
                UrlSegment2 = Map(urlSegmentPart2),
                Command = command,
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode);
        }

        [HttpPut("password")]
        public async Task<IActionResult> UserAuthorizationRefreshTokenAsync(
            UserUpdatePersonPasswordAuthorizeCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserUpdatePersonPasswordAuthorizeRequest
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
            var request = new UserRemovePersonRequest
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
            var request = new UserRestorePersonRequest
            {
                UrlSegment1 = urlSegmentPart1,
                UrlSegment2 = Map(urlSegmentPart2),
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode);
        }

        [Authorize]
        [HttpPut("login")]
        public async Task<IActionResult> UserProfileRemoveAsync(
            UserUpdatePersonLoginCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserUpdatePersonLoginRequest
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
            UserSetBasePersonDataCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserSetBasePersonDataRequest
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
            UserUpdatePersonDataCommand command,
            CancellationToken cancellationToken)
        {
            var request = new UserUpdatePersonDataRequest
            {
                Command = command,
                Metadata = (RequestMetadata)HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> GetPersonProfileAsync(
            CancellationToken cancellationToken)
        {
            var request = new GetPersonProfileRequest
            {
                Metadata = (RequestMetadata)HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }


        [Authorize]
        //[RequestSizeLimit(30 * 1024 * 1024)]
        [HttpPost("recruitments/{offerId:guid}")]
        public async Task<IActionResult> UserRecruitsOfferAsync(
            Guid offerId,
            [FromForm] UserRecruitsOfferCommand command,
            CancellationToken cancellationToken)
        {
            var maxFileSize = 25 * 1024 * 1024;
            if (command.File.Length > maxFileSize)
            {
                return BadRequest("File should be less 25MB");
            }
            // Pdf only


            var request = new UserRecruitsOfferRequest
            {
                OfferId = offerId,
                Command = command,
                Metadata = (RequestMetadata)HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        //[RequestSizeLimit(30 * 1024 * 1024)]
        [HttpGet("recruitments")]
        [HttpGet("recruitments/{recruitmentId:guid}")]
        public async Task<IActionResult> GetCompanyUserOffersAsync(
            Guid? recruitmentId,
            string? searchText,

            bool? ascending,
            ProcessType? processType,

            [FromHeader] IEnumerable<int> skillIds,
            [FromHeader] IEnumerable<int> contractParameterIds,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] SalaryQueryParametersDto salaryParameters,
            [FromQuery] OfferQueryParametersDto offerParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            CancellationToken cancellationToken)
        {
            var request = new GetPersonRecruitmentsRequest
            {
                OfferQueryParameters = offerParameters,

                CompanyQueryParameters = companyParameters,
                SalaryParameters = salaryParameters,
                ContractParameterIds = contractParameterIds,

                SkillIds = skillIds,

                SearchText = searchText,
                ProcessType = processType,
                RecruitmentId = recruitmentId,

                Ascending = ascending ?? true,
                Pagination = pagination,

                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        private static string Map(string urlSegment)
        {
            return urlSegment
                .Replace("!", "%21")
                .Replace("#", "%23")
                .Replace("$", "%24")
                .Replace("&", "%26")
                .Replace("'", "%27")
                .Replace("(", "%28")
                .Replace(")", "%29")
                .Replace("*", "%2A")
                .Replace("+", "%2B")
                .Replace(",", "%2C")
                .Replace("/", "%2F")
                .Replace(":", "%3A")
                .Replace(";", "%3B")
                .Replace("=", "%3D")
                .Replace("?", "%3F")
                .Replace("@", "%40")
                .Replace("[", "%5B")
                .Replace("]", "%5D");
        }
    }
}
