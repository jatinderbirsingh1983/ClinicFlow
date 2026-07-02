using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicFlow.Domain.Entities;

namespace ClinicFlow.Application.Interfaces;

public interface IPatientRepository
{
    Task<Patient> AddAsync(Patient patient);
    Task<List<Patient>> GetAllAsync();
    Task<Patient?> GetByIdAsync(Guid id);
}