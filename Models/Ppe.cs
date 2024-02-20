namespace PPE.Models;

public class Ppe : AuditableEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    // Threshold for the stock and set default value 5
    public int Threshold { get; set; } = 5;
    //public virtual ICollection<Variant>? Variants { get; set; }
    public virtual ICollection<PpeAttributeCategoryAttributeValue>? PpeAttributeCategoryAttributeValues { get; set; }
}

public class PpeFilter
{
    public int Category { get; set; }
    public string Title { get; set; }
}

