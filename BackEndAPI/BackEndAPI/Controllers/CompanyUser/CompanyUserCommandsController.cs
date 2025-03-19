using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.CompanyUser.Commands.BranchesCreate.Request;
using UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Request;
using UseCase.Roles.CompanyUser.Commands.ContractConditionsCreate.Request;
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
        public async Task<IActionResult> CompaniesCreateAsync(
            CompanyCreateCommand command,
            CancellationToken cancellationToken)
        {
            var request = new CompaniesCreateRequest
            {
                Commands = [command],
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpPost("{companyId:guid}/branches")]
        public async Task<IActionResult> BranchesCreateAsync(
            Guid companyId,
            IEnumerable<BranchCreateCommand> commands,
            CancellationToken cancellationToken)
        {
            var request = new BranchesCreateRequest
            {
                CompanyId = companyId,
                Commands = commands,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpPost("{companyId:guid}/offerTemplates")]
        public async Task<IActionResult> OfferTemplatesCreateAsync(
            Guid companyId,
            IEnumerable<OfferTemplateCreateCommand> commands,
            CancellationToken cancellationToken)
        {
            var request = new OfferTemplatesCreateRequest
            {
                CompanyId = companyId,
                Commands = commands,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpPost("{companyId:guid}/contractConditions")]
        public async Task<IActionResult> ContractConditionsCreateAsync(
            Guid companyId,
            IEnumerable<ContractConditionsCreateCommand> commands,
            CancellationToken cancellationToken)
        {
            var request = new ContractConditionsCreateRequest
            {
                CompanyId = companyId,
                Commands = commands,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpPost("offers")]
        public async Task<IActionResult> OffersCreateAsync(
            IEnumerable<OfferCreateCommand> commands,
            CancellationToken cancellationToken)
        {
            var request = new OffersCreateRequest
            {
                Commands = commands,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }
    }
}
