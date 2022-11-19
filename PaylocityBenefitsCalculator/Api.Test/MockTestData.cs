using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Models;
using System.Collections.Generic;

namespace Api.Test
{
    internal class MockTestData
    {
        // Employee Dummy Data
        public static AddEmployeeDto addEmployeeDto = new AddEmployeeDto()
            {
                FirstName = "Daniel",
                LastName = "Standard",
                DateOfBirth = "2000-5-31",
                Dependents = listAddDependentsDto,
                Salary = 150000,
            };

        public static GetEmployeeDto getEmployeeDto = new GetEmployeeDto()
        {
            FirstName = "Daniel",
            LastName = "Standard",
            DateOfBirth = "2000-5-31",
            Dependents = (ICollection<GetDependentDto>)listAddDependentsDto,
            Salary = 150000,
            Id = 0
        };

        public static UpdateEmployeeDto updateEmployeeDto = new UpdateEmployeeDto()
        {
            FirstName = "Daniel",
            LastName = "Updated",
            Salary = 200000,
        };

        // Dependent Dummy Data
        public static AddDependentDto addDependentDto = new AddDependentDto()
        {
            FirstName = "Child",
            LastName = "Standard",
            DateOfBirth = "2020-05-1",
            Relationship = Relationship.Child
        };

        public static AddDependentWithEmployeeIdDto addDependentWithEmployeeIdDto = new AddDependentWithEmployeeIdDto()
        {
            EmployeeId = 0,
        };

        public static List<AddDependentWithEmployeeIdDto> listAddDependentWithEmployeeIdDto = new List<AddDependentWithEmployeeIdDto>()
        {
            addDependentWithEmployeeIdDto
        };

        public static List<AddDependentDto> listAddDependentsDto = new List<AddDependentDto>()
        {
            addDependentDto
        };

        public static GetDependentDto getDependentDto = new GetDependentDto()
        {
            Id = 0,
            FirstName = "Child",
            LastName = "Standard",
            DateOfBirth = "2020-05-1",
            Relationship = Relationship.Child
        };

        public static List<GetDependentDto> listGetDependentDtos = new List<GetDependentDto>()
        {
            getDependentDto
        };

        public static UpdateDependentDto updateDependentDto = new UpdateDependentDto()
        {
            FirstName = "Child",
            LastName = "Updated",
            DateOfBirth = "2020-05-1",
            Relationship = Relationship.Child
        };
    }
}
