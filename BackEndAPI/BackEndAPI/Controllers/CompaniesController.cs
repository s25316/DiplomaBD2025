using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.CompanyUser.Commands.BranchCreate.Request;
using UseCase.Roles.CompanyUser.Commands.CompanyCreate.Request;
using UseCase.Roles.CompanyUser.Commands.OffersCreate.Request;
using UseCase.Roles.CompanyUser.Commands.OfferTemplatesCreate.Request;
using UseCase.Roles.Guests.Queries.GetOffers.Request;

namespace BackEndAPI.Controllers
{
    [Route("api/User/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        //Properties
        private readonly IMediator _mediator;


        // Constructor
        public CompaniesController(IMediator mediator)
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
            return Ok(result);
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
            return Ok(result);
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
            return Ok(result);
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
            };
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }
        /*
                [Authorize]
                [HttpPost("{companyId:guid}/offers/")]
                public async Task<IActionResult> x2Async(
                    CancellationToken cancellationToken)
                {
                    return Ok();
                }*/

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetOffersRequest(), cancellationToken);
            return Ok(result);
        }
    }
}
