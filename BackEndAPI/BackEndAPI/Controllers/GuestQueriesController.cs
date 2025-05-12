using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.Guests.Queries.GuestGetBranches;
using UseCase.Roles.Guests.Queries.GuestGetBranches.Request;
using UseCase.Roles.Guests.Queries.GuestGetCompanies.Request;
using UseCase.Shared.Enums;
using UseCase.Shared.Requests.QueryParameters;

namespace BackEndAPI.Controllers
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
        public async Task<IActionResult> GetCompaniesAsync(
            Guid? companyId,
            string? searchText,
            bool? ascending,
            CompaniesOrderBy? orderBy,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GuestGetCompaniesRequest
            {
                CompanyId = companyId,
                CompanyQueryParameters = companyParameters,

                SearchText = searchText,

                OrderBy = orderBy ?? CompaniesOrderBy.Created,
                Ascending = ascending ?? true,
                Pagination = pagination,

                Metadata = HttpContext,
            }, cancellationToken);

            return StatusCode((int)result.HttpCode, result.Result);
        }


        [HttpGet("branches")]
        [HttpGet("branches/{branchId:guid}")]
        [HttpGet("companies/{companyId:guid}/branches")]
        public async Task<IActionResult> GetUserBranchesAsync(
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
    }
}
