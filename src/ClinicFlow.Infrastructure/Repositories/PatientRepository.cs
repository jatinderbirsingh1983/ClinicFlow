using ClinicFlow.Application.Interfaces;
using ClinicFlow.Domain.Entities;
using ClinicFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly ClinicDbContext _context;

    public PatientRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task<Patient> AddAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
        return patient;
    }

    public async Task<List<Patient>> GetAllAsync()
    {
        return await _context.Patients
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Patient?> GetByIdAsync(Guid id)
    {
        return await _context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}