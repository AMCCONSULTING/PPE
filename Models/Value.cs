namespace PPE.Models;

public class Value
{
    public int Id { get; set; }
    public int VariantId { get; set; }
    public Variant Variant { get; set; }
    public string Val { get; set; }
    public virtual ICollection<VariantValue> VariantValues { get; set; }
}