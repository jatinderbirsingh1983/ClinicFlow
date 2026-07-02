using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicFlow.Application.DTOs.Patients;
using ClinicFlow.Application.Interfaces;
using ClinicFlow.Domain.Entities;

namespace ClinicFlow.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<PatientResponse> CreateAsync(CreatePatientRequest request)
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
            CreatedOn = DateTime.UtcNow
        };

        var savedPatient = await _patientRepository.AddAsync(patient);
        return MapToResponse(savedPatient);
    }

    public async Task<List<PatientResponse>> GetAllAsync()
    {
        var patients = await _patientRepository.GetAllAsync();
        return patients.Select(MapToResponse).ToList();
    }

    public async Task<PatientResponse?> GetByIdAsync(Guid id)
    {
        var patient = await _patientRepository.GetByIdAsync(id);
        return patient is null ? null : MapToResponse(patient);
    }

    private static PatientResponse MapToResponse(Patient patient)
    {
        return new PatientResponse
        {
            Id = patient.Id,
            FullName = $"{patient.FirstName} {patient.LastName}",
            DateOfBirth = patient.DateOfBirth,
            Gender = patient.Gender,
            PhoneNumber = patient.PhoneNumber,
            Email = patient.Email
        };
    }
}