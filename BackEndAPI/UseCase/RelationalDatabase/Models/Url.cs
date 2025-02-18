using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class Url
{
    public int UrlId { get; set; }

    public int UrlTypeId { get; set; }

    public Guid PersonId { get; set; }

    public string? Value { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime Created { get; set; }

    public DateTime? Removed { get; set; }

    public virtual Person Person { get; set; } = null!;

    public virtual UrlType UrlType { get; set; } = null!;
}
