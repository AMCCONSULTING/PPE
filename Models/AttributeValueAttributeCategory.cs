namespace PPE.Models;

public class AttributeValueAttributeCategory : AuditableEntity
{
    public int Id { get; set; }
    public int AttributeValueId { get; set; }
    public AttributeValue? AttributeValue { get; set; }
    public int AttributeCategoryId { get; set; }
    public AttributeCategory? AttributeCategory { get; set; }
}
