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
    public virtual ICollection<ProjectStock>? ProjectStocks { get; set; }
    public virtual ICollection<MouvementDetail>? MouvementDetails { get; set; }
    public virtual ICollection<DotationDetail>? DotationDetails { get; set; }
    public virtual ICollection<StockEmployee>? StockEmployees { get; set; }
    public virtual ICollection<PayableStock>? PayableStocks { get; set; }
    
    
}