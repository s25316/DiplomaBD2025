// Ignore Spelling: Regon, Krs
namespace UseCase.Roles.CompanyUser.Commands.CompanyCreate.Request
{
    public class CompanyCreateCommand
    {
        public string Name { get; init; } = null!;
        public string Description { get; init; } = null!;
        public string Regon { get; init; } = null!;
        public string Nip { get; init; } = null!;
        public string? Krs { get; init; } = null;
        public string? WebsiteUrl { get; init; } = null!;
    }
}
