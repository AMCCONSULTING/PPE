using PPE.Data.Enums;

namespace PPE.Models;

public class Return
{
    public int Id { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public string? Document { get; set; }
    public int EmployeeId { get; set; }
    public int ArticleId { get; set; }
    public int ResponsableId { get; set; }
    public int? HseId { get; set; }
    public int MagazinierId { get; set; }
    public int Quantity { get; set; }
    public bool IsPaid { get; set; } = false;
    public RetunStatus Status { get; set; }
    
    public Hse? Hse { get; set; }
    public Employee? Employee { get; set; }
    public Responsable? Responsable { get; set; }
    public Magazinier? Magazinier { get; set; }
    public PpeAttributeCategoryAttributeValue? Article { get; set; }
}