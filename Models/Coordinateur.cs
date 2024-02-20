using System.ComponentModel;

namespace PPE.Models;

public class Coordinateur : AuditableEntity
{
    public int Id { get; set; }
    [DisplayName("Employee")]
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; } = null!;
}