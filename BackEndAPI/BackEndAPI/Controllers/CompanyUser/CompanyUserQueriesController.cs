// Ignore Spelling: regon, nip, krs, api
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            // For single company
            Guid? companyId,
            string? regon,
            string? nip,
            string? krs,

            // Other Filters
            string? searchText,

            // Pagination
            int? page,
            int? itemsPerPage,

            // Sorting
            CompaniesOrderBy? orderBy,
            bool? ascending,

            CancellationToken cancellationToken)
        {
            var request = new GetPersonCompaniesRequest
            {
                // For single company
                CompanyId = companyId,
                Regon = regon,
                Nip = nip,
                Krs = krs,

                // Other Filters
                SearchText = searchText,

                // Pagination
                Page = page ?? 1,
                ItemsPerPage = itemsPerPage ?? 10,

                // Sorting
                OrderBy = orderBy ?? CompaniesOrderBy.Created,
                Ascending = ascending ?? true,

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
            // For single Company
            Guid? branchId,

            // Company identification
            Guid? companyId,
            string? regon,
            string? nip,
            string? krs,

            // Other filters
            string? searchText,
            float? lon,
            float? lat,
            bool? showRemoved,

            // Pagination
            int? page,
            int? itemsPerPage,

            // Sorting
            BranchesOrderBy? orderBy,
            bool? ascending,

            CancellationToken cancellationToken)
        {
            var request = new GetCompanyBranchesRequest
            {
                // For single Company
                BranchId = branchId,

                // Company identification
                CompanyId = companyId,
                Regon = regon,
                Nip = nip,
                Krs = krs,

                // Other filters
                SearchText = searchText,
                ShowRemoved = showRemoved ?? false,

                // Pagination
                Page = page ?? 1,
                ItemsPerPage = itemsPerPage ?? 10,

                // Sorting
                OrderBy = orderBy ?? BranchesOrderBy.BranchCreated,
                Ascending = ascending ?? true,

                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpGet("offerTemplates")]
        [HttpGet("offerTemplates/{offerTemplateId:guid}")]
        [HttpGet("companies/{companyId:guid}/offerTemplates")]
        public async Task<IActionResult> GetCompanyOfferTemplatesAsync(
            // For selection single Template
            Guid? offerTemplateId,

            // Company Identification  
            Guid? companyId,
            string? regon,
            string? nip,
            string? krs,

            string? searchText,
            bool? showRemoved,
            [FromHeader] IEnumerable<int> skillIds,

            int? page,
            int? itemsPerPage,

            OfferTemplatesOrderBy? orderBy,
            bool? ascending,

            CancellationToken cancellationToken)
        {
            var request = new GetCompanyOfferTemplatesRequest
            {
                // For selection single Template
                OfferTemplateId = offerTemplateId,

                // Company Identification  
                CompanyId = companyId,
                Regon = regon,
                Nip = nip,
                Krs = krs,

                // Other filters
                SearchText = searchText,
                SkillIds = skillIds,
                ShowRemoved = showRemoved ?? false,

                // Pagination
                Page = page ?? 1,
                ItemsPerPage = itemsPerPage ?? 10,

                // Sorting
                OrderBy = orderBy ?? OfferTemplatesOrderBy.OfferTemplateCreated,
                Ascending = ascending ?? true,

                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpGet("offers")]
        [HttpGet("offers/{offerId:guid}")]
        [HttpGet("branches/{branchId:guid}/offers")]
        [HttpGet("companies/{companyId:guid}/offers")]
        [HttpGet("offerTemplates/{offerTemplateId:guid}/offers")]
        public async Task<IActionResult> GetCompanyOffersAsync(
            // For single Offer
            Guid? offerId,

            // Company Identification
            Guid? companyId,
            string? regon,
            string? nip,
            string? krs,

            // Branch
            Guid? branchId,
            float? lon,
            float? lat,

            // Offer Template
            Guid? offerTemplateId,
            [FromHeader] IEnumerable<int> skillIds,

            string? searchText,
            DateTime? publicationStart,
            DateTime? publicationEnd,
            DateTime? workBeginDate,
            DateTime? workEndDate,
            float? minSalary,
            float? maxSalary,
            bool? isNegotiated,
            int? currencyId,
            int? salaryTermIds,
            [FromHeader] IEnumerable<int> workModeIds,
            [FromHeader] IEnumerable<int> employmentTypeIds,

            CancellationToken cancellationToken,
            int? page,
            int? itemsPerPage,

            bool? ascending)
        {
            var request = new GetCompanyOffersRequest
            {
                // For single Offer
                OfferId = offerId,

                // Company Identification
                CompanyId = companyId,
                Regon = regon,
                Nip = nip,
                Krs = krs,

                // Branch
                BranchId = branchId,
                Lon = lon,
                Lat = lat,

                // Other Filters
                SearchText = searchText,
                PublicationStart = publicationStart,
                PublicationEnd = publicationEnd,
                WorkBegin = workBeginDate,
                WorkEnd = workEndDate,
                MinSalary = minSalary,
                MaxSalary = maxSalary,
                IsNegotiated = isNegotiated,
                CurrencyId = currencyId,

                // Pagination
                Page = page ?? 1,
                ItemsPerPage = itemsPerPage ?? 10,

                // Sorting
                Ascending = ascending ?? true,

                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }
    }
}
