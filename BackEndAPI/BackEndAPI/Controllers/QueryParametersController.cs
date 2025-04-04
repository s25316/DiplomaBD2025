﻿using Domain.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using UseCase.Roles.CompanyUser.Queries.GetBranches.Enums;
using UseCase.Roles.CompanyUser.Queries.GetCompanies.Enums;
using UseCase.Roles.CompanyUser.Queries.GetContractConditions.Enums;
using UseCase.Roles.CompanyUser.Queries.GetOfferTemplates.Enums;
using UseCase.Shared.DTOs.Responses.Companies.Offers;

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
            var list = EnumExtensionMethods.GetList<CompanyUserCompaniesOrderBy>()
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
            var list = EnumExtensionMethods.GetList<CompanyUserOfferTemplatesOrderBy>()
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
            var list = EnumExtensionMethods.GetList<CompanyUserBranchesOrderBy>()
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
            var list = EnumExtensionMethods.GetList<CompanyUserContractConditionsOrderBy>()
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
