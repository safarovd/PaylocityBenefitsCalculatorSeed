using Api.Services.BenefitsHelper;
using Api.Repositories.MockDataBase;
using Api.Dtos.Employee;
using Api.Models;

namespace Api.Services
{
    public class EmployeeService : BenefitsService, IEmployeeService
    {
        private IMockDataBase _repository;
        public EmployeeService(IMockDataBase employeeRepository) 
        { 
            // setup the database service to query for data
            _repository = employeeRepository;
        }

        public List<GetEmployeeDto> GetAllEmployees()
        {
            // create a list of GetEmployeeDtos
            var allEmployeeDtos = new List<GetEmployeeDto>();
            // mock out a database query to all employees
            var allEmployees = _repository.QueryAllEmployees();
            foreach (Employee emp in allEmployees)
            {
                // map Employee to GetEmployeeDto
                var employeeDto = Mapper.Map<GetEmployeeDto>(emp);
                allEmployeeDtos.Add(employeeDto);
            }
            return allEmployeeDtos;
        }
        // Get employee object and map it to a GetEmpoyeeDto, the return
        public GetEmployeeDto? GetEmployee(int id)
        {
            var employee = _repository.QueryEmployeeById(id);
            var getEmployeeDto = Mapper.Map<GetEmployeeDto>(employee);
            return getEmployeeDto;
        }

        public AddEmployeeDto AddEmployee(AddEmployeeDto newEmployeeDto)
        {
            var newEmployee = Mapper.Map<Employee>(newEmployeeDto);
            _repository.InsertEmployee(newEmployee);
            return newEmployeeDto;
        }


        public GetEmployeeDto UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            _repository.UpdateEmployee(id, updateEmployeeDto);
            return GetEmployee(id);
        }

        public GetEmployeeDto DeleteEmployee(int id)
        {
            var getEmployeeDto = GetEmployee(id);
            _repository.DeleteEmployee(id);
            return getEmployeeDto;
        }

        public decimal GetEmployeePaycheck(int id)
        {
            var getEmployeeDto = GetEmployee(id);
            // call the calculator and calculate Employee's paycheck based off their info
            return PaycheckCalculator.CalculatePaycheck(getEmployeeDto);
        }
    }
}
