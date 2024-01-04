using System.ComponentModel.DataAnnotations;

namespace PPE.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        [Required] public string Prefix { get; set; } = null!;
        public string? Description { get; set; }
        
        // list of employees
        public ICollection<Employee>? Employees { get; set; }
        // number of employees
        public int? EmployeeCount => Employees?.Count ?? 0;
        
        
        // list of stocks
        public ICollection<Stock>? Stocks { get; set; }
        public ICollection<ProjectStock> ProjectStocks { get; set; }
        
        
    }
}
