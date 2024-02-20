using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PPE.Data.Enums;

namespace PPE.Models;

public class Employee : AuditableEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    [DisplayName("Matricule")]
    public string Matricule { get; set; }
    [Required]
    [DisplayName("First name")]
    public string FirstName { get; set; } 
    [Required]
    [DisplayName("Last name")]
    public string LastName { get; set; }
    [DisplayName("Full name")]
    public string FullName => $"{FirstName} {LastName}";
    public Size? Size { get; set; }
    public ShoeSize? ShoeSize { get; set; }
    public string? NNI { get; set; }
    public string? Phone { get; set; }
    public string? Tel { get; set; }
    public Gender? Gender { get; set; }
    
    [DisplayName("Is active")]
    public bool IsActive { get; set; } = true;
    
    // Relationship with Project
    [Required] [DisplayName("Project")]
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    
    // Relationship with Function
    [Required] [DisplayName("Function")]
    public int FunctionId { get; set; }
    public Function? Function { get; set; } = null!;
    
    // Relationship with EmployeeStock
    public ICollection<EmployeeStock>? EmployeeStocks { get; set; }
    
    // navigation properties
    public ICollection<Magazinier>? Magaziniers { get; set; }
    public ICollection<Coordinateur>? Coordinateurs { get; set; }
    public ICollection<Transporteur>? Transporteurs { get; set; }
    public ICollection<Responsable>? Responsables { get; set; }
    public ICollection<Hse>? Hses { get; set; }
    public ICollection<StockEmployee>? StockEmployees { get; set; }
    public ICollection<PayableStock>? PayableStocks { get; set; }
    public ICollection<Return>? Returns { get; set; }
    public ICollection<Dotation>? Dotations { get; set; }
    public ICollection<StockToBePaid>? StocksToBePaid { get; set; }
    //public ICollection<DotationDetail> DotationDetails { get; set; }
    
    // createdAt, updatedAt, createdBy
}