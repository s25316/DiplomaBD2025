using Domain.Features.People.ValueObjects;
using UseCase.RelationalDatabase.Models;
using UseCase.Roles.CompanyUser.Enums;

namespace UseCase.Shared.ExtensionMethods.EF.CompanyPeople
{
    public static class CompanyPeopleEFExtensionMethods
    {
        public static IQueryable<CompanyPerson> WhereAuthorize(
            this IQueryable<CompanyPerson> query,
            PersonId personId,
            IEnumerable<CompanyUserRoles> roles)
        {
            var expression = CompanyPeopleEFExpressions.AuthorizeExpression(personId, roles);
            return query.Where(expression);
        }
    }
}
