// Ignore Spelling: regon, nip, krs, api
using Domain.Features.Offers.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches.Request;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetCompanies.Request;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetContractConditions;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetContractConditions.Request;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetOffers.Request;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetOfferTemplates;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetOfferTemplates.Request;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetRecruitmentFile.Request;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetRecruitments.Request;
using UseCase.Shared.Enums;
using UseCase.Shared.Requests.QueryParameters;
using UseCase.Shared.ValidationAttributes.UserAttributes;

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
            CompanyOrderBy? orderBy,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CompanyUserGetCompaniesRequest
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
            CompanyUserBranchOrderBy? orderBy,

            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            [FromQuery] GeographyPointQueryParametersDto geographyPoint,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CompanyUserGetBranchesRequest
            {
                BranchId = branchId,
                CompanyId = companyId,
                CompanyQueryParameters = companyParameters,

                ShowRemoved = showRemoved ?? false,
                SearchText = searchText,

                GeographyPoint = geographyPoint,

                Ascending = ascending ?? true,
                OrderBy = orderBy ?? CompanyUserBranchOrderBy.BranchCreated,
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
            CompanyUserOfferTemplateOrderBy? orderBy,

            [FromHeader] IEnumerable<int> skillIds,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUserGetOfferTemplatesRequest
            {
                OfferTemplateId = offerTemplateId,

                CompanyId = companyId,
                CompanyQueryParameters = companyParameters,

                ShowRemoved = showRemoved ?? false,
                SearchText = searchText,
                SkillIds = skillIds,

                Ascending = ascending ?? true,
                OrderBy = orderBy ?? CompanyUserOfferTemplateOrderBy.OfferTemplateCreated,
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
            CompanyUserContractConditionOrderBy? orderBy,

            [FromHeader] IEnumerable<int> parameterIds,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            [FromQuery] SalaryQueryParametersDto salaryParameters,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUserGetContractConditionsRequest
            {
                ContractConditionId = contractConditionId,

                CompanyId = companyId,
                CompanyQueryParameters = companyParameters,

                ShowRemoved = showRemoved ?? false,
                SearchText = searchText,
                SalaryParameters = salaryParameters,
                ContractParameterIds = parameterIds,

                Ascending = ascending ?? true,
                OrderBy = orderBy ?? CompanyUserContractConditionOrderBy.ContractConditionCreated,
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
        public async Task<IActionResult> CompanyUserGetOffersAsync(
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
            var request = new CompanyUserGetOffersRequest
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
                Status = status,

                Ascending = ascending ?? true,
                OrderBy = orderBy ?? OfferOrderBy.PublicationStart,
                Pagination = pagination,

                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        //[RequestSizeLimit(30 * 1024 * 1024)]
        [HttpGet("recruitments")]
        [HttpGet("recruitments/{processId:guid}")]
        [HttpGet("offers/{offerId:guid}/recruitments")]
        [HttpGet("branches/{branchId:guid}/recruitments")]
        [HttpGet("companies/{companyId:guid}/recruitments")]
        public async Task<IActionResult> CompanyUserGetRecruitmentsAsync(
            Guid? processId,
            Guid? offerId,
            Guid? branchId,
            Guid? companyId,

            [EmailAddress] string? personEmail,
            [PhoneNumber] string? personPhoneNumber,

            string? searchText,

            bool? ascending,
            Domain.Features.Recruitments.Enums.ProcessType? processType,

            [FromHeader] IEnumerable<int> skillIds,
            [FromHeader] IEnumerable<int> contractParameterIds,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] SalaryQueryParametersDto salaryParameters,
            [FromQuery] OfferQueryParametersDto offerParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            CancellationToken cancellationToken)
        {
            var request = new CompanyUserGetRecruitmentsRequest
            {
                OfferId = offerId,
                BranchId = branchId,
                CompanyId = companyId,
                PersonEmail = personEmail,
                PersonPhoneNumber = personPhoneNumber,

                OfferQueryParameters = offerParameters,

                CompanyQueryParameters = companyParameters,
                SalaryParameters = salaryParameters,
                ContractParameterIds = contractParameterIds,

                SkillIds = skillIds,

                SearchText = searchText,
                ProcessType = processType,
                RecruitmentId = processId,

                Ascending = ascending ?? true,
                Pagination = pagination,

                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }

        [Authorize]
        [HttpGet("recruitments/{processId:guid}/file")]
        public async Task<IActionResult> GetCompanyUserOffersAsync(
             Guid processId,
             CancellationToken cancellationToken)
        {
            var request = new CompanyUserGetRecruitmentFileRequest
            {
                RecruitmentId = processId,
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
