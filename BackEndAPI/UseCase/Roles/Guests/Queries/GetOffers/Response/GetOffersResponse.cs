namespace UseCase.Roles.Guests.Queries.GetOffers.Response
{
    public class GetOffersResponse
    {
        public IEnumerable<OfferDto> Offers { get; set; } = [];
    }
}
