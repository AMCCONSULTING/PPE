namespace PPE.Models;

public class Coordinateur
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; } = null!;
}