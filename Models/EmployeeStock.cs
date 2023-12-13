using PPE.Data.Enums;

namespace PPE.Models;

public class EmployeeStock
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int StockIn { get; set; }
    public int StockOut { get; set; }
    public StockEmployeeStatus Status { get; set; } = StockEmployeeStatus.Current;
    public string? Remarks { get; set; }
    public bool IsArchived { get; set; } = false;
    public StockType StockType { get; set; } = StockType.Normal;
    public Designation Designation { get; set; } = Designation.Donation;
    public PpeCondition PpeCondition { get; set; } = PpeCondition.Good;
    public int? PpeAttributeCategoryAttributeValueId { get; set; }
    public PpeAttributeCategoryAttributeValue? PpeAttributeCategoryAttributeValue { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; } = null!;
    public int FunctionId { get; set; }
    public Function? Function { get; set; } = null!;
    public int Total => StockIn - StockOut;
   
   // public 
    public virtual ICollection<StockToBePaid> StockToBePaid { get; set; } = null!;
    
    
}