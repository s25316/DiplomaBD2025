namespace UseCase.Roles.CompanyUser.Commands.OfferCommands.CompanyUserCreateOffers.Request
{
    public class CompanyUserCreateOffersCommand
    {
        public Guid OfferTemplateId { get; init; }

        public Guid? BranchId { get; init; }

        public DateTime PublicationStart { get; init; }

        public DateTime? PublicationEnd { get; init; }

        public float? EmploymentLength { get; init; }

        public string? WebsiteUrl { get; init; }

        public IEnumerable<Guid> ConditionIds { get; init; } = [];
    }
}
