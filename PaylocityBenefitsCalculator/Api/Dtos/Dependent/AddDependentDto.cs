﻿using Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Dependent
{
    public class AddDependentDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Relationship Relationship { get; set; }
        [Required]
        public string? DateOfBirth { get; set; }
    }
}
