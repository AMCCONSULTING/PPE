namespace PPE.Models;

public class ProjectStock : AuditableEntity
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;
    public PpeAttributeCategoryAttributeValue PpeAttributeCategoryAttributeValue { get; set; } = null!;
    public int PpeAttributeCategoryAttributeValueId { get; set; }
    public int QuantityIn { get; set; }
    public int QuantityOut { get; set; }
    public int QuantityStock { get; set; }
}