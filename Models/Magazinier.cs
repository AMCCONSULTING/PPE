using System.ComponentModel.DataAnnotations;

namespace PPE.Models;

public class Magazinier : AuditableEntity
{
    public int Id { get; set; }
    [Required]
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}