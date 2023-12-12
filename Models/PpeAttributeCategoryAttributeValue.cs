namespace PPE.Models;

public class PpeAttributeCategoryAttributeValue
{
    public int Id { get; set; }
    public int PpeId { get; set; }
    public Ppe Ppe { get; set; }
    public int AttributeValueAttributeCategoryId { get; set; }
    public AttributeValueAttributeCategory AttributeValueAttributeCategory { get; set; }
    public virtual ICollection<StockDetail>? StockDetails { get; set; }
    public virtual ICollection<EmployeeStock>? EmployeeStocks { get; set; }
}