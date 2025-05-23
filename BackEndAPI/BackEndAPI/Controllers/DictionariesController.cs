using MediatR;
using Microsoft.AspNetCore.Mvc;
using UseCase.Shared.Dictionaries.GetContractParameters.Request;
using UseCase.Shared.Dictionaries.GetContractParameterTypes.Request;
using UseCase.Shared.Dictionaries.GetFaqs.Request;
using UseCase.Shared.Dictionaries.GetProcessTypes.Request;
using UseCase.Shared.Dictionaries.GetSkills.Request;
using UseCase.Shared.Dictionaries.GetSkillTypes.Request;
using UseCase.Shared.Dictionaries.GetUrlTypes.Request;

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
        [HttpGet("contractParameters")]
        public async Task<IActionResult> GetContractParametersAsync(
            int? contractParameterTypeId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetContractParametersRequest
                {
                    ContractParameterTypeId = contractParameterTypeId,
                },
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("contractParameterTypes")]
        public async Task<IActionResult> GetContractParameterTypesAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetContractParameterTypesRequest(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("faqs")]
        public async Task<IActionResult> GetFaqsAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetFaqsRequest(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("skills")]
        public async Task<IActionResult> GetSkillsAsync(
            int? skillTypeId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetSkillsRequest
                {
                    SkillTypeId = skillTypeId,
                },
                cancellationToken);
            return Ok(result);
        }

        [HttpGet("skillTypes")]
        public async Task<IActionResult> GetSkillTypesAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetSkillTypesRequest(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("urlTypes")]
        public async Task<IActionResult> GetUrlTypesAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUrlTypesRequest(), cancellationToken);
            return Ok(result);
        }


        [HttpGet("processTypes")]
        public async Task<IActionResult> GetProcessTypesRequestAsync(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProcessTypesRequest(), cancellationToken);
            return Ok(result);
        }
    }
}
