using Domain.Features.Offers.Enums;
using Domain.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetBranches;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetContractConditions;
using UseCase.Roles.CompanyUser.Queries.CompanyUserGetOfferTemplates;
using UseCase.Roles.Guests.Queries.GuestGetBranches;
using UseCase.Roles.Guests.Queries.GuestGetContractConditions;
using UseCase.Roles.Guests.Queries.GuestGetOfferTemplates;
using UseCase.Shared.Enums;

namespace BackEndAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryParametersController : ControllerBase
    {
        // Shared
        [HttpGet("companies/orderBy")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetCompanyOrderBy()
        {
            var list = EnumExtensionMethods.GetList<CompanyOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }


        [HttpGet("offers/orderBy")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetOfferOrderBy()
        {
            var list = EnumExtensionMethods.GetList<OfferOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }

        [HttpGet("offer/statuses")]
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

        // Guest
        [HttpGet("Guest/branches/orderBy")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetGuestBranchOrderBy()
        {
            var list = EnumExtensionMethods.GetList<GuestBranchOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }

        [HttpGet("Guest/contractConditions/orderBy")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetGuestContractConditionOrderBy()
        {
            var list = EnumExtensionMethods.GetList<GuestContractConditionOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }

        [HttpGet("Guest/offerTemplates/orderBy")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetGuestOfferTemplateOrderBy()
        {
            var list = EnumExtensionMethods.GetList<GuestOfferTemplateOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }


        // CompanyUser
        [HttpGet("CompanyUser/branches/orderBy")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetCompanyUserBranchOrderBy()
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
        public IActionResult GetCompanyUserContractConditionOrderBy()
        {
            var list = EnumExtensionMethods.GetList<CompanyUserContractConditionOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }

        [HttpGet("CompanyUser/offerTemplates/orderBy")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public IActionResult GetCompanyUserOfferTemplateOrderBy()
        {
            var list = EnumExtensionMethods.GetList<CompanyUserOfferTemplateOrderBy>()
                .Select(@enum => new
                {
                    Id = (int)@enum,
                    Name = @enum.Description(),
                });
            return Ok(list);
        }
    }
}
