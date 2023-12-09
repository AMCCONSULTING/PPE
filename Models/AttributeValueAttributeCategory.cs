namespace PPE.Models;

public class AttributeValueAttributeCategory{
    public int Id { get; set; }
    public int AttributeValueId { get; set; }
    public AttributeValue? AttributeValue { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
