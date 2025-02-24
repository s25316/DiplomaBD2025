using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.Guests.Queries.GetCompanyRoles.Request;
using UseCase.Roles.Guests.Queries.GetCurrencies.Request;
using UseCase.Roles.Guests.Queries.GetEmploymentTypes.Request;
using UseCase.Roles.Guests.Queries.GetSalaryTerms.Request;
using UseCase.Roles.Guests.Queries.GetWorkModes.Request;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DictionariesController : ControllerBase
    {
        //Properties
        private readonly IMediator _mediator;


        // Constructor
        public DictionariesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // Methods
        [HttpGet("workModes")]
        public async Task<IActionResult> GetWorkModesAsync()
        {
            var result = await _mediator.Send(new GetWorkModesRequest());
            return Ok(result);
        }

        [HttpGet("salaryTerms")]
        public async Task<IActionResult> GetSalaryTermsAsync()
        {
            var result = await _mediator.Send(new GetSalaryTermsRequest());
            return Ok(result);
        }

        [HttpGet("employmentTypes")]
        public async Task<IActionResult> GetEmploymentTypesAsync()
        {
            var result = await _mediator.Send(new GetEmploymentTypesRequest());
            return Ok(result);
        }

        [HttpGet("currencies")]
        public async Task<IActionResult> GetCurrenciesAsync()
        {
            var result = await _mediator.Send(new GetCurrenciesRequest());
            return Ok(result);
        }

        [HttpGet("companyRoles")]
        public async Task<IActionResult> GetCompanyRolesAsync()
        {
            var result = await _mediator.Send(new GetCompanyRolesRequest());
            return Ok(result);
        }
    }
}
