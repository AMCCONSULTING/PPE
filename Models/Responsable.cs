﻿namespace PPE.Models;

public class Responsable
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}