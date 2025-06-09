using Domain.Shared.CustomProviders.StringProvider;
using System.Linq.Expressions;
using UseCase.RelationalDatabase.Models;

namespace UseCase.Shared.ExtensionMethods.EF.People
{
    public static class PersonExpression
    {
        public static Expression<Func<Person, bool>> EmailExpression(string? email)
        {
            return person =>
                string.IsNullOrWhiteSpace(email) ||
                (
                    person.Login == email ||
                    person.ContactEmail == email
                );
        }

        public static Expression<Func<Person, bool>> SearchTextExpression(string? searchText)
        {
            var words = CustomStringProvider.Split(searchText, WhiteSpace.All);
            return person =>
                !words.Any() ||
                (
                    words.Any(word =>
                        (
                            person.Name != null &&
                            person.Name.Contains(word)
                        ) ||
                        (
                            person.Surname != null &&
                            person.Surname.Contains(word)
                        ) ||
                        (
                            person.Description != null &&
                            person.Description.Contains(word)
                        )
                    )
                );
        }

        public static Expression<Func<Person, bool>> RemovedExpression(bool? showRemoved)
        {
            return person =>
                !showRemoved.HasValue ||
                (
                    (
                        showRemoved == true &&
                        person.Removed != null
                    ) || (
                        showRemoved == false &&
                        person.Removed == null
                    )
                );
        }
    }
}
