using System.ComponentModel;

namespace PPE.Models;

public class Mouvement : AuditableEntity
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string? Reference { get; set; }
    public Project? Project { get; set; } = null!;
    public string Document { get; set; }
    [DisplayName("Created at")]
    public DateTime? CreatedAt { get; set; }
    [DisplayName("Updated at")]
    public DateTime? UpdatedAt { get; set; }
    [DisplayName("Updated by")]
    public string? UpdatedBy { get; set; }
    [DisplayName("Created by")]
    public string? CreatedBy { get; set; }
    public int ProjectId { get; set; }
    [DisplayName ("Responsable")]
    public int ResponsableId { get; set; }
    public Responsable? Responsable { get; set; } = null!;
    [DisplayName ("Magazinier")]
    public int MagazinierId { get; set; }
    public Magazinier? Magazinier { get; set; } = null!;
    [DisplayName ("Transporteur")]
    public int TransporteurId { get; set; }
    public Transporteur? Transporteur { get; set; } = null!;
    [DisplayName ("Coordinateur")]
    public int CoordinateurId { get; set; }
    public Coordinateur? Coordinateur { get; set; } = null!;
    
    // navigation properties
    public ICollection<MouvementDetail>? MouvementDetails { get; set; } = null!;
    
}