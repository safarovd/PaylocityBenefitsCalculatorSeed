using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using AutoMapper;

namespace Api.Services
{
    public class BenefitsService : IBenefitsService
    {
        protected IMapper Mapper;
        public BenefitsService()
        {
            // Setup the AutoMapper so that we can map our Employee and Dependent data to DTOs and vis-versa
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Employee, GetEmployeeDto>();
                cfg.CreateMap<Dependent, GetDependentDto>();
                cfg.CreateMap<AddEmployeeDto, Employee>();
                cfg.CreateMap<AddDependentDto, Dependent>();
                cfg.CreateMap<AddDependentDto, Employee>()
                .ForMember(emp => emp.Dependents, opt => opt.MapFrom(addDep => addDep));
                cfg.CreateMap<UpdateEmployeeDto, GetEmployeeDto>();
                cfg.CreateMap<UpdateEmployeeDto, Employee>();
                cfg.CreateMap<UpdateDependentDto, Dependent>();
                cfg.CreateMap<AddDependentWithEmployeeIdDto, Dependent>();
            });
            Mapper = new Mapper(config);
        }
    }
}
