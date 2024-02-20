using PPE.Data.Enums;

namespace PPE.Models;

public class Stock : AuditableEntity
{
    public int Id { get; set; }
    //public int Quantity { get; set; }
    public DateTime Date { get; set; }
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
    public int StockIn { get; set; }
    public int StockOut { get; set; }
    public int PpeId { get; set; }
    public Ppe? Ppe { get; set; }
    public StockNature StockNature { get; set; } 
    public StockType StockType { get; set; }
    public int CurrentStock => StockIn - StockOut;
    public ICollection<StockDetail>? StockDetails { get; set; }
    
    
}