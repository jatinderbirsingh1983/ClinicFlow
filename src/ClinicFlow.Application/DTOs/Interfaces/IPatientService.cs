using ClinicFlow.Application.DTOs.Patients;

namespace ClinicFlow.Application.Interfaces;

public interface IPatientService
{
    Task<PatientResponse> CreateAsync(CreatePatientRequest request);

    Task<List<PatientResponse>> GetAllAsync();

    Task<PatientResponse?> GetByIdAsync(Guid id);
}