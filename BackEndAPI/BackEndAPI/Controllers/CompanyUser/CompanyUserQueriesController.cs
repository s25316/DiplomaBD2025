// Ignore Spelling: regon, nip, krs
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UseCase.Roles.CompanyUser.Queries.GetCompanyBranches.Request;
using UseCase.Roles.CompanyUser.Queries.GetCompanyOffers.Request;
using UseCase.Roles.CompanyUser.Queries.GetCompanyOfferTemplates.Request;
using UseCase.Roles.CompanyUser.Queries.GetPersonCompanies.Request;
using UseCase.Shared.Enums;

namespace BackEndAPI.Controllers.CompanyUser
{
    [Route("api/CompanyUser")]
    [ApiController]
    public class CompanyUserQueriesController : ControllerBase
    {
        // Properties
        private readonly IMediator _mediator;


        //Constructor 
        public CompanyUserQueriesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        //Methods
        [Authorize]
        [HttpGet("companies")]
        [HttpGet("companies/{companyId:guid}")]
        public async Task<IActionResult> GetCompaniesAsync(
            Guid? companyId,
            string? searchText,
            string? regon,
            string? nip,
            string? krs,
            CancellationToken cancellationToken,
            [Required] int page = 1,
            [Required] int itemsPerPage = 10,
            [Required] CompaniesOrderBy orderBy = CompaniesOrderBy.Created,
            [Required] bool ascending = true)
        {
            var request = new GetPersonCompaniesRequest
            {
                CompanyId = companyId,
                SearchText = searchText,
                Regon = regon,
                Nip = nip,
                Krs = krs,
                Page = page,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Ascending = ascending,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpGet("branches")]
        [HttpGet("branches/{branchId:guid}")]
        [HttpGet("companies/{companyId:guid}/branches")]
        public async Task<IActionResult> GetBranchesAsync(
            Guid? companyId,
            Guid? branchId,
            string? searchText,
            string? regon,
            string? nip,
            string? krs,
            float? lon,
            float? lat,
            CancellationToken cancellationToken,
            [Required] bool showRemoved = false,
            [Required] int page = 1,
            [Required] int itemsPerPage = 10,
            [Required] BranchesOrderBy orderBy = BranchesOrderBy.BranchCreated,
            [Required] bool ascending = true)
        {
            var request = new GetCompanyBranchesRequest
            {
                CompanyId = companyId,
                BranchId = branchId,
                SearchText = searchText,
                Regon = regon,
                Nip = nip,
                Krs = krs,
                ShowRemoved = showRemoved,
                Page = page,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Ascending = ascending,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("offerTemplates")]
        [HttpGet("companies/{companyId:guid}/offerTemplates")]
        public async Task<IActionResult> GetCompanyOfferTemplatesAsync(
            Guid? companyId,
            Guid? offerTemplateId,
            string? searchText,
            string? regon,
            string? nip,
            string? krs,
            [FromHeader] IEnumerable<int> skillIds,
            CancellationToken cancellationToken,
            [Required] bool showRemoved = false,
            [Required] int page = 1,
            [Required] int itemsPerPage = 10,
            [Required] OfferTemplatesOrderBy orderBy = OfferTemplatesOrderBy.OfferTemplateCreated,
            [Required] bool ascending = true)
        {
            var request = new GetCompanyOfferTemplatesRequest
            {
                CompanyId = companyId,
                OfferTemplateId = offerTemplateId,
                SearchText = searchText,
                Regon = regon,
                Nip = nip,
                Krs = krs,
                SkillIds = skillIds,
                ShowRemoved = showRemoved,
                Page = page,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy,
                Ascending = ascending,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("offers")]
        //[HttpGet("{companyId:guid}/offerTemplates/{offerTemplateId:guid}/offers")]
        public async Task<IActionResult> GetCompanyOffersAsync(
            Guid companyId,
            Guid offerTemplateId,
            DateTime? publicationStart,
            DateTime? publicationEnd,
            DateTime? workBeginDate,
            DateTime? workEndDate,
            float? minSalary,
            float? maxSalary,
            int? salaryTermId,
            int? currencyId,
            bool? isNegotiated,
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
