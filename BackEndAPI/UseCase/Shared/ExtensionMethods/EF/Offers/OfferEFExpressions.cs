using Domain.Features.Offers.Enums;
using Domain.Shared.CustomProviders;
using System.Linq.Expressions;
using UseCase.RelationalDatabase.Models;
using UseCase.Shared.Requests.QueryParameters;

namespace UseCase.Shared.ExtensionMethods.EF.Offers
{
    public static class OfferEFExpressions
    {
        public static Expression<Func<Offer, bool>> OfferStatusExpression(
            OfferStatus? status)
        {
            var now = CustomTimeProvider.Now;
            return offer =>
            (
                !status.HasValue ||
                (
                    status == OfferStatus.Expired &&
                    offer.PublicationEnd.HasValue &&
                    offer.PublicationEnd < now
                ) ||
                (
                    status == OfferStatus.Active &&
                    offer.PublicationStart <= now &&
                    (
                        !offer.PublicationEnd.HasValue ||
                        offer.PublicationEnd > now
                    )
                ) ||
                (
                    status == OfferStatus.Scheduled &&
                    offer.PublicationStart > now
                ));
        }

        public static Expression<Func<Offer, bool>> OfferParametersExpression(
            OfferQueryParametersDto offerParameters)
        {
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
                );
        }
    }
}
