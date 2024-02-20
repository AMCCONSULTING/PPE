namespace PPE.Data.Services;

public class UserResolverService
{
    private readonly IHttpContextAccessor _context;
    public UserResolverService(IHttpContextAccessor context)
    {
        _context = context;
    }

    public string? GetUser()
    {
        return _context.HttpContext?.User?.Identity?.Name;
    }
    
    public async Task<string?> GetUserAsync()
    {
        return await Task.FromResult(_context.HttpContext?.User?.Identity?.Name);
    }
    
}