using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PPE.Models;

public class Function
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = null!;
    public int EmployeeCount => Employees?.Count ?? 0;
    public string? Description { get; set; }
    [DisplayName("Created at")]
    public DateTime? CreatedAt { get; set; }
    [DisplayName("Updated at")]
    public DateTime? UpdatedAt { get; set; }
    [DisplayName("Updated by")]
    public string? UpdatedBy { get; set; }
    [DisplayName("Created by")]
    public string? CreatedBy { get; set; }
    public ICollection<Employee>? Employees { get; set; }
    
}