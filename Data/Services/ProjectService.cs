using PPE.Models;

namespace PPE.Data.Services;


public class ProjectService : IProjectService
{
    
    private readonly AppDbContext _context;
    ProjectService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        return await _context.Projects.FindAsync(id);
    }

    public List<Project> GetProjectsAsync()
    {
        return _context.Projects.ToList();
    }

    public async Task<Project> AddProjectAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project> UpdateProjectAsync(Project project)
    {
        
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project> DeleteProjectAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Project> Get(int id)
    {
        throw new NotImplementedException();
    }
}