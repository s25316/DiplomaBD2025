using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.Guests.Queries.Dictionaries.GetCompanyRoles.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.GetCurrencies.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.GetEmploymentTypes.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.GetFaqs.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.GetNotificationTypes.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.GetSalaryTerms.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.GetSkills.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.GetSkillTypes.Request;
using UseCase.Roles.Guests.Queries.Dictionaries.GetUrlTypes.Requests;
using UseCase.Roles.Guests.Queries.Dictionaries.GetWorkModes.Request;

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
        [HttpGet("offer/workModes")]
        public async Task<IActionResult> GetWorkModesAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetWorkModesRequest(),
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("offer/salaryTerms")]
        public async Task<IActionResult> GetSalaryTermsAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetSalaryTermsRequest(),
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("offer/employmentTypes")]
        public async Task<IActionResult> GetEmploymentTypesAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetEmploymentTypesRequest(),
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("offer/currencies")]
        public async Task<IActionResult> GetCurrenciesAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetCurrenciesRequest(),
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("company/roles")]
        public async Task<IActionResult> GetCompanyRolesAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetCompanyRolesRequest(),
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("skillTypes")]
        public async Task<IActionResult> GetSkillTypesAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetSkillTypesRequest(),
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("skills")]
        public async Task<IActionResult> GetSkillsAsync(
            int? skillTypeId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetSkillsRequest { SkillTypeId = skillTypeId },
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("faq")]
        public async Task<IActionResult> GetFaqsAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetFaqsRequest(),
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("notificationTypes")]
        public async Task<IActionResult> GetNotificationTypesAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetNotificationTypesRequest(),
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("urlTypes")]
        public async Task<IActionResult> GetUrlTypesAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetUrlTypesRequest(),
                cancellationToken);
            return Ok(result);
        }
    }
}
