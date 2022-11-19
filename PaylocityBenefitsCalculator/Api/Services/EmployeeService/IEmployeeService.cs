using Api.Dtos.Employee;

namespace Api.Services
{
    public interface IEmployeeService : IBenefitsService
    {
        List<GetEmployeeDto> GetAllEmployees();
        GetEmployeeDto? GetEmployee(int id);
        AddEmployeeDto AddEmployee(AddEmployeeDto employee);
        GetEmployeeDto UpdateEmployee(int id, UpdateEmployeeDto update);
        GetEmployeeDto DeleteEmployee(int id);
        decimal GetEmployeePaycheck(int id);
    }
}
