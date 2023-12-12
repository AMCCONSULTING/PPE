namespace PPE.Models;

public class AttributeCategory
{
    public int Id { get; set; }

    // Navigation properties
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    
    public int AttributeId { get; set; }
    public Attribute? Attribute { get; set; }

    public List<AttributeValueAttributeCategory>? AttrValueAttrCategories { get; set; }
}