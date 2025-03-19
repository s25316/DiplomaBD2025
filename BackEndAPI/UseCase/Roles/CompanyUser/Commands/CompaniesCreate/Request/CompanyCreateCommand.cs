// Ignore Spelling: Krs Regon

namespace UseCase.Roles.CompanyUser.Commands.CompaniesCreate.Request
{
    public class CompanyCreateCommand
    {
        public string? Name { get; init; }

        public string? Description { get; init; }

        public string? Regon { get; init; }

        public string? Nip { get; init; }

        public string? Krs { get; init; }

        public string? WebsiteUrl { get; init; }
    }
}
