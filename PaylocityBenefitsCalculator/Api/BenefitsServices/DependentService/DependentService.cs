using Api.Dtos.Dependent;
using Api.Models;

namespace Api.BenefitsServices.DependentService
{
    public class DependentService : BenefitsService, IDependentService
    {
        public AddDependentWithEmployeeIdDto AddDependent(AddDependentWithEmployeeIdDto newDependent)
        {
            var newDep = Mapper.Map<Dependent>(newDependent);
            var employee = Data.Employees.Where(emp => emp.Id == newDependent.EmployeeId).FirstOrDefault();
            // make sure the new Dependent does not have more than one partner
            if (!CanAddDependent(employee))
            {
                throw new InvalidOperationException("Dependent cannot have two household parters of type Spouse or DomesticPartner.");
            }
            newDep.Id = (Data.Dependents.Count > 0 ? Data.Dependents.Last().Id + 1 : 0);
            Data.Dependents.Add(newDep);
            JsonLoader.WriteJson(Data, MockEntitiesPath);
            return newDependent;
        }

        public GetDependentDto DeleteDependent(int id)
        {
            var removedDependent = GetDependent(id);
            var employee = Data.Employees.Where(emp => emp.Id == id).FirstOrDefault();
            foreach (Dependent dep in employee.Dependents.ToList())
            {
                if (dep.Id == id)
                {
                    employee.Dependents.Remove(dep);
                    break;
                }
            }

            foreach (Dependent dep in Data.Dependents.ToList())
            {
                if (dep.Id == id)
                {
                    Data.Dependents.Remove(dep);
                    break;
                }
            }

            // write our updated data to our mock storage
            JsonLoader.WriteJson(Data, MockEntitiesPath);
            return removedDependent;
        }

        public List<GetDependentDto> GetAllDependents()
        {
            // create a list of GetDependentDtos
            var allDependents = new List<GetDependentDto>();
            foreach (Dependent emp in Data.Dependents)
            {
                // map Dependent to GetDependentDto
                var dependentDto = Mapper.Map<GetDependentDto>(emp);
                allDependents.Add(dependentDto);
            }
            return allDependents;
        }

        public GetDependentDto GetDependent(int id)
        {
            var dependent = Data.Dependents.Where(dep => dep.Id == id).FirstOrDefault();
            if (dependent == null)
            {
                throw new InvalidOperationException($"Dependent with ID: {id}, does not exist, please query a new Id.");
            }
            var getdependentDto = Mapper.Map<GetDependentDto>(dependent);
            return getdependentDto;
        }

        public GetDependentDto UpdateDependent(int id, UpdateDependentDto update)
        {
            // access the targeted employee object
            var dependent = Data.Dependents.Where(dep => dep.Id == id).FirstOrDefault();
            
            // update the values
            dependent.FirstName = update.FirstName;
            dependent.LastName = update.LastName;
            dependent.DateOfBirth = update.DateOfBirth;
            dependent.Relationship = update.Relationship;
            SyncEmployeeDependents(id, update);
            // write our updated data to our mock storage
            JsonLoader.WriteJson(Data, MockEntitiesPath);
            // get the updated Dto and return
            var getDependentDto = GetDependent(id);
            return getDependentDto;
        }

        public void SyncEmployeeDependents(int dependentId, UpdateDependentDto update)
        {
            var employeeId = Data.Dependents.FirstOrDefault(dep => dep.Id == dependentId).EmployeeId;
            var employee = Data.Employees.Where(emp => emp.Id == employeeId).FirstOrDefault();
            foreach (Dependent dep in employee.Dependents)
            {
                if (dep.Id == dependentId)
                {
                    dep.FirstName = update.FirstName;
                    dep.LastName = update.LastName;
                    dep.DateOfBirth = update.DateOfBirth;
                    dep.Relationship = update.Relationship;
                    break;
                }
            }
        }
    }
}
