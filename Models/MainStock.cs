namespace PPE.Models;

public class MainStock
{
    public int Id { get; set; }
    public PpeAttributeCategoryAttributeValue PpeAttributeCategoryAttributeValue { get; set; } = null!;
    public int PpeAttributeCategoryAttributeValueId { get; set; }
    public int QuantityIn { get; set; }
    public int QuantityOut { get; set; }
    public int QuantityStock { get; set; }
}