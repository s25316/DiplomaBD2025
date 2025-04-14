// Ignore Spelling: Dto

using AutoMapper;
using Domain.Features.People.ValueObjects.Ids;
using Domain.Shared.Enums;
using System.Linq.Expressions;
using System.Security.Claims;
using UseCase.RelationalDatabase;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;
using UseCase.Roles.CompanyUser.Queries.Template.Response;
using UseCase.Shared.Services.Authentication.Inspectors;
using UseCase.Shared.Templates.Response.QueryResults;

namespace UseCase.Roles.CompanyUser.Queries.Template
{
    public class GetCompanyUserGenericsBase<TDbEntity, TDto>
    {
        //Properties
        protected readonly DiplomaBdContext _context;
        protected readonly IMapper _mapper;
        protected readonly IAuthenticationInspectorService _authenticationInspector;

        //Constructor
        public GetCompanyUserGenericsBase(
            DiplomaBdContext context,
            IMapper mapper,
            IAuthenticationInspectorService authenticationInspector)
        {
            _context = context;
            _mapper = mapper;
            _authenticationInspector = authenticationInspector;
        }


        // Protected Static Methods       
        protected sealed class SelectResult
        {
            public required TDbEntity Item { get; init; }
            public required int AuthorizedRolesCount { get; init; }
            public required int TotalCount { get; init; }

        }

        protected static Expression<Func<TDbEntity, SelectResult>> BuildSelector(
            PersonId personId,
            IEnumerable<CompanyUserRoles> roles,
            IQueryable<TDbEntity> totalCountQuery,
            Func<TDbEntity, IEnumerable<CompanyPerson>> getCompanyPeople)
        {
            return item => new SelectResult
            {
                Item = item,
                AuthorizedRolesCount = CountAuthorizedRoles(
                    getCompanyPeople(item),
                    personId,
                    roles),
                TotalCount = totalCountQuery.Count()
            };
        }

        // Protected Non Static 
        protected PersonId GetPersonId(IEnumerable<Claim> claims)
        {
            return _authenticationInspector.GetPersonId(claims);
        }

        protected static GetCompanyUserGenericItemsResponse<TDto> PrepareResponse(
            List<SelectResult> items,
            Func<TDbEntity, bool> isRemovedCompany,
            Func<TDbEntity, TDto> map)
        {
            if (items.Count == 0)
            {
                return InvalidResponse(HttpCode.NotFound);
            }

            var totalCount = -1;
            var dtos = new List<TDto>();

            foreach (var selectedItem in items)
            {
                if (totalCount < 0)
                {
                    totalCount = selectedItem.TotalCount;
                }
                if (selectedItem.AuthorizedRolesCount == 0)
                {
                    return InvalidResponse(HttpCode.Forbidden);
                }
                if (isRemovedCompany(selectedItem.Item))
                {
                    return InvalidResponse(HttpCode.Gone);
                }
                dtos.Add(map(selectedItem.Item));
            }

            return ValidResponse(HttpCode.Ok, dtos, totalCount);
        }

        // Private Static Methods
        private static int CountAuthorizedRoles(
           IEnumerable<CompanyPerson> companyPeople,
           PersonId personId,
           IEnumerable<CompanyUserRoles> roles)
        {
            var personIdValue = personId.Value;
            var roleIds = roles.Select(r => (int)r);

            return companyPeople.Count(role =>
                roleIds.Any(roleId =>
                    role.PersonId == personIdValue &&
                    role.RoleId == roleId &&
                    role.Deny == null
                )
            );
        }

        private static GetCompanyUserGenericItemsResponse<TDto> InvalidResponse(HttpCode code)
        {
            return new GetCompanyUserGenericItemsResponse<TDto>
            {
                Result = new ResponseQueryResultTemplate<TDto>
                {
                    Items = [],
                    TotalCount = 0,
                },
                HttpCode = code,
            };
        }

        private static GetCompanyUserGenericItemsResponse<TDto> ValidResponse(
            HttpCode code,
            IEnumerable<TDto> items,
            int totalCount)
        {
            return new GetCompanyUserGenericItemsResponse<TDto>
            {
                Result = new ResponseQueryResultTemplate<TDto>
                {
                    Items = items,
                    TotalCount = totalCount,
                },
                HttpCode = code,
            };
        }
    }
}
