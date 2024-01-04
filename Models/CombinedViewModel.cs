namespace PPE.Models;

public class CombinedViewModel
{
    public IEnumerable<PPE.Models.StokeDetail> StokeDetails { get; set; }
    public IEnumerable<PPE.Models.MouvementDetail> MouvementDetails { get; set; }
}