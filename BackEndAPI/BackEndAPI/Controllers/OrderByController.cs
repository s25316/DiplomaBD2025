using Domain.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.CompanyUser.Queries.GetBranches.Enums;
using UseCase.Roles.CompanyUser.Queries.GetCompanies.Enums;
using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Enums;
using UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Enums;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderByController : ControllerBase
    {
        [HttpGet("CompanyUser/companies")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetCompaniesOrderBy()
        {
            var list = EnumExtensionMethods.GetList<CompanyUserCompaniesOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }

        [HttpGet("CompanyUser/offerTemplates")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetOfferTemplatesOrderBy()
        {
            var list = EnumExtensionMethods.GetList<CompanyUserOfferTemplatesOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }

        [HttpGet("CompanyUser/branches")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetBranchesOrderByOrderBy()
        {
            var list = EnumExtensionMethods.GetList<CompanyUserBranchesOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }

        [HttpGet("CompanyUser/contractConditions")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetCompanyUserContractConditionsOrderBy()
        {
            var list = EnumExtensionMethods.GetList<CompanyUserContractConditionsOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }
    }
}
