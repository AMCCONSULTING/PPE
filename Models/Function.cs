﻿using System.ComponentModel.DataAnnotations;

namespace PPE.Models;

public class Function
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = null!;
    public int EmployeeCount => Employees?.Count ?? 0;
    public string? Description { get; set; }
    public ICollection<Employee>? Employees { get; set; }
    
}