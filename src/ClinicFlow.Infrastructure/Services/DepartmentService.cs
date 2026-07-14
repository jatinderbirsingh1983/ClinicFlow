using ClinicFlow.Application.Departments;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Infrastructure.Services;

public class DepartmentService : IDepartmentService
{
    private readonly ClinicDbContext _dbContext;

    public DepartmentService(ClinicDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<DepartmentResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Departments
            .AsNoTracking()
            .OrderBy(department => department.Name)
            .Select(department => new DepartmentResponse
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                CreatedAtUtc = department.CreatedAtUtc
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<DepartmentResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Departments
            .AsNoTracking()
            .Where(department => department.Id == id)
            .Select(department => new DepartmentResponse
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                CreatedAtUtc = department.CreatedAtUtc
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<DepartmentResponse> CreateAsync(
        CreateDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        _dbContext.Departments.Add(department);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new DepartmentResponse
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            IsActive = department.IsActive,
            CreatedAtUtc = department.CreatedAtUtc
        };
    }

    public async Task<bool> UpdateAsync(
        Guid id,
        UpdateDepartmentRequest request,
        CancellationToken cancellationToken = default)
    {
        var department = await _dbContext.Departments
            .FirstOrDefaultAsync(
                department => department.Id == id,
                cancellationToken);

        if (department is null)
        {
            return false;
        }

        department.Name = request.Name.Trim();
        department.Description = request.Description?.Trim();
        department.IsActive = request.IsActive;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var department = await _dbContext.Departments
            .FirstOrDefaultAsync(
                department => department.Id == id,
                cancellationToken);

        if (department is null)
        {
            return false;
        }

        _dbContext.Departments.Remove(department);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}