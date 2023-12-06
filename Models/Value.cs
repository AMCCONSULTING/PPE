﻿namespace PPE.Models;

public class Value
{
    public int Id { get; set; }
    public string Text { get; set; }

    // Navigation properties
    public List<AttributeValue> AttributeValues { get; set; }
}