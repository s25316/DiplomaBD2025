using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.CompanyUser.Commands.BranchCommands.CompanyUserCreateBranches.Request;
using UseCase.Roles.CompanyUser.Commands.BranchCommands.CompanyUserRemoveBranch.Request;
using UseCase.Roles.CompanyUser.Commands.BranchCommands.CompanyUserUpdateBranch.Request;
using UseCase.Roles.CompanyUser.Commands.CompanyCommands.CompanyUserCreateCompanies.Request;
using UseCase.Roles.CompanyUser.Commands.CompanyCommands.CompanyUserUpdateCompany.Request;
using UseCase.Roles.CompanyUser.Commands.ContractConditionCommands.CompanyUserCreateContractConditions.Request;
using UseCase.Roles.CompanyUser.Commands.ContractConditionCommands.CompanyUserRemoveContractCondition.Request;
using UseCase.Roles.CompanyUser.Commands.ContractConditionCommands.CompanyUserUpdateContractCondition.Request;
using UseCase.Roles.CompanyUser.Commands.OfferCommands.CompanyUserCreateOffers.Request;
using UseCase.Roles.CompanyUser.Commands.OfferCommands.CompanyUserRemoveOffer.Request;
using UseCase.Roles.CompanyUser.Commands.OfferCommands.CompanyUserUpdateOffer.Request;
using UseCase.Roles.CompanyUser.Commands.OfferTemplateCommands.CompanyUserCreateOfferTemplates.Request;
using UseCase.Roles.CompanyUser.Commands.OfferTemplateCommands.CompanyUserRemoveOfferTemplate.Request;
using UseCase.Roles.CompanyUser.Commands.OfferTemplateCommands.CompanyUserUpdateOfferTemplate.Request;
using UseCase.Roles.CompanyUser.Commands.RecruitmentCommands.CompanyUserRecruitmentSetMessage.Request;
using UseCase.Roles.CompanyUser.Commands.RecruitmentCommands.CompanyUserUpdateRecruitment.Request;

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
            CompanyUserCreateCompaniesCommand command,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUserCreateCompaniesRequest
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
            CompanyUserUpdateCompanyCommand command,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUserUpdateCompanyRequest
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
            IEnumerable<CompanyUserCreateBranchesCommand> commands,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUserCreateBranchesRequest
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
            CompanyUserUpdateBranchCommand command,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUserUpdateBranchRequest
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
            var request = new CompanyUserRemoveBranchRequest
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
            IEnumerable<CompanyUserCreateOfferTemplatesCommand> commands,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUserCreateOfferTemplatesRequest
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
            CompanyUserUpdateOfferTemplateCommand command,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUserUpdateOfferTemplateRequest
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
            var request = new CompanyUserRemoveOfferTemplateRequest
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
            IEnumerable<CompanyUserCreateContractConditionsCommand> commands,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUserCreateContractConditionsRequest
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
            CompanyUserUpdateContractConditionCommand command,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUserUpdateContractConditionRequest
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
            var request = new CompanyUserRemoveContractConditionRequest
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
            IEnumerable<CompanyUserCreateOffersCommand> commands,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUserCreateOffersRequest
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
            CompanyUserUpdateOfferCommand command,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUserUpdateOfferRequest
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
            var request = new CompanyUserRemoveOfferRequest
            {
                OfferId = offerId,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpPost("recruitments/{processId:guid}")]
        public async Task<IActionResult> CompanyUserUpdateRecruitmentAsync(
            Guid processId,
            CompanyUserUpdateRecruitmentCommand command,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUserUpdateRecruitmentRequest
            {
                RecruitmentId = processId,
                Command = command,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpPost("recruitments/{processId:guid}/messages")]
        public async Task<IActionResult> UserRecruitmentSetMessageAsync(
            Guid processId,
            CompanyUserRecruitmentSetMessageCommand command,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUserRecruitmentSetMessageRequest
            {
                RecruitmentId = processId,
                Command = command,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }
    }
}
