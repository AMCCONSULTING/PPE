namespace PPE.Models;

public class AttributeValue
{
    public int Id { get; set; }

    // Foreign keys
    public int AttributeId { get; set; }
    public int ValueId { get; set; }
    public int PpeId { get; set; }

    // Navigation properties
    public Attribute Attribute { get; set; }
    public Value Value { get; set; }
    public Ppe Ppe { get; set; }
}