using Domain.Shared.CustomProviders;
using System.Linq.Expressions;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.DTOs.Responses.Companies.Offers;
using UseCase.Shared.Requests.QueryParameters;

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

        public static Expression<Func<Offer, bool>> OfferParametersExpression(
            OfferQueryParametersDto offerParameters)
        {
            var now = CustomTimeProvider.Now;
            return offer =>
                (
                    !offerParameters.PublicationStartFrom.HasValue ||
                    offer.PublicationStart >= offerParameters.PublicationStartFrom
                ) &&
                (
                    !offerParameters.PublicationStartTo.HasValue ||
                    offer.PublicationStart <= offerParameters.PublicationStartTo
                ) &&
                (
                    !offerParameters.PublicationEndFrom.HasValue ||
                    offer.PublicationEnd >= offerParameters.PublicationEndFrom
                ) &&
                (
                    !offerParameters.PublicationEndTo.HasValue ||
                    offer.PublicationEnd <= offerParameters.PublicationEndTo
                ) &&
                (
                    !offerParameters.EmploymentLengthFrom.HasValue ||
                    offer.EmploymentLength >= offerParameters.EmploymentLengthFrom
                ) &&
                (
                    !offerParameters.EmploymentLengthTo.HasValue ||
                    offer.EmploymentLength <= offerParameters.EmploymentLengthTo
                ) &&
                (
                    !offerParameters.Status.HasValue ||
                    (
                        offerParameters.Status == OfferStatus.Expired &&
                        offer.PublicationEnd.HasValue &&
                        offer.PublicationEnd < now
                    ) ||
                    (
                        offerParameters.Status == OfferStatus.Active &&
                        offer.PublicationStart <= now &&
                        (
                            !offer.PublicationEnd.HasValue ||
                            offer.PublicationEnd > now
                        )
                    ) ||
                    (
                        offerParameters.Status == OfferStatus.Pending &&
                        offer.PublicationStart > now
                    )
                );
        }
    }
}
