using Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Dependent
{
    public class UpdateDependentDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
        public Relationship Relationship { get; set; }
    }
}
