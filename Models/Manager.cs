using System.ComponentModel;

namespace PPE.Models;
using PPE.Data.Enums;

public class Manager : AuditableEntity
{
    public int Id { get; set; }
    [DisplayName("First Name")]
    public string FirstName { get; set; }
    [DisplayName("Last Name")]
    public string LastName { get; set; }
    [DisplayName("Phone Number")]
    public string? PhoneNumber { get; set; }
    [DisplayName("Role")]
    public Fonction Fonction { get; set; }
    [DisplayName("Project")]
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    [DisplayName("Full Name")]
    public string FullName => $"{FirstName} {LastName}";
}