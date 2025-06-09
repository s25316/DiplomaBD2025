using Domain.Features.Offers.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.Guests.Queries.GuestGetBranches;
using UseCase.Roles.Guests.Queries.GuestGetBranches.Request;
using UseCase.Roles.Guests.Queries.GuestGetCompanies.Request;
using UseCase.Roles.Guests.Queries.GuestGetCompanyLogo.Request;
using UseCase.Roles.Guests.Queries.GuestGetContractConditions;
using UseCase.Roles.Guests.Queries.GuestGetContractConditions.Request;
using UseCase.Roles.Guests.Queries.GuestGetOffers.Request;
using UseCase.Roles.Guests.Queries.GuestGetOfferTemplates;
using UseCase.Roles.Guests.Queries.GuestGetOfferTemplates.Request;
using UseCase.Shared.Enums;
using UseCase.Shared.Requests.QueryParameters;

namespace BackEndAPI.Controllers.Guest
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestQueriesController : ControllerBase
    {
        // Properties
        private readonly IMediator _mediator;



        // Constructor
        public GuestQueriesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // Public Methods
        [HttpGet("companies")]
        [HttpGet("companies/{companyId:guid}")]
        public async Task<IActionResult> GuestGetCompaniesAsync(
            Guid? companyId,
            string? searchText,
            bool? ascending,
            CompanyOrderBy? orderBy,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GuestGetCompaniesRequest
            {
                CompanyId = companyId,
                CompanyQueryParameters = companyParameters,

                SearchText = searchText,

                OrderBy = orderBy ?? CompanyOrderBy.Created,
                Ascending = ascending ?? true,
                Pagination = pagination,

                Metadata = HttpContext,
            }, cancellationToken);

            return StatusCode((int)result.HttpCode, result.Result);
        }

        [HttpGet("branches")]
        [HttpGet("branches/{branchId:guid}")]
        [HttpGet("companies/{companyId:guid}/branches")]
        public async Task<IActionResult> GuestGetBranchesAsync(
            Guid? branchId,
            Guid? companyId,
            string? searchText,
            bool? ascending,
            GuestBranchOrderBy? orderBy,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            [FromQuery] GeographyPointQueryParametersDto geographyPoint,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GuestGetBranchesRequest
            {
                BranchId = branchId,
                CompanyId = companyId,
                CompanyQueryParameters = companyParameters,

                SearchText = searchText,

                GeographyPoint = geographyPoint,

                Ascending = ascending ?? true,
                OrderBy = orderBy ?? GuestBranchOrderBy.BranchCreated,
                Pagination = pagination,

                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [HttpGet("offerTemplates")]
        [HttpGet("offerTemplates/{offerTemplateId:guid}")]
        [HttpGet("companies/{companyId:guid}/offerTemplates")]
        public async Task<IActionResult> GuestGetOfferTemplatesAsync(
            Guid? companyId,
            Guid? offerTemplateId,

            bool? showRemoved,
            string? searchText,

            bool? ascending,
            GuestOfferTemplateOrderBy? orderBy,

            [FromHeader] IEnumerable<int> skillIds,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            CancellationToken cancellationToken)
        {
            var request = new GuestGetOfferTemplatesRequest
            {
                OfferTemplateId = offerTemplateId,

                CompanyId = companyId,
                CompanyQueryParameters = companyParameters,

                SearchText = searchText,
                SkillIds = skillIds,

                Ascending = ascending ?? true,
                OrderBy = orderBy ?? GuestOfferTemplateOrderBy.OfferTemplateCreated,
                Pagination = pagination,

                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [HttpGet("contractConditions")]
        [HttpGet("contractConditions/{contractConditionId:guid}")]
        [HttpGet("companies/{companyId:guid}/contractConditions")]
        public async Task<IActionResult> GuestGetContractConditionsAsync(
            Guid? companyId,
            Guid? contractConditionId,
            string? searchText,
            bool? showRemoved,
            bool? ascending,
            GuestContractConditionOrderBy? orderBy,

            [FromHeader] IEnumerable<int> parameterIds,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            [FromQuery] SalaryQueryParametersDto salaryParameters,
            CancellationToken cancellationToken)
        {
            var request = new GuestGetContractConditionsRequest
            {
                ContractConditionId = contractConditionId,

                CompanyId = companyId,
                CompanyQueryParameters = companyParameters,

                SearchText = searchText,
                SalaryParameters = salaryParameters,
                ContractParameterIds = parameterIds,

                Ascending = ascending ?? true,
                OrderBy = orderBy ?? GuestContractConditionOrderBy.ContractConditionCreated,
                Pagination = pagination,

                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [HttpGet("offers")]
        [HttpGet("offers/{offerId:guid}")]
        [HttpGet("companies/{companyId:guid}/offers")]
        [HttpGet("branches/{branchId:guid}/offers")]
        [HttpGet("contractConditions/{contractConditionId:guid}/offers")]
        [HttpGet("offerTemplates/{offerTemplateId:guid}/offers")]
        public async Task<IActionResult> GuestGetOffersAsync(
            Guid? offerId,
            Guid? companyId,
            Guid? branchId,
            Guid? offerTemplateId,
            Guid? contractConditionId,

            string? searchText,
            OfferStatus? status,

            bool? ascending,
            OfferOrderBy? orderBy,

            [FromHeader] IEnumerable<int> skillIds,
            [FromHeader] IEnumerable<int> contractParameterIds,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] SalaryQueryParametersDto salaryParameters,
            [FromQuery] GeographyPointQueryParametersDto geographyPoint,
            [FromQuery] OfferQueryParametersDto offerParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            CancellationToken cancellationToken)
        {
            var request = new GuestGetOffersRequest
            {
                OfferId = offerId,
                OfferQueryParameters = offerParameters,

                CompanyId = companyId,
                CompanyQueryParameters = companyParameters,

                BranchId = branchId,
                GeographyPoint = geographyPoint,

                ContractConditionId = contractConditionId,
                SalaryParameters = salaryParameters,
                ContractParameterIds = contractParameterIds,

                OfferTemplateId = offerTemplateId,
                SkillIds = skillIds,

                SearchText = searchText,

                Ascending = ascending ?? true,
                OrderBy = orderBy ?? OfferOrderBy.PublicationStart,
                Pagination = pagination,

                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [HttpGet("companies/{companyId:guid}/logo")]
        public async Task<IActionResult> GetCompanyUserOffersAsync(
            Guid companyId,
            CancellationToken cancellationToken)
        {
            var request = new GuestGetCompanyLogoRequest
            {
                CompanyId = companyId,
                Metadata = HttpContext,
            };

            var result = await _mediator.Send(request, cancellationToken);

            if (result.HttpCode != Domain.Shared.Enums.HttpCode.Ok ||
                result.Result == null)
            {
                return StatusCode((int)result.HttpCode);
            }

            return File(result.Result.Stream, "application/octet-stream", result.Result.FileName);
        }
    }
}
