using Microsoft.EntityFrameworkCore;
using ClinicFlow.Domain.Entities;

namespace ClinicFlow.Infrastructure.Persistence;

public class ClinicDbContext : DbContext
{
    public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
        : base(options)
    {
        
    }

        public DbSet<Patient> Patients => Set<Patient>();
}