namespace UseCase.Roles.Guests.Queries.GetOffers.Response
{
    public class CompanyDto
    {
        public Guid CompanyId { get; set; }
        public string? Logo { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Regon { get; set; } = null!;
        public string Nip { get; set; } = null!;
        public string? Krs { get; set; }
        public string? WebsiteUrl { get; set; }
        public DateTime Created { get; set; }
    }
}
