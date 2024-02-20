using System.ComponentModel;

namespace PPE.Models;

public class Attribute : AuditableEntity
{
    public int Id { get; set; }
    public string Title { get; set; }

    [DisplayName("Attribute Values")]
    public List<AttributeValue>? AttributeValues { get; set; }
}