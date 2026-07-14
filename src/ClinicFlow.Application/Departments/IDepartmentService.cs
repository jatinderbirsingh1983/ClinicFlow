namespace ClinicFlow.Application.Departments;

public interface IDepartmentService
{
    Task<IReadOnlyList<DepartmentResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<DepartmentResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<DepartmentResponse> CreateAsync(
        CreateDepartmentRequest request,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(
        Guid id,
        UpdateDepartmentRequest request,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
