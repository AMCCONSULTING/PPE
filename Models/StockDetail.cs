using System.ComponentModel.DataAnnotations.Schema;

namespace PPE.Models;

public class StockDetail
{
    public int Id { get; set; }
   // public DateTime Date { get; set; }
    public int StockId { get; set; }
    public Stock? Stock { get; set; }
    public int PpeAttributeCategoryAttributeValueId { get; set; }
    [ForeignKey("PpeAttributeCategoryAttributeValueId")]
    public PpeAttributeCategoryAttributeValue? PpeAttributeCategoryAttributeValue { get; set; }
    public int StockIn { get; set; }
    public int StockOut { get; set; }
    
}