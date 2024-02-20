using System.ComponentModel;

namespace PPE.Models;

public class AuditableEntity
{
    [DisplayName("Created At")]
    public DateTime? CreatedAt { get; set; }
    [DisplayName("Updated At")]
    public DateTime? UpdatedAt { get; set; }
    [DisplayName("Created By")]
    public string? CreatedBy { get; set; }
    [DisplayName("Updated By")]
    public string? UpdatedBy { get; set; }
}