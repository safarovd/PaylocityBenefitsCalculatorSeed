using Api.BenefitsServices;
using Api.Dtos.Employee;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        protected IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [SwaggerOperation(Summary = "Get employee by id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
        {
            var employeeDto = _employeeService.GetEmployee(id);
            return Ok(employeeDto);
        }

        [SwaggerOperation(Summary = "Get all employees")]
        [HttpGet("")]
        public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
        {
            var allEmployeesDto = _employeeService.GetAllEmployees();
            return Ok(allEmployeesDto);
        }

        [SwaggerOperation(Summary = "Get Employee Monthly Paycheck")]
        [HttpGet("{id}/Paycheck")]
        public async Task<ActionResult<decimal>> GetEmployeePaycheck(int id)
        {
            decimal paycheck = _employeeService.GetEmployeePaycheck(id);
            return Ok(paycheck);
        }

        [SwaggerOperation(Summary = "Add employee")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<List<AddEmployeeDto>>>> AddEmployee(AddEmployeeDto newEmployee)
        {
            var addedEmployee = _employeeService.AddEmployee(newEmployee);
            return Ok(addedEmployee);
        }

        [SwaggerOperation(Summary = "Update employee")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> UpdateEmployee(int id, UpdateEmployeeDto update)
        {
            var updatedEmployee = _employeeService.UpdateEmployee(id, update);
            return Ok(updatedEmployee);
        }

        [SwaggerOperation(Summary = "Delete employee")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> DeleteEmployee(int id)
        {
            var removedEmployee = _employeeService.DeleteEmployee(id);
            return Ok(removedEmployee);
        }
    }
}
