using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Dependent
{
    public class AddDependentWithEmployeeIdDto : AddDependentDto
    {
        [Required]
        public int EmployeeId { get; set; }
    }
}
