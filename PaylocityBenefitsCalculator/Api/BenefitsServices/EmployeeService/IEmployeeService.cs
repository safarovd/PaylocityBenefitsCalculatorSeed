using Api.BenefitsServices;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.BenefitsServices
{
    public interface IEmployeeService : IBenefitsService
    {
        List<GetEmployeeDto> GetAllEmployees();
        GetEmployeeDto? GetEmployee(int id);
        AddEmployeeDto AddEmployee(AddEmployeeDto employee);
        GetEmployeeDto UpdateEmployee(int id, UpdateEmployeeDto update);
        GetEmployeeDto DeleteEmployee(int id);
        decimal GetEmployeeMonthlyPaycheck(int id);
    }
}
