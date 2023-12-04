namespace PPE.Models;

public class Variant
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int PpeId { get; set; }
    public virtual Ppe Ppe { get; set; }
    public virtual ICollection<VariantValue> VariantValues { get; set; }   
    //public virtual ICollection<Stock> Stocks { get; set; }
}