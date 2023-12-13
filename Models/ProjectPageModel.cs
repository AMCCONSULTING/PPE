namespace PPE.Models;

public class ProjectPageModel
{
    public Project Project { get; set; }
    public FormModel FormModel { get; set; }
    
}

public class FormModel
{
    public int PpeId { get; set; }
    public int ProjectId { get; set; }
}