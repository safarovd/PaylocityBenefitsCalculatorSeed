using Api.BenefitsServices.MockDataBaseService;
using Api.Dtos.Employee;
using Api.Models;
using System.Globalization;

namespace Api.BenefitsServices
{
    public class EmployeeService : BenefitsService, IEmployeeService
    {
        private IMockDataBaseService _databaseService;
        public EmployeeService(IMockDataBaseService databaseService) 
        { 
            _databaseService = databaseService;
        }

        public List<GetEmployeeDto> GetAllEmployees()
        {
            // create a list of GetEmployeeDtos
            var allEmployeeDtos = new List<GetEmployeeDto>();
            // mock out a database query to all employees
            var allEmployees = _databaseService.QueryAllEmployees();
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
            var employee = _databaseService.QueryEmployeeById(id);
            var getEmployeeDto = Mapper.Map<GetEmployeeDto>(employee);
            return getEmployeeDto;
        }

        public AddEmployeeDto AddEmployee(AddEmployeeDto newEmployeeDto)
        {
            var newEmployee = Mapper.Map<Employee>(newEmployeeDto);
            _databaseService.InsertEmployee(newEmployee);
            return newEmployeeDto;
        }


        public GetEmployeeDto UpdateEmployee(int id, UpdateEmployeeDto updateEmployeeDto)
        {
            _databaseService.UpdateEmployee(id, updateEmployeeDto);
            return GetEmployee(id);
        }

        public GetEmployeeDto DeleteEmployee(int id)
        {
            var getEmployeeDto = GetEmployee(id);
            _databaseService.DeleteEmployee(id);
            return getEmployeeDto;
        }

        public decimal GetEmployeePaycheck(int id)
        {
            var getEmployeeDto = GetEmployee(id);
            return CalculatePaycheck(getEmployeeDto);

        }

        private decimal CalculatePaycheck(GetEmployeeDto getEmployeeDto)
        {
            decimal salary = getEmployeeDto.Salary;
            int age = GetEmployeeAge(getEmployeeDto.DateOfBirth);
            // If salary is over 80k, incure 2% fee
            salary = SalaryCapFee(salary);
            var monthlySalary = ConvertSalaryToMonthly(salary);
            // if employye is 50 years or older, deduct $200 per month
            monthlySalary = OldAgeFee(age, monthlySalary);
            // every employee pays a base fee of $1000
            monthlySalary = BaseFee(monthlySalary);
            // every dependent costs $600 per month for benefits
            monthlySalary = DependentFee(monthlySalary, getEmployeeDto);
            // convert monthly to actual paycheck
            var paycheck = ConvertToPaycheck(monthlySalary);
            if (paycheck < 0) throw new InvalidOperationException($"Employee {getEmployeeDto.Id} earns peanuts, pay more.");
            return decimal.Round(paycheck, 2, MidpointRounding.AwayFromZero); ;
        }
        private decimal ConvertSalaryToMonthly(decimal salary)
        {
            return salary / 12;
        }

        private decimal ConvertToPaycheck(decimal monthlySalary)
        {
            return (monthlySalary * 12) / 26;
        }

        private int GetEmployeeAge(string date)
        {
            var cultureInfo = new CultureInfo("de-DE");
            var birthday = DateTime.Parse(date, cultureInfo,
                                            DateTimeStyles.NoCurrentDateDefault);
            int age = (DateTime.Now - birthday).Days / 365;
            return age;
        }

        private decimal SalaryCapFee(decimal salary)
        {
            if (salary >= 80000)
            {
                return salary * (decimal)0.98;
            }
            return salary;
        }

        private decimal OldAgeFee(int age, decimal monthlySalary)
        {
            if (age >= 50)
            {
                return monthlySalary - 200;
            }
            return monthlySalary;
        }

        private decimal BaseFee(decimal monthlySalary)
        {
            return monthlySalary - 1000;
        }

        private decimal DependentFee(decimal monthlySalary, GetEmployeeDto getEmployeeDto)
        {
            return monthlySalary - (getEmployeeDto.Dependents.Count * 600);
        }
    }
}
