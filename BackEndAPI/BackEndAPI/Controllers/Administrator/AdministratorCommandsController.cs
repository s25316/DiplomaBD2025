using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.Administrators.Commands.AdministrationCommands.AdministratorGrandAdministrator.Request;
using UseCase.Roles.Administrators.Commands.AdministrationCommands.AdministratorRevokeAdministrator.Request;
using UseCase.Roles.Administrators.Commands.BlockCompanyCommands.AdministratorBlockCompany.Request;
using UseCase.Roles.Administrators.Commands.BlockCompanyCommands.AdministratorUnBlockCompany.Request;
using UseCase.Roles.Administrators.Commands.BlockPersonCommands.AdministratorBlockPerson.Request;
using UseCase.Roles.Administrators.Commands.BlockPersonCommands.AdministratorUnBlockPerson.Request;
using UseCase.Roles.Administrators.Commands.FaqCommands.AdministratorCreateFaq.Request;
using UseCase.Roles.Administrators.Commands.FaqCommands.AdministratorDeleteFaq.Request;
using UseCase.Roles.Administrators.Commands.FaqCommands.AdministratorUpdateFaq.Request;

namespace BackEndAPI.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class AdministratorCommandsController : ControllerBase
    {
        // Properties
        private readonly IMediator _mediator;


        // Constructor
        public AdministratorCommandsController(IMediator mediator) { _mediator = mediator; }


        // Methods
        [HttpPost("faq")]
        public async Task<IActionResult> AdministratorCreateFaqAsync(
            AdministratorCreateFaqCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AdministratorCreateFaqRequest
            {
                Command = command,
                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [HttpPut("faq/{faqId:guid}")]
        public async Task<IActionResult> AdministratorUpdateFaqAsync(
            Guid faqId,
            AdministratorUpdateFaqCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AdministratorUpdateFaqRequest
            {
                FaqId = faqId,
                Command = command,
                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [HttpDelete("faq/{faqId:guid}")]
        public async Task<IActionResult> AdministratorDeleteFaqAsync(
            Guid faqId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AdministratorDeleteFaqRequest
            {
                FaqId = faqId,
                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [HttpPost("administrators/{personId:guid}")]
        public async Task<IActionResult> AdministratorGrandAdministratorAsync(
            Guid personId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AdministratorGrandAdministratorRequest
            {
                PersonId = personId,
                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [HttpDelete("administrators/{personId:guid}")]
        public async Task<IActionResult> AdministratorRevokeAdministratorAsync(
            Guid personId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AdministratorRevokeAdministratorRequest
            {
                PersonId = personId,
                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [HttpPost("people/{personId:guid}/block")]
        public async Task<IActionResult> AdministratorBlockPersonAsync(
            Guid personId,
            AdministratorBlockPersonCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AdministratorBlockPersonRequest
            {
                PersonId = personId,
                Command = command,
                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [HttpDelete("people/{personId:guid}/block")]
        public async Task<IActionResult> AdministratorUnBlockPersonAsync(
            Guid personId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AdministratorUnBlockPersonRequest
            {
                PersonId = personId,
                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [HttpPost("company/{companyId:guid}/block")]
        public async Task<IActionResult> AdministratorBlockCompanyAsync(
            Guid companyId,
            AdministratorBlockCompanyCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AdministratorBlockCompanyRequest
            {
                CompanyId = companyId,
                Command = command,
                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [HttpDelete("company/{companyId:guid}/block")]
        public async Task<IActionResult> AdministratorUnBlockCompanyAsync(
            Guid companyId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AdministratorUnBlockCompanyRequest
            {
                CompanyId = companyId,
                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }
    }
}
