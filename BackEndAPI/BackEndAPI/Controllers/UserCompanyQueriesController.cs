using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Request;
using UseCase.Roles.CompanyUser.Queries.GetCompanyOffers.Request;
using UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates.Request;
using UseCase.Roles.CompanyUser.Queries.GetPersonCompanies.Request;

namespace BackEndAPI.Controllers
{
    [Route("api/User/companies")]
    [ApiController]
    public class UserCompanyQueriesController : ControllerBase
    {
        // Properties
        private readonly IMediator _mediator;


        //Constructor 
        public UserCompanyQueriesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        //Methods
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCompaniesAsync(CancellationToken cancellationToken)
        {
            var request = new GetPersonCompaniesRequest
            {
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{companyId:guid}/branches")]
        public async Task<IActionResult> GetBranchesAsync(
            Guid companyId,
            CancellationToken cancellationToken)
        {
            var request = new GetCompanyBranchesRequest
            {
                CompanyId = companyId,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{companyId:guid}/offerTemplates")]
        public async Task<IActionResult> GetCompanyOfferTemplatesAsync(
            Guid companyId,
            CancellationToken cancellationToken)
        {
            var request = new GetCompanyOfferTemplatesRequest
            {
                CompanyId = companyId,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{companyId:guid}/offerTemplates/{offerTemplateId:guid}")]
        public async Task<IActionResult> GetCompanyOffersAsync(
            Guid companyId,
            Guid offerTemplateId,
            CancellationToken cancellationToken)
        {
            var request = new GetCompanyOffersRequest
            {
                CompanyId = companyId,
                OfferTemplateId = offerTemplateId,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

    }
}
