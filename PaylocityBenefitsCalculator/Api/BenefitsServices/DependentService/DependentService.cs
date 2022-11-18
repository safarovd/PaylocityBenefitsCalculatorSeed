using Api.BenefitsServices.MockDataBaseService;
using Api.Dtos.Dependent;
using Api.Models;

namespace Api.BenefitsServices.DependentService
{
    public class DependentService : BenefitsService, IDependentService
    {
        IMockDataBaseService _databaseService;
        public DependentService(IMockDataBaseService databaseService)
        {
            _databaseService = databaseService;

        }
        public List<GetDependentDto> GetAllDependents()
        {
            // create a list of GetDependentDtos
            var dependentsDtos = new List<GetDependentDto>();
            var dependents = _databaseService.QueryAllDependents();
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
            var dependent = _databaseService.QueryDependentById(id);
            var getdependentDto = Mapper.Map<GetDependentDto>(dependent);
            return getdependentDto;
        }

        public GetDependentDto UpdateDependent(int id, UpdateDependentDto update)
        {
            // get the updated Dto and return
            var getDependentDto = GetDependent(id);
            _databaseService.UpdateDependent(id, update);
            return getDependentDto;
        }
        public AddDependentWithEmployeeIdDto AddDependent(AddDependentWithEmployeeIdDto newDependent)
        {
            var dependent = Mapper.Map<Dependent>(newDependent);
            _databaseService.InsertDependent(dependent);
            return newDependent;
        }

        public GetDependentDto DeleteDependent(int id)
        {
            var dependentDto = GetDependent(id);
            _databaseService.DeleteDependent(id);
            return dependentDto;
        }
    }
}
