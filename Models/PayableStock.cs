namespace PPE.Models;

public class PayableStock : AuditableEntity
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int ArticleId { get; set; }
    public int Quantity { get; set; }
    public bool IsPaid { get; set; } = false;
    
    public Employee? Employee { get; set; }
    public PpeAttributeCategoryAttributeValue? Article { get; set; }
}