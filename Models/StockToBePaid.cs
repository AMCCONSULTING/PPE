namespace PPE.Models;

public class StockToBePaid : AuditableEntity
{
    public int Id { get; set; }
    public int EmployeeStockId { get; set; }
    public EmployeeStock? EmployeeStock { get; set; }
    public bool IsPaid { get; set; } = false;
    
}