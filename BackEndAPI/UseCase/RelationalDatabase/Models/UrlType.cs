using System;
using System.Collections.Generic;

namespace UseCase.RelationalDatabase.Models;

public partial class UrlType
{
    public int UrlTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Url> Urls { get; set; } = new List<Url>();
}
