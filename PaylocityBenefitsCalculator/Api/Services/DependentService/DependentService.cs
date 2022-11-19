using Api.Repositories.MockDataBase;
using Api.Dtos.Dependent;
using Api.Models;

namespace Api.Services.DependentService
{
    public class DependentService : BenefitsService, IDependentService
    {
        IMockDataBase _repository;
        public DependentService(IMockDataBase dependentRepository)
        {
            // hook up our Database mock service
            // Reason: we do this so that if we ever decide to hook up a real database, not 
            // much refactoring will be needed.
            _repository = dependentRepository;

        }
        public List<GetDependentDto> GetAllDependents()
        {
            // create a list of GetDependentDtos
            var dependentsDtos = new List<GetDependentDto>();
            var dependents = _repository.QueryAllDependents();
            foreach (Dependent dep in dependents)
            {
                // map Dependent to GetDependentDto
                var dependentDto = Mapper.Map<GetDependentDto>(dep);
                dependentsDtos.Add(dependentDto);
            }
            return dependentsDtos;
        }

        public GetDependentDto GetDependent(int id)
        {
            var dependent = _repository.QueryDependentById(id);
            var getdependentDto = Mapper.Map<GetDependentDto>(dependent);
            return getdependentDto;
        }

        public GetDependentDto UpdateDependent(int id, UpdateDependentDto update)
        {
            // get the updated Dto and return
            var getDependentDto = GetDependent(id);
            _repository.UpdateDependent(id, update);
            return getDependentDto;
        }
        public AddDependentWithEmployeeIdDto AddDependent(AddDependentWithEmployeeIdDto newDependent)
        {
            var dependent = Mapper.Map<Dependent>(newDependent);
            _repository.InsertDependent(dependent);
            return newDependent;
        }

        public GetDependentDto DeleteDependent(int id)
        {
            var dependentDto = GetDependent(id);
            _repository.DeleteDependent(id);
            return dependentDto;
        }
    }
}
