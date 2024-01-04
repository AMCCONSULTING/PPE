using System.ComponentModel;

namespace PPE.Models;

public class Stoke
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Reference { get; set; }
    public string Document { get; set; }
    [DisplayName ("Magazinier")]
    public int? MagazinierId { get; set; }
    public Magazinier? Magazinier { get; set; }
    [DisplayName ("Responsable")]
    public int ResponsableId { get; set; }
    public Responsable? Responsable { get; set; } = null!;
    
    // navigation properties
    public ICollection<StokeDetail>? StokeDetails { get; set; } = null!;
    
}