namespace UseCase.RelationalDatabase.Models;

public partial class Person
{
    public Guid PersonId { get; set; }

    public Guid? AddressId { get; set; }

    public string? Login { get; set; }

    public string? Logo { get; set; }

    public string? Name { get; set; }

    public string? Surname { get; set; }

    public string? Description { get; set; }

    public string? PhoneNum { get; set; }

    public string? ContactEmail { get; set; }

    public DateOnly? BirthDate { get; set; }

    public bool IsTwoFactorAuth { get; set; }

    public bool IsStudent { get; set; }

    public bool IsAdmin { get; set; }

    public string Password { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public DateTime Created { get; set; }

    public DateTime? Blocked { get; set; }

    public DateTime? Removed { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<CompanyPerson> CompanyPeople { get; set; } = new List<CompanyPerson>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<PersonSkill> PersonSkills { get; set; } = new List<PersonSkill>();

    public virtual ICollection<Url> Urls { get; set; } = new List<Url>();
}
