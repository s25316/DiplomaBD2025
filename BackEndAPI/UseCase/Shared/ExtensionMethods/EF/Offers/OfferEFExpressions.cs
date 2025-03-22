using System.Linq.Expressions;
using UseCase.RelationalDatabase.Models;

namespace UseCase.Shared.ExtensionMethods.EF.Offers
{
    public static class OfferEFExpressions
    {
        public static Expression<Func<Offer, bool>> SearchTextExpression(
            IEnumerable<string> searchWords)
        {
            return offer =>
                !searchWords.Any() ||
                searchWords.Any(word =>
                    offer.OfferConnections.Any(oc =>
                        oc.OfferTemplate.Name.Contains(word) ||
                        oc.OfferTemplate.Description.Contains(word) ||
                        (
                            oc.OfferTemplate.Company.Name != null &&
                            oc.OfferTemplate.Company.Name.Contains(word)
                        ) ||
                        (
                            oc.OfferTemplate.Company.Description != null &&
                            oc.OfferTemplate.Company.Description.Contains(word)
                        )
                    ) ||
                    (
                        offer.Branch != null &&
                        (
                            (
                                offer.Branch.Name != null &&
                                offer.Branch.Name.Contains(word)
                            ) ||
                            (
                                offer.Branch.Description != null &&
                                offer.Branch.Description.Contains(word)
                            )
                        )
                    ));
        }
    }
}
