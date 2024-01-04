namespace PPE.Models;

public class MouvementDetail
{
    public int Id { get; set; }
    public int MouvementId { get; set; }
    public Mouvement Mouvement { get; set; } = null!;
    public int ArticleId { get; set; }
    public PpeAttributeCategoryAttributeValue Article { get; set; } = null!;
    public int Quantity { get; set; }
}