using Employee.Services;
using Microsoft.AspNetCore.Mvc;
using Employee.Models;
using Microsoft.EntityFrameworkCore;


namespace Employee.Controllers
{
  

    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;
        public EmployeesController(IEmployeeService service) => _service = service;

        [HttpGet]
        public async Task<ActionResult<List<Employeee>>> GetAll(CancellationToken ct)
        {
            try { return Ok(await _service.GetAllAsync(ct)); }
            catch (OperationCanceledException) { return StatusCode(499, "Request cancelled"); }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employeee>> Get(int id, CancellationToken ct)
        {
            try
            {
                var emp = await _service.GetByIdAsync(id, ct);
                return emp is null ? NotFound() : Ok(emp);
            }
            catch (OperationCanceledException) { return StatusCode(499, "Request cancelled"); }
        }

        [HttpPost]
        public async Task<ActionResult<Employeee>> Create(Employeee employee, CancellationToken ct)
        {
            try
            {
                var created = await _service.CreateAsync(employee, ct);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (OperationCanceledException) { return StatusCode(499, "Request cancelled"); }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Employeee employee, CancellationToken ct)
        {
            try { return await _service.UpdateAsync(id, employee, ct) ? NoContent() : NotFound(); }
            catch (OperationCanceledException) { return StatusCode(499, "Request cancelled"); }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            try { return await _service.DeleteAsync(id, ct) ? NoContent() : NotFound(); }
            catch (OperationCanceledException) { return StatusCode(499, "Request cancelled"); }
        }
    }
}
