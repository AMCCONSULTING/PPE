namespace PPE.Models;

public class StokeDetail : AuditableEntity
{
    public int Id { get; set; }
    public int StokeId { get; set; }
    public Stoke? Stoke { get; set; } = null!;
    public int ArticleId { get; set; }
    public PpeAttributeCategoryAttributeValue? Article { get; set; } = null!;
    public int Quantity { get; set; }
}