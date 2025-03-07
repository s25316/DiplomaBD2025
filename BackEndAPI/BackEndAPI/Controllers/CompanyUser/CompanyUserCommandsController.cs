using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.CompanyUser.Commands.BranchCreate.Request;
using UseCase.Roles.CompanyUser.Commands.CompanyCreate.Request;
using UseCase.Roles.CompanyUser.Commands.OffersCreate.Request;
using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request;

namespace BackEndAPI.Controllers.CompanyUser
{
    [Route("api/CompanyUser/companies")]
    [ApiController]
    public class CompanyUserCommandsController : ControllerBase
    {
        //Properties
        private readonly IMediator _mediator;


        // Constructor
        public CompanyUserCommandsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // Methods
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CompanyCreateAsync(
            CompanyCreateCommand command,
            CancellationToken cancellationToken)
        {
            var request = new CompanyCreateRequest
            {
                Command = command,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Command);
        }

        // Methods
        [Authorize]
        [HttpPost("{companyId:guid}/branches")]
        public async Task<IActionResult> BranchCreateAsync(
            Guid companyId,
            IEnumerable<BranchCreateCommand> commands,
            CancellationToken cancellationToken)
        {
            var request = new BranchCreateRequest
            {
                CompanyId = companyId,
                Commands = commands,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Commands);
        }

        [Authorize]
        [HttpPost("{companyId:guid}/offers/templates")]
        public async Task<IActionResult> OfferTemplatesCreateAsync(
            Guid companyId,
            IEnumerable<OfferTemplateCommand> commands,
            CancellationToken cancellationToken)
        {
            var request = new OfferTemplatesCreateRequest
            {
                CompanyId = companyId,
                Commands = commands,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Commands);
        }

        [Authorize]
        [HttpPost("{companyId:guid}/offers/templates/{offerTemplateId:guid}/offers")]
        public async Task<IActionResult> OffersCreateAsync(
           Guid companyId,
           Guid offerTemplateId,
           IEnumerable<OfferCreateCommand> commands,
           CancellationToken cancellationToken)
        {
            var request = new OffersCreateRequest
            {
                CompanyId = companyId,
                OfferTemplateId = offerTemplateId,
                Commands = commands,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Commands);
        }
    }
}
