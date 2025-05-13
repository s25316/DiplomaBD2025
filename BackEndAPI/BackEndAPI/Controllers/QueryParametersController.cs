using Domain.Features.Offers.Enums;
using Domain.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetContractConditions;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetOfferTemplates;
using UseCase.Shared.Enums;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryParametersController : ControllerBase
    {
        [HttpGet("CompanyUser/companies/orderBy")]
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

        [HttpGet("CompanyUser/offerTemplates/orderBy")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetOfferTemplatesOrderBy()
        {
            var list = EnumExtensionMethods.GetList<CompanyUserOfferTemplateOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }

        [HttpGet("CompanyUser/branches/orderBy")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetBranchesOrderByOrderBy()
        {
            var list = EnumExtensionMethods.GetList<CompanyUserBranchOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }

        [HttpGet("CompanyUser/contractConditions/orderBy")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetCompanyUserContractConditionsOrderBy()
        {
            var list = EnumExtensionMethods.GetList<CompanyUserContractConditionOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }

        [HttpGet("CompanyUser/offer/status")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetOfferStatuses()
        {
            var list = EnumExtensionMethods.GetList<OfferStatus>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }
    }
}
