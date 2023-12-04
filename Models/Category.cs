namespace PPE.Models;

public class Category
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<Ppe> Ppes { get; set; }
    
}