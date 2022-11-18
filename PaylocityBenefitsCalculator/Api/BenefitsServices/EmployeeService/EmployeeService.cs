using Api.Dtos.Employee;
using Api.Models;


namespace Api.BenefitsServices
{
    public class EmployeeService : BenefitsService, IEmployeeService
    {
        public EmployeeService() { }

        public List<GetEmployeeDto> GetAllEmployees()
        {
            // create a list of GetEmployeeDtos
            var allEmployees = new List<GetEmployeeDto>();
            foreach (Employee emp in Data.Employees)
            {
                // map Employee to GetEmployeeDto
                var employeeDto = Mapper.Map<GetEmployeeDto>(emp);
                allEmployees.Add(employeeDto);
            }
            return allEmployees;
        }
        // Get employee object and map it to a GetEmpoyeeDto, the return
        public GetEmployeeDto? GetEmployee(int id)
        {
            var employee = Data.Employees.Where(emp => emp.Id == id).FirstOrDefault();
            if (employee == null)
            {
                return new GetEmployeeDto();
            }
            var getEmployeeDto = Mapper.Map<GetEmployeeDto>(employee);
            return getEmployeeDto;
        }
        // reverse map the Dto to the Employee entity, build the dependents and update data, then return the dto
        public AddEmployeeDto AddEmployee(AddEmployeeDto newEmployeeDto)
        {

            var newEmp = Mapper.Map<Employee>(newEmployeeDto);
            // make sure the new Employee does not have more than one partner
            if (!CanAddDependent(newEmp))
            {
                throw new InvalidOperationException("Employee cannot have two household parters of type Spouse or DomesticPartner.");
            }
            // assign the correct ID's to each new added entity
            newEmp.Dependents = ConfigureDependentIds(newEmp.Dependents,
                                                      (Data.Dependents.Count > 0 ? Data.Dependents.Last().Id : -1));
            newEmp.Id = (Data.Employees.Count > 0 ? Data.Employees.Last().Id + 1 : 0);
            Data.Employees.Add(newEmp);
            JsonLoader.WriteJson(Data, MockEntitiesPath);
            return newEmployeeDto;
        }

        private ICollection<Dependent> ConfigureDependentIds(ICollection<Dependent> dependents, int latestId)
        {
            foreach (Dependent dependent in dependents)
            {
                dependent.Id = ++latestId;
            }
            return dependents;
        }

        public GetEmployeeDto UpdateEmployee(int id, UpdateEmployeeDto updatedEmployee)
        {
            // access the targeted employee object
            var employee = Data.Employees.Where(emp => emp.Id == id).FirstOrDefault();
            // update the values
            employee.FirstName = updatedEmployee.FirstName; 
            employee.LastName = updatedEmployee.LastName;
            employee.Salary = updatedEmployee.Salary;
            // write our updated data to our mock storage
            JsonLoader.WriteJson(Data, MockEntitiesPath);
            // get the updated Dto and return
            var getEmployeeDto = GetEmployee(id);
            return getEmployeeDto;
        }

        public GetEmployeeDto DeleteEmployee(int id)
        {
            var removedEmployee = GetEmployee(id);
            
            foreach (Employee emp in Data.Employees.ToList())
            {
                if (emp.Id == id)
                {
                    CleanUpEmployeeDependents(emp);
                    Data.Employees.Remove(emp);
                }
            }
            // write our updated data to our mock storage
            JsonLoader.WriteJson(Data, MockEntitiesPath);
            return removedEmployee;
        }

        public void CleanUpEmployeeDependents(Employee employee)
        {
            if (employee.Dependents.Count > 0)
            {
                foreach(Dependent dep in Data.Dependents.ToList())
                {
                    if (employee.Dependents.Contains(dep))
                    {
                        Data.Dependents.Remove(dep);
                    }
                }
            }
        }
    }
}
