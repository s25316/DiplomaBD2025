using Domain.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using UseCase.Shared.Enums;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderByController : ControllerBase
    {
        [HttpGet("companies")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetCompaniesOrderBy()
        {
            var list = EnumExtensionMethods.GetList<CompaniesOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }

        [HttpGet("offerTemplates")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetOfferTemplatesOrderBy()
        {
            var list = EnumExtensionMethods.GetList<OfferTemplatesOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }

        [HttpGet("branches")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetBranchesOrderByOrderBy()
        {
            var list = EnumExtensionMethods.GetList<BranchesOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }
    }
}
