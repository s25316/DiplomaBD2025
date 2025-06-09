using UseCase.RelationalDatabase.Models;

namespace UseCase.Shared.ExtensionMethods.EF.People
{
    public static class PersonExtensionMethods
    {
        public static IQueryable<Person> WhereEmail(
            this IQueryable<Person> query,
            string? email)
        {
            var expression = PersonExpression.EmailExpression(email);
            return query.Where(expression);
        }

        public static IQueryable<Person> WhereSearchText(
            this IQueryable<Person> query,
            string? searchText)
        {
            var expression = PersonExpression.SearchTextExpression(searchText);
            return query.Where(expression);
        }

        public static IQueryable<Person> WhereRemoved(
            this IQueryable<Person> query,
            bool? showRemoved)
        {
            var expression = PersonExpression.RemovedExpression(showRemoved);
            return query.Where(expression);
        }
    }
}
