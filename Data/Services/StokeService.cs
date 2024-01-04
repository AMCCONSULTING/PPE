using Microsoft.EntityFrameworkCore;
using PPE.Models;

namespace PPE.Data.Services;

public class StokeService : IStokeService
{
    
    private readonly AppDbContext _context;
    
    public StokeService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Stoke>> GetStocks()
    {
        
        IQueryable<Stoke> stokes = _context.Stokes
            .Include(s => s.Magazinier)
            .ThenInclude(e => e.Employee)
            .Include(s => s.Responsable)
            .ThenInclude(e => e.Employee)
            .Include(s => s.StokeDetails)!
            .ThenInclude(sd => sd.Article)
            .ThenInclude(a => a.Ppe)
            .Include(s => s.StokeDetails)!
            .ThenInclude(sd => sd.Article)
            .ThenInclude(a => a.AttributeValueAttributeCategory)
            .ThenInclude(avac => avac.AttributeValue)
            .ThenInclude(av => av.Value)
            .OrderByDescending(s => s.Date);
        
        /*var stokes = _context.Stokes
            .Include(s => s.Magazinier)
            .Include(s => s.Responsable)
            .OrderByDescending(s => s.Date);
        return await stokes.ToListAsync();*/
        return await stokes.ToListAsync();
    }

    public async Task<Stoke?> GetStoke(int id)
    {
        var stoke = await _context.Stokes
            .Include(s => s.Magazinier)
            .ThenInclude(s => s.Employee)
            .Include(s => s.Responsable)
            .ThenInclude(s => s.Employee)
            .Include(s => s.StokeDetails)!
            .ThenInclude(sd => sd.Article)
            .ThenInclude(a => a.Ppe)
            .Include(s => s.StokeDetails)!
            .ThenInclude(sd => sd.Article)
            .ThenInclude(a => a.AttributeValueAttributeCategory)
            .ThenInclude(avac => avac.AttributeValue)
            .ThenInclude(av => av.Value)
            .Where(s => s.Id == id)
            .OrderByDescending(s => s.Date)
            .FirstOrDefaultAsync();
        
        
        
        return stoke;
    }

    public async Task<Stoke> AddStoke(Stoke stoke)
    {
        await _context.Stokes.AddAsync(stoke);
        await _context.SaveChangesAsync();
        return stoke;
    }

    public async Task<Stoke> UpdateStoke(Stoke stock)
    {
        _context.Stokes.Update(stock);
        await _context.SaveChangesAsync();
        return stock;
    }

    public async Task<Stoke?> DeleteStoke(int id)
    {
        var stock = await _context.Stokes.FindAsync(id);
        if (stock != null)
        {
            _context.Stokes.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;
        }
        else
        {
            return null;
        }
    }

    public async Task<StokeDetail> AddStokeDetails(StokeDetail stokeDetail)
    {
        await _context.StokeDetails.AddAsync(stokeDetail);
        await _context.SaveChangesAsync();
        return stokeDetail;
    }

    public async Task<StokeDetail> UpdateStokeDetails(StokeDetail stockDetail)
    {
        _context.StokeDetails.Update(stockDetail);
        await _context.SaveChangesAsync();
        return stockDetail;
    }

    public async Task<MainStock> AddMainStock(MainStock mainStock)
    {
        await _context.MainStocks.AddAsync(mainStock);
        await _context.SaveChangesAsync();
        return mainStock;
    }

    public async Task<MainStock> UpdateMainStock(MainStock mainStock)
    {
        _context.MainStocks.Update(mainStock);
        await _context.SaveChangesAsync();
        return mainStock;
    }
}