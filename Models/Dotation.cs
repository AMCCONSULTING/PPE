using System.ComponentModel;
using PPE.Data.Enums;

namespace PPE.Models;

public class Dotation : AuditableEntity
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Reference { get; set; }
    [DisplayName("Employé")]
    public int EmployeeId { get; set; }
    [DisplayName("Coordinateur")]
    public int CoordinatorId { get; set; }
    [DisplayName("Magasinier")]
    public int MagasinierId { get; set; }
    [DisplayName("Type de dotation")]
    public TypeDotation Type { get; set; }
    public string? Document { get; set; }
    [DisplayName("Stock initial")]
    public bool IsFromStock { get; set; } = false;
    public Employee? Employee { get; set; }
    //public Responsable? Responsible { get; set; }
    public Coordinateur? Coordinator { get; set; }
   // public Transporteur? Transporter { get; set; }
    public Magazinier? Magasinier { get; set; }
   
    public ICollection<DotationDetail>? DotationDetails { get; set; }
    
}