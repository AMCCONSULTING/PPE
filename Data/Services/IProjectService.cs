using Microsoft.Build.Evaluation;
using PPE.Models;
using Project = PPE.Models.Project;

namespace PPE.Data.Services;

public interface IProjectService
{
    Task<Project?> GetProjectByIdAsync(int id);
    List<Project> GetProjectsAsync();
    Task<Project> AddProjectAsync(Project project);
    Task<Project> UpdateProjectAsync(Project project);
    Task<Project> DeleteProjectAsync(int id);
    Task<Project> Get(int id);
    
}