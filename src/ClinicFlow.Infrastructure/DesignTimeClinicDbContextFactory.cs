using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ClinicFlow.Infrastructure.Persistence;

namespace ClinicFlow.Infrastructure;

public class DesignTimeClinicDbContextFactory : IDesignTimeDbContextFactory<ClinicDbContext>
{
    public ClinicDbContext CreateDbContext(string[] args)
    {
        var apiProjectDir = FindApiProjectDirectory();

        var connectionString = ReadConnectionString(apiProjectDir, "ClinicDb");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"Connection string 'ClinicDb' is missing. Checked path: {apiProjectDir}");
        }

        var optionsBuilder = new DbContextOptionsBuilder<ClinicDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new ClinicDbContext(optionsBuilder.Options);
    }

    private static string ReadConnectionString(string apiProjectDir, string name)
    {
        var appsettingsPath = Path.Combine(apiProjectDir, "appsettings.json");
        if (!File.Exists(appsettingsPath))
        {
            throw new FileNotFoundException($"Could not find appsettings.json at {appsettingsPath}");
        }

        using var stream = File.OpenRead(appsettingsPath);
        var document = JsonDocument.Parse(stream);
        if (!document.RootElement.TryGetProperty("ConnectionStrings", out var connectionStringsElement))
        {
            throw new InvalidOperationException("appsettings.json does not contain a ConnectionStrings section.");
        }

        if (!connectionStringsElement.TryGetProperty(name, out var connectionStringElement))
        {
            throw new InvalidOperationException($"Connection string '{name}' does not exist in appsettings.json.");
        }

        return connectionStringElement.GetString() ?? string.Empty;
    }

    private static string FindApiProjectDirectory()
    {
        var searchRoots = new[]
        {
            Directory.GetCurrentDirectory(),
            AppContext.BaseDirectory,
            Path.GetDirectoryName(typeof(DesignTimeClinicDbContextFactory).Assembly.Location) ?? string.Empty
        }
        .Where(p => !string.IsNullOrWhiteSpace(p))
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToArray();

        foreach (var root in searchRoots)
        {
            var directory = new DirectoryInfo(root);
            while (directory != null)
            {
                var candidatePaths = new[]
                {
                    Path.Combine(directory.FullName, "src", "ClinicFlow.API"),
                    Path.Combine(directory.FullName, "ClinicFlow.API"),
                };

                foreach (var candidate in candidatePaths)
                {
                    if (Directory.Exists(candidate) && File.Exists(Path.Combine(candidate, "appsettings.json")))
                    {
                        return candidate;
                    }
                }

                directory = directory.Parent;
            }
        }

        throw new InvalidOperationException("Could not locate the ClinicFlow.API project directory with appsettings.json. Verify the repository layout and the design-time factory.");
    }
}
