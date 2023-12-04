namespace PPE.Models;

public class StockDetail
{
    public int Id { get; set; }
    public int StockId { get; set; }
    public virtual Stock Stock { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
    public int Total { get; set; }
    public DateTime Date { get; set; }
    
    // stock detail relationship
    
    
}