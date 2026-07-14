using ClinicFlow.Application.Departments;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentsController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DepartmentResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var departments =
            await _departmentService.GetAllAsync(cancellationToken);

        return Ok(departments);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DepartmentResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var department =
            await _departmentService.GetByIdAsync(id, cancellationToken);

        if (department is null)
        {
            return NotFound();
        }

        return Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentResponse>> Create(
        CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var department =
            await _departmentService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = department.Id },
            department);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var updated =
            await _departmentService.UpdateAsync(
                id,
                request,
                cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var deleted =
            await _departmentService.DeleteAsync(id, cancellationToken);

        return deleted ? NoContent() : NotFound();
    }
}