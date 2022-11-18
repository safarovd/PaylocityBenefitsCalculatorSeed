using Api.Dtos.Employee;
using Api.Dtos.Dependent;
using Api.Models;

namespace Api.BenefitsServices.MockDataBaseService
{
    public interface IMockDataBaseService
    {
        public Employee QueryEmployeeById(int id);
        public List<Employee> QueryAllEmployees();
        public Dependent QueryDependentById(int id);
        public List<Dependent> QueryAllDependents();
        public void InsertEmployee(Employee employee);
        public void UpdateEmployee(int id, UpdateEmployeeDto updatedEmployee);
        public void DeleteEmployee(int id);
        public void InsertDependent(Dependent dependent);
        public void UpdateDependent(int id, UpdateDependentDto updatedDependent);
        public void DeleteDependent(int id);
    }
}
