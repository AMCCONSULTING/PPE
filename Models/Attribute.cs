using System.ComponentModel;

namespace PPE.Models;

public class Attribute
{
    public int Id { get; set; }
    public string Title { get; set; }

    // Navigation properties
    /*public int CategoryId { get; set; }
    public Category? Category { get; set; }*/

    [DisplayName("Attribute Values")]
    public List<AttributeValue>? AttributeValues { get; set; }
}