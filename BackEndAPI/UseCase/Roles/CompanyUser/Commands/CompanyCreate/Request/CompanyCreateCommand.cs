// Ignore Spelling: Regon, Krs
using System.ComponentModel.DataAnnotations;

namespace UseCase.Roles.CompanyUser.Commands.CompanyCreate.Request
{
    public class CompanyCreateCommand
    {
        //[Required]
        [MaxLength(100)]
        public string Name { get; init; } = null!;

        [MaxLength(800)]
        public string? Description { get; init; } = null!;

        //[Required]
        [MaxLength(25)]
        public string Regon { get; init; } = null!;

        //[Required]
        [MaxLength(25)]
        public string Nip { get; init; } = null!;

        [MaxLength(25)]
        public string? Krs { get; init; } = null;

        [MaxLength(800)]
        public string? WebsiteUrl { get; init; } = null!;
    }
}
