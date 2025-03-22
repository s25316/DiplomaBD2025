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
using UseCase.Shared.DTOs.QueryParameters;
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
            string? searchText,
            bool? ascending,
            CompanyUserCompaniesOrderBy? orderBy,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCompanyUserCompaniesRequest
            {
                CompanyId = companyId,
                CompanyParameters = companyParameters,

                SearchText = searchText,

                OrderBy = orderBy ?? CompanyUserCompaniesOrderBy.Created,
                Ascending = ascending ?? true,
                Pagination = pagination,

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

            string? searchText,
            bool? showRemoved,

            bool? ascending,
            CompanyUserBranchesOrderBy? orderBy,

            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            [FromQuery] GeographyPointQueryParametersDto geographyPoint,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetCompanyUserBranchesRequest
            {
                BranchId = branchId,
                CompanyId = companyId,
                CompanyParameters = companyParameters,

                ShowRemoved = showRemoved ?? false,
                SearchText = searchText,

                GeographyPoint = geographyPoint,

                Ascending = ascending ?? true,
                OrderBy = orderBy ?? CompanyUserBranchesOrderBy.BranchCreated,
                Pagination = pagination,

                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [Authorize]
        [HttpGet("offerTemplates")]
        [HttpGet("offerTemplates/{offerTemplateId:guid}")]
        [HttpGet("companies/{companyId:guid}/offerTemplates")]
        public async Task<IActionResult> GetCompanyOfferTemplatesAsync(
            Guid? companyId,
            Guid? offerTemplateId,

            bool? showRemoved,
            string? searchText,

            bool? ascending,
            CompanyUserOfferTemplatesOrderBy? orderBy,

            [FromHeader] IEnumerable<int> skillIds,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            CancellationToken cancellationToken)
        {
            var request = new GetCompanyUserOfferTemplatesRequest
            {
                OfferTemplateId = offerTemplateId,

                CompanyId = companyId,
                CompanyParameters = companyParameters,

                ShowRemoved = showRemoved ?? false,
                SearchText = searchText,
                SkillIds = skillIds,

                Ascending = ascending ?? true,
                OrderBy = orderBy ?? CompanyUserOfferTemplatesOrderBy.OfferTemplateCreated,
                Pagination = pagination,

                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpGet("contractConditions")]
        [HttpGet("contractConditions/{contractConditionId:guid}")]
        [HttpGet("companies/{companyId:guid}/contractConditions")]
        public async Task<IActionResult> GetCompanyUserContractConditionsAsync(
            Guid? companyId,
            Guid? contractConditionId,
            string? searchText,
            bool? showRemoved,
            bool? ascending,
            CompanyUserContractConditionsOrderBy? orderBy,

            [FromHeader] IEnumerable<int> parameterIds,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            [FromQuery] SalaryQueryParametersDto salaryParameters,
            CancellationToken cancellationToken)
        {
            var request = new GetCompanyUserContractConditionsRequest
            {
                ContractConditionId = contractConditionId,

                CompanyId = companyId,
                CompanyParameters = companyParameters,

                ShowRemoved = showRemoved ?? false,
                SearchText = searchText,
                SalaryParameters = salaryParameters,
                ContractParameterIds = parameterIds,

                Ascending = ascending ?? true,
                OrderBy = orderBy ?? CompanyUserContractConditionsOrderBy.ContractConditionCreated,
                Pagination = pagination,

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
            Guid? companyId,
            Guid? branchId,
            Guid? offerTemplateId,
            Guid? contractConditionId,

            string? searchText,
            OfferStatus? status,

            bool? ascending,
            CompanyUserOffersOrderBy? orderBy,

            [FromHeader] IEnumerable<int> skillIds,
            [FromHeader] IEnumerable<int> contractParameterIds,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] SalaryQueryParametersDto salaryParameters,
            [FromQuery] GeographyPointQueryParametersDto geographyPoint,
            [FromQuery] OfferQueryParametersDto offerParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            CancellationToken cancellationToken)
        {
            var request = new GetCompanyUserOffersRequest
            {
                OfferId = offerId,
                OfferParameters = offerParameters,

                CompanyId = companyId,
                CompanyParameters = companyParameters,

                BranchId = branchId,
                GeographyPoint = geographyPoint,

                ContractConditionId = contractConditionId,
                SalaryParameters = salaryParameters,
                ContractParameterIds = contractParameterIds,

                OfferTemplateId = offerTemplateId,
                SkillIds = skillIds,

                SearchText = searchText,

                Ascending = ascending ?? true,
                OrderBy = orderBy ?? CompanyUserOffersOrderBy.Undefined,
                Pagination = pagination,

                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }
    }
}
