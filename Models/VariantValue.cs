namespace PPE.Models;

public class VariantValue
{
    public int Id { get; set; }
    public int VariantId { get; set; }
    public Variant Variant { get; set; }
    public string Value { get; set; }
    public virtual ICollection<Stock> Stocks { get; set; }
}