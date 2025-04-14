using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.CompanyUser.Commands.BranchesCreate.Request;
using UseCase.Roles.CompanyUser.Commands.BranchRemove.Request;
using UseCase.Roles.CompanyUser.Commands.BranchUpdate.Request;
using UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Request;
using UseCase.Roles.CompanyUser.Commands.CompanyUpdate.Request;
using UseCase.Roles.CompanyUser.Commands.ContractConditionRemove.Request;
using UseCase.Roles.CompanyUser.Commands.ContractConditionsCreate.Request;
using UseCase.Roles.CompanyUser.Commands.ContractConditionUpdate.Request;
using UseCase.Roles.CompanyUser.Commands.OfferRemove.Request;
using UseCase.Roles.CompanyUser.Commands.OffersCreate.Request;
using UseCase.Roles.CompanyUser.Commands.OfferTemplateRemove.Request;
using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request;
using UseCase.Roles.CompanyUser.Commands.OfferTemplateUpdate.Request;
using UseCase.Roles.CompanyUser.Commands.OfferUpdate.Request;

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
        [HttpPut("{companyId:guid}")]
        public async Task<IActionResult> CompanyUpdateAsync(
            Guid companyId,
            CompanyUpdateCommand command,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUpdateRequest
            {
                CompanyId = companyId,
                Command = command,
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
        [HttpPut("branches/{branchId:guid}")]
        public async Task<IActionResult> BranchUpdateAsync(
            Guid branchId,
            BranchUpdateCommand command,
            CancellationToken cancellationToken)
        {
            var request = new BranchUpdateRequest
            {
                BranchId = branchId,
                Command = command,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpDelete("branches/{branchId:guid}")]
        public async Task<IActionResult> BranchRemoveAsync(
            Guid branchId,
            CancellationToken cancellationToken)
        {
            var request = new BranchRemoveRequest
            {
                BranchId = branchId,
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
        [HttpPut("offerTemplates/{offerTemplateId:guid}")]
        public async Task<IActionResult> OfferTemplateUpdateAsync(
            Guid offerTemplateId,
            OfferTemplateUpdateCommand command,
            CancellationToken cancellationToken)
        {
            var request = new OfferTemplateUpdateRequest
            {
                OfferTemplateId = offerTemplateId,
                Command = command,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpDelete("offerTemplates/{offerTemplateId:guid}")]
        public async Task<IActionResult> OfferTemplateRemoveAsync(
            Guid offerTemplateId,
            CancellationToken cancellationToken)
        {
            var request = new OfferTemplateRemoveRequest
            {
                OfferTemplateId = offerTemplateId,
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
        [HttpPut("contractConditions/{contractConditionId:guid}")]
        public async Task<IActionResult> ContractConditionUpdateAsync(
            Guid contractConditionId,
            ContractConditionsUpdateCommand command,
            CancellationToken cancellationToken)
        {
            var request = new ContractConditionUpdateRequest
            {
                ContractConditionId = contractConditionId,
                Command = command,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpDelete("contractConditions/{contractConditionId:guid}")]
        public async Task<IActionResult> ContractConditionRemoveAsync(
            Guid contractConditionId,
            CancellationToken cancellationToken)
        {
            var request = new ContractConditionRemoveRequest
            {
                ContractConditionId = contractConditionId,
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

        [Authorize]
        [HttpPut("offers/{offerId:guid}")]
        public async Task<IActionResult> OffersCreateAsync(
            Guid offerId,
            OfferUpdateCommand command,
            CancellationToken cancellationToken)
        {
            var request = new OfferUpdateRequest
            {
                OfferId = offerId,
                Command = command,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpDelete("offers/{offerId:guid}")]
        public async Task<IActionResult> OfferRemoveAsync(
            Guid offerId,
            CancellationToken cancellationToken)
        {
            var request = new OfferRemoveRequest
            {
                OfferId = offerId,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }
    }
}
