using Api.BenefitsServices.BenefitsHelper;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using AutoMapper;
using System.Text.Json;

namespace Api.BenefitsServices
{
    public class EmployeeService : IEmployeeService
    {
        public IMapper _mapper;
        private CompanyEmployees _allEmployees;
        public EmployeeService()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Employee, GetEmployeeDto>();
                cfg.CreateMap<Dependent, GetDependentDto>();
             });
            _mapper = new Mapper(config);

            JsonLoader jsonLoader = new JsonLoader();
            _allEmployees = jsonLoader.LoadJson<CompanyEmployees>("MockData\\MockEmployees\\MockEmployees.json");
        }

        public List<GetEmployeeDto> GetAllEmployees()
        {
            var allEmployees = new List<GetEmployeeDto>();
            foreach (Employee emp in _allEmployees.Employees)
            {
                var employeeDto = _mapper.Map<GetEmployeeDto>(emp);
                allEmployees.Add(employeeDto);
            }
            return allEmployees;
        }

        public GetEmployeeDto GetEmployee(int id)
        {
            throw new NotImplementedException();
        }
    }
}
