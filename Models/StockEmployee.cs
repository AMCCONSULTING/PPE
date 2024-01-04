namespace PPE.Models;

public class StockEmployee
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int ArticleId { get; set; }
    public int Quantity { get; set; }
    
    public Employee? Employee { get; set; }
    public PpeAttributeCategoryAttributeValue? Article { get; set; }
    
}