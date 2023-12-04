namespace PPE.Models;
using PPE.Data.Enums;

public class Manager
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public Fonction Fonction { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
}