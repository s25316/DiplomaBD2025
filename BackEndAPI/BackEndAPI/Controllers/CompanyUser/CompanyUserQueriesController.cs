// Ignore Spelling: regon, nip, krs, api
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.CompanyUser.Queries.GetBranches.Enums;
using UseCase.Roles.CompanyUser.Queries.GetBranches.Request;
using UseCase.Roles.CompanyUser.Queries.GetCompanies.Enums;
using UseCase.Roles.CompanyUser.Queries.GetCompanies.Request;
using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Enums;
using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Request;
using UseCase.Roles.CompanyUser.Queries.GetOffers.Enums;
using UseCase.Roles.CompanyUser.Queries.GetOffers.Request;
using UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Enums;
using UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Request;
using UseCase.Shared.DTOs.Responses.Companies.Offers;

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


        // Methods
        [Authorize]
        [HttpGet("companies")]
        [HttpGet("companies/{companyId:guid}")]
        public async Task<IActionResult> GetUserCompaniesAsync(
            Guid? companyId,
            string? regon,
            string? nip,
            string? krs,
            string? searchText,
            int? page,
            int? itemsPerPage,
            CompanyUserCompaniesOrderBy? orderBy,
            bool? ascending,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCompanyUserCompaniesRequest
            {
                CompanyId = companyId,
                Regon = regon,
                Nip = nip,
                Krs = krs,
                SearchText = searchText,
                Page = page ?? 1,
                ItemsPerPage = itemsPerPage ?? 100,
                OrderBy = orderBy ?? CompanyUserCompaniesOrderBy.Created,
                Ascending = ascending ?? true,
                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpGet("branches")]
        [HttpGet("branches/{branchId:guid}")]
        [HttpGet("companies/{companyId:guid}/branches")]
        public async Task<IActionResult> GetUserBranchesAsync(
            Guid? branchId,
            Guid? companyId,
            string? regon,
            string? nip,
            string? krs,
            string? searchText,
            float? lon,
            float? lat,
            bool? showRemoved,
            int? page,
            int? itemsPerPage,
            CompanyUserBranchesOrderBy? orderBy,
            bool? ascending,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCompanyUserBranchesRequest
            {
                BranchId = branchId,
                CompanyId = companyId,
                Regon = regon,
                Nip = nip,
                Krs = krs,
                SearchText = searchText,
                Lon = lon,
                Lat = lat,
                ShowRemoved = showRemoved ?? false,
                Page = page ?? 1,
                ItemsPerPage = itemsPerPage ?? 100,
                OrderBy = orderBy ?? CompanyUserBranchesOrderBy.BranchCreated,
                Ascending = ascending ?? true,
                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpGet("offerTemplates")]
        [HttpGet("offerTemplates/{offerTemplateId:guid}")]
        [HttpGet("companies/{companyId:guid}/offerTemplates")]
        public async Task<IActionResult> GetCompanyOfferTemplatesAsync(
            Guid? offerTemplateId,

            Guid? companyId,
            string? regon,
            string? nip,
            string? krs,

            string? searchText,
            bool? showRemoved,
            [FromHeader] IEnumerable<int> skillIds,

            int? page,
            int? itemsPerPage,
            CompanyUserOfferTemplatesOrderBy? orderBy,
            bool? ascending,
            CancellationToken cancellationToken)
        {
            var request = new GetCompanyUserOfferTemplatesRequest
            {
                OfferTemplateId = offerTemplateId,

                CompanyId = companyId,
                Regon = regon,
                Nip = nip,
                Krs = krs,

                SearchText = searchText,
                SkillIds = skillIds,
                ShowRemoved = showRemoved ?? false,

                Page = page ?? 1,
                ItemsPerPage = itemsPerPage ?? 10,

                OrderBy = orderBy ?? CompanyUserOfferTemplatesOrderBy.OfferTemplateCreated,
                Ascending = ascending ?? true,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpGet("contractConditions")]
        [HttpGet("contractConditions/{contractConditionId:guid}")]
        [HttpGet("companies/{companyId:guid}/contractConditions")]
        public async Task<IActionResult> GetCompanyOfferTemplatesAsync(
            Guid? contractConditionId,

            Guid? companyId,
            string? regon,
            string? nip,
            string? krs,

            string? searchText,
            bool? showRemoved,
            bool? isNegotiable,
            bool? isPaid,
            decimal? salaryPerHourMin,
            decimal? salaryPerHourMax,
            decimal? salaryMin,
            decimal? salaryMax,
            int? hoursMin,
            int? hoursMax,
            [FromHeader] IEnumerable<int> parameterIds,

            int? page,
            int? itemsPerPage,
            CompanyUserContractConditionsOrderBy? orderBy,
            bool? ascending,
            CancellationToken cancellationToken)
        {
            var request = new GetCompanyUserContractConditionsRequest
            {
                ContractConditionId = contractConditionId,
                CompanyId = companyId,
                Regon = regon,
                Nip = nip,
                Krs = krs,

                SearchText = searchText,
                ShowRemoved = showRemoved ?? false,
                IsNegotiable = isNegotiable,
                IsPaid = isPaid,
                SalaryPerHourMin = salaryPerHourMin,
                SalaryPerHourMax = salaryPerHourMax,
                SalaryMin = salaryMin,
                SalaryMax = salaryMax,
                HoursMin = hoursMin,
                HoursMax = hoursMax,
                ParameterIds = parameterIds,

                Page = page ?? 1,
                ItemsPerPage = itemsPerPage ?? 10,

                OrderBy = orderBy ?? CompanyUserContractConditionsOrderBy.ContractConditionCreated,
                Ascending = ascending ?? true,

                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpGet("offers")]
        [HttpGet("offers/{offerId:guid}")]
        [HttpGet("companies/{companyId:guid}/offers")]
        [HttpGet("branches/{branchId:guid}/offers")]
        [HttpGet("contractConditions/{contractConditionId:guid}/offers")]
        [HttpGet("offerTemplates/{offerTemplateId:guid}/offers")]
        public async Task<IActionResult> GetCompanyUserOffersAsync(
            Guid? offerId,

            OfferStatus? status,

            Guid? companyId,
            string? regon,
            string? nip,
            string? krs,

            Guid? branchId,
            float? lon,
            float? lat,

            Guid? contractConditionId,
            bool? isNegotiable,
            bool? isPaid,
            decimal? salaryPerHourMin,
            decimal? salaryPerHourMax,
            decimal? salaryMin,
            decimal? salaryMax,
            int? hoursMin,
            int? hoursMax,
            [FromHeader] IEnumerable<int> parameterIds,

            Guid? offerTemplateId,
            [FromHeader] IEnumerable<int> skillIds,

            string? searchText,

            int? page,
            int? itemsPerPage,

            CompanyUserOffersOrderBy? orderBy,
            bool? ascending,

            CancellationToken cancellationToken)
        {
            var request = new GetCompanyUserOffersRequest
            {
                OfferId = offerId,

                Status = status,

                CompanyId = companyId,
                Regon = regon,
                Nip = nip,
                Krs = krs,

                BranchId = branchId,
                Lon = lon,
                Lat = lat,

                ContractConditionId = contractConditionId,
                IsNegotiable = isNegotiable,
                IsPaid = isPaid,
                SalaryPerHourMin = salaryPerHourMin,
                SalaryPerHourMax = salaryPerHourMax,
                SalaryMin = salaryMin,
                SalaryMax = salaryMax,
                HoursMin = hoursMin,
                HoursMax = hoursMax,
                ParameterIds = parameterIds,

                OfferTemplateId = offerTemplateId,
                SkillIds = skillIds,

                SearchText = searchText,

                Page = page ?? 1,
                ItemsPerPage = itemsPerPage ?? 10,

                OrderBy = orderBy ?? CompanyUserOffersOrderBy.Undefined,
                Ascending = ascending ?? true,

                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }
    }
}
