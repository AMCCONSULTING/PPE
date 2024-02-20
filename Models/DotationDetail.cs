namespace PPE.Models;

public class DotationDetail : AuditableEntity
{
    public int Id { get; set; }
    public int DotationId { get; set; }
    public int ArticleId { get; set; }
    public int Quantity { get; set; }
    
    public Dotation? Dotation { get; set; }
    public PpeAttributeCategoryAttributeValue? Article { get; set; }
    
}