using Api.BenefitsServices.DependentService;
using Api.Dtos.Dependent;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DependentsController : ControllerBase
    {
        private IDependentService _dependentService;

        public DependentsController(IDependentService dependentService)
        {
            // Dependency Injection
            _dependentService = dependentService;
        }

        [SwaggerOperation(Summary = "Get dependent by id")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
        {
            var dependent = _dependentService.GetDependent(id);
            return Ok(dependent);
        }

        [SwaggerOperation(Summary = "Get all dependents")]
        [HttpGet("")]
        public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
        {
            var allDependents = _dependentService.GetAllDependents();
            return Ok(allDependents);
        }

        [SwaggerOperation(Summary = "Add dependent")]
        [HttpPost]
        public async Task<ActionResult<ApiResponse<List<AddDependentWithEmployeeIdDto>>>> AddDependent(AddDependentWithEmployeeIdDto newDependent)
        {
            var dependentsAdded = new List<AddDependentWithEmployeeIdDto>();
            var dependent = _dependentService.AddDependent(newDependent);
            dependentsAdded.Add(dependent);
            return Ok(dependentsAdded);
        }

        [SwaggerOperation(Summary = "Update dependent")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<GetDependentDto>>> UpdateDependent(int id, UpdateDependentDto updatedDependent)
        {
            _dependentService.UpdateDependent(id, updatedDependent);
            var dependent = _dependentService.GetDependent(id);
            return Ok(dependent);
        }

        [SwaggerOperation(Summary = "Delete dependent")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> DeleteDependent(int id)
        {
            var removedDependent = _dependentService.DeleteDependent(id);
            return Ok(removedDependent);
        }
    }
}
