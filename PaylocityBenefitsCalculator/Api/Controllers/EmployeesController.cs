using Api.Services;
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
            try
            {
                var employeeDto = _employeeService.GetEmployee(id);
                return Ok(employeeDto);
            } catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Get all employees")]
        [HttpGet("")]
        public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
        {
            try
            {
                var allEmployeesDto = _employeeService.GetAllEmployees();
                return Ok(allEmployeesDto);
            } catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [SwaggerOperation(Summary = "Get Employee Paycheck")]
        [HttpGet("{id}/Paycheck")]
        public async Task<ActionResult<decimal>> GetEmployeePaycheck(int id)
        {
            try
            {
                decimal paycheck = _employeeService.GetEmployeePaycheck(id);
                return Ok(paycheck);
            } catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [SwaggerOperation(Summary = "Add employee")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<List<AddEmployeeDto>>>> AddEmployee(AddEmployeeDto newEmployee)
        {
            try
            {
                var addedEmployee = _employeeService.AddEmployee(newEmployee);
                return Ok(addedEmployee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [SwaggerOperation(Summary = "Update employee")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> UpdateEmployee(int id, UpdateEmployeeDto update)
        {
            try
            {
                var updatedEmployee = _employeeService.UpdateEmployee(id, update);
                return Ok(updatedEmployee);
            } catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }

        [SwaggerOperation(Summary = "Delete employee")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> DeleteEmployee(int id)
        {
            try
            {
                var removedEmployee = _employeeService.DeleteEmployee(id);
                return Ok(removedEmployee);
            } catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
