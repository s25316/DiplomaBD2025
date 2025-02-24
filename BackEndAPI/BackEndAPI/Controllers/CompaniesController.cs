using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.CompanyUser.Commands.BranchCreate.Request;
using UseCase.Roles.CompanyUser.Commands.CompanyCreate.Request;

namespace BackEndAPI.Controllers
{
    [Route("api/User/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        //Properties
        private readonly IMediator _mediator;


        // Constructor
        public CompaniesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        // Methods
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CompanyCreateAsync(
            CompanyCreateCommand command,
            CancellationToken cancellationToken)
        {
            var request = new CompanyCreateRequest
            {
                Command = command,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }

        // Methods
        [Authorize]
        [HttpPost("{companyId:guid}/branches")]
        public async Task<IActionResult> BranchCreateAsync(
            Guid companyId,
            BranchCreateCommand command,
            CancellationToken cancellationToken)
        {
            var request = new BranchCreateRequest
            {
                CompanyId = companyId,
                Command = command,
                Metadata = HttpContext,
            };
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(result);
        }
    }
}
