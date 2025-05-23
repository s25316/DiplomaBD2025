﻿// Ignore Spelling: Regon Krs Dto

namespace UseCase.Shared.Responses.BaseResponses
{
    public class CompanyDto
    {
        public Guid CompanyId { get; init; }

        public string? Logo { get; init; }

        public string? Name { get; init; }

        public string? Description { get; init; }

        public string? Regon { get; init; }

        public string? Nip { get; init; }

        public string? Krs { get; init; }

        public string? WebsiteUrl { get; init; }

        public DateTime Created { get; init; }

        public DateTime? Blocked { get; init; }
    }
}
