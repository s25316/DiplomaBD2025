﻿namespace UseCase.RelationalDatabase.Models;

public partial class Country
{
    public int CountryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<State> States { get; set; } = new List<State>();
}
