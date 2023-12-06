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
    public int VariantValueId { get; set; }
    public VariantValue VariantValue { get; set; } = null!;
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public int FunctionId { get; set; }
    public Function Function { get; set; } = null!;
    public int Total => StockIn - StockOut;

    /*public EmployeeStock()
    {
        Status = StockEmployeeStatus.Current;
    }*/
}