﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PPE.Data.Enums;

namespace PPE.Models;

public class Employee
{
    [Key]
    public int Id { get; set; }
    [Required]
    [DisplayName("First name")]
    public string FirstName { get; set; } 
    [Required]
    [DisplayName("Last name")]
    public string LastName { get; set; }
    [DisplayName("Full name")]
    public string FullName => $"{FirstName} {LastName}";
    [Required]
    public string NNI { get; set; }
    [Required]
    public string Phone { get; set; }
    public string? Tel { get; set; }
    [Required]
    public Gender Gender { get; set; }
    
    // Relationship with Project
    [Required] [DisplayName("Project")]
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    
    // Relationship with Function
    [Required] [DisplayName("Function")]
    public int FunctionId { get; set; }
    public Function? Function { get; set; } = null!;
    
}