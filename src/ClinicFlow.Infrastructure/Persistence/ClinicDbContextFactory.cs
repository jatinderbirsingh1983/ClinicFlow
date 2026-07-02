using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ClinicFlow.Infrastructure.Persistence;

public class ClinicDbContextFactory : IDesignTimeDbContextFactory<ClinicDbContext>
{
    public ClinicDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ClinicDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=ClinicFlowDb;Username=jatinderbirsingh1983gmaail.com;Password=");

        return new ClinicDbContext(optionsBuilder.Options);
    }
}