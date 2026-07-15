using ClinicFlow.Application.DTOs.Patients;
using ClinicFlow.Application.Interfaces;
using ClinicFlow.Domain.Entities;

namespace ClinicFlow.Application.DTOs.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<PatientResponse> CreateAsync(
        CreatePatientRequest request,
        CancellationToken cancellationToken = default)
    {
        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Address = request.Address,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        var added = await _patientRepository.AddAsync(patient);

        return MapToResponse(added);
    }

    public Task<PatientResponse> CreateAsync(CreatePatientRequest request)
        => CreateAsync(request, CancellationToken.None);

    public async Task<List<PatientResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var patients = await _patientRepository.GetAllAsync();

        return patients
            .Select(MapToResponse)
            .ToList();
    }

    public Task<List<PatientResponse>> GetAllAsync()
        => GetAllAsync(CancellationToken.None);

    public async Task<PatientResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var patient = await _patientRepository.GetByIdAsync(id);

        if (patient is null)
        {
            return null;
        }

        return MapToResponse(patient);
    }

    public Task<PatientResponse?> GetByIdAsync(Guid id)
        => GetByIdAsync(id, CancellationToken.None);

    private static PatientResponse MapToResponse(Patient patient)
    {
        return new PatientResponse
        {
            Id = patient.Id,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender,
            PhoneNumber = patient.PhoneNumber,
            Email = patient.Email,
            Address = patient.Address,
            IsActive = patient.IsActive,
            CreatedAtUtc = patient.CreatedAtUtc
        };
    }

    
}