using Domain.Features.People.ValueObjects;
using System.Linq.Expressions;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;

namespace UseCase.Shared.ExtensionMethods.EF.CompanyPeople
{
    public static class CompanyPeopleEFExpressions
    {
        public static Expression<Func<CompanyPerson, bool>> AuthorizeExpression(
            PersonId personId,
            IEnumerable<CompanyUserRoles> roles)
        {
            var personGuid = personId.Value;
            var roleIds = roles.Select(r => (int)r);
            return companyPerson => roleIds.Any(role =>
                companyPerson.RoleId == role &&
                companyPerson.PersonId == personGuid &&
                companyPerson.Deny == null
            );
        }
    }
}
