using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.Administrators.Queries.AdministratorGetAdministrators.Request;
using UseCase.Roles.Administrators.Queries.AdministratorGetCompanies.Request;
using UseCase.Roles.Administrators.Queries.AdministratorGetExceptions.Request;
using UseCase.Roles.Administrators.Queries.AdministratorGetFAQ.Request;
using UseCase.Roles.Administrators.Queries.AdministratorGetPeople.Request;
using UseCase.Shared.Enums;
using UseCase.Shared.Requests.QueryParameters;

namespace BackEndAPI.Controllers.Administrator
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class AdministratorQueriesController : ControllerBase
    {
        // Properties
        private readonly IMediator _mediator;


        // Constructor
        public AdministratorQueriesController(IMediator mediator) { _mediator = mediator; }


        // Methods
        [HttpGet("exceptions")]
        public async Task<IActionResult> AdministratorGetExceptionsAsync(
            [FromQuery] PaginationQueryParametersDto PaginationQueryParameters,
            [FromQuery] bool? ascending,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AdministratorGetExceptionsRequest
            {
                PaginationQueryParameters = PaginationQueryParameters,
                Ascending = ascending ?? true,
                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [HttpGet("faq")]
        public async Task<IActionResult> AdministratorGetFaqAsync(
            [FromQuery] PaginationQueryParametersDto PaginationQueryParameters,
            [FromQuery] bool? ascending,
            [FromQuery] bool? showRemoved,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AdministratorGetFaqRequest
            {
                ShowRemoved = showRemoved,
                PaginationQueryParameters = PaginationQueryParameters,
                Ascending = ascending ?? true,
                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [HttpGet("companies")]
        [HttpGet("companies/{companyId:guid}")]
        public async Task<IActionResult> AdministratorGetCompaniesAsync(
            Guid? companyId,
            string? searchText,
            bool? showRemoved,
            bool? ascending,
            CompanyOrderBy? orderBy,
            [FromQuery] CompanyQueryParametersDto companyParameters,
            [FromQuery] PaginationQueryParametersDto pagination,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AdministratorGetCompaniesRequest
            {
                CompanyId = companyId,
                CompanyQueryParameters = companyParameters,

                ShowRemoved = showRemoved,
                SearchText = searchText,

                OrderBy = orderBy ?? CompanyOrderBy.Created,
                Ascending = ascending ?? true,
                Pagination = pagination,

                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [HttpGet("people")]
        [HttpGet("people/{personId:guid}")]
        public async Task<IActionResult> AdministratorGetPeopleAsync(
            Guid? personId,
            string? searchText,
            string? email,
            bool? showRemoved,
            bool? ascending,
            [FromQuery] PaginationQueryParametersDto pagination,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AdministratorGetPeopleRequest
            {
                PersonId = personId,
                Email = email,
                SearchText = searchText,
                ShowRemoved = showRemoved,

                Ascending = ascending ?? true,
                PaginationQueryParameters = pagination,

                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }


        [HttpGet("administrators")]
        [HttpGet("administrators/{personId:guid}")]
        public async Task<IActionResult> AdministratorGetAdministratorsAsync(
            Guid? personId,
            string? searchText,
            string? email,
            bool? showRemoved,
            bool? ascending,
            [FromQuery] PaginationQueryParametersDto pagination,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AdministratorGetAdministratorsRequest
            {
                PersonId = personId,
                Email = email,
                SearchText = searchText,
                ShowRemoved = showRemoved,

                Ascending = ascending ?? true,
                PaginationQueryParameters = pagination,

                Metadata = HttpContext,
            }, cancellationToken);
            return StatusCode((int)result.HttpCode, result.Result);
        }
    }
}
