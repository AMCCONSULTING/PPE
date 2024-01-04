using PPE.Data.Enums;

namespace PPE.Models;

public class StockRetun
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int ArticleId { get; set; }
    public int Quantity { get; set; }
    public RetunStatus Status { get; set; }
    
    public Employee? Employee { get; set; }
    public PpeAttributeCategoryAttributeValue? Article { get; set; }
}