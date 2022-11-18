﻿using Api.Dtos.Dependent;

namespace Api.BenefitsServices.DependentService
{
    public interface IDependentService
    {
        List<GetDependentDto> GetAllDependents();
        GetDependentDto? GetDependent(int id);
        AddDependentWithEmployeeIdDto AddDependent(AddDependentWithEmployeeIdDto Dependent);
        GetDependentDto UpdateDependent(int id, UpdateDependentDto update);
        GetDependentDto DeleteDependent(int id);
    }
}
