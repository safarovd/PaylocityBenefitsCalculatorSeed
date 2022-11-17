using Api.Dtos.Employee;
using Api.Models;

namespace Api.BenefitsServices
{
    public interface IEmployeeService
    {
        List<GetEmployeeDto> GetAllEmployees();
        GetEmployeeDto GetEmployee(int id);
        
    }
}
