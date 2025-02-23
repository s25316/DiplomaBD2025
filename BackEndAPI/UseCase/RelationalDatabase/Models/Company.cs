using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class Company
{
    public Guid CompanyId { get; set; }

    public string? Logo { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Regon { get; set; }

    public string? Nip { get; set; }

    public string? Krs { get; set; }

    public string? WebsiteUrl { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public DateTime? Blocked { get; set; }

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual ICollection<CompanyPerson> CompanyPeople { get; set; } = new List<CompanyPerson>();

    public virtual ICollection<Offer> Offers { get; set; } = new List<Offer>();
}
