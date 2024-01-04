using PPE.Models;

namespace PPE.Data.Services;

public interface IStokeService
{
    Task<List<Stoke>> GetStocks();
    Task<Stoke?> GetStoke(int id);
    Task<Stoke> AddStoke(Stoke stock);
    Task<Stoke> UpdateStoke(Stoke stock);
    Task<Stoke?> DeleteStoke(int id);
    Task<StokeDetail> AddStokeDetails(StokeDetail stockDetail);
    Task<StokeDetail> UpdateStokeDetails(StokeDetail stockDetail);
    Task<MainStock> AddMainStock(MainStock mainStock);
    Task<MainStock> UpdateMainStock(MainStock mainStock);
    
}