namespace PPE.Models;

public class Stock
{
    public int Id { get; set; }
    //public int Quantity { get; set; }
    public DateTime Date { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    public int StockIn { get; set; }
    public int StockOut { get; set; }
    public int CurrentStock => StockIn - StockOut;
    public int VariantValueId { get; set; }
    public VariantValue? VariantValue { get; set; }
    
}