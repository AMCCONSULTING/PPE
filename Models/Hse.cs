namespace PPE.Models;

public class Hse
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    
    public ICollection<Return>? Returns { get; set; }
}