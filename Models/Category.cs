using System.ComponentModel;

namespace PPE.Models;

public class Category
{
    public int Id { get; set; }
    public string Title { get; set; }
    [DisplayName ("PPE Count")]
    public int PpeCount => Ppes?.Count ?? 0;
    public string? Description { get; set; }
    public virtual ICollection<Ppe>? Ppes { get; set; }
    /*
    public virtual ICollection<Variant>? Variants { get; set; }
    */
    [DisplayName("Attribute Categories")]
    public virtual ICollection<AttributeCategory>? AttributeCategories { get; set; }    
}