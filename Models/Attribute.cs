namespace PPE.Models;

public class Attribute
{
    public int Id { get; set; }
    public string Name { get; set; }

    // Navigation properties
    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public List<AttributeValue> AttributeValues { get; set; }
}