using Api.Dtos.Dependent;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Employee
{
    public class AddEmployeeDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public decimal Salary { get; set; }
        public string? DateOfBirth { get; set; }
        public ICollection<AddDependentDto>? Dependents { get; set; }
    }
}
