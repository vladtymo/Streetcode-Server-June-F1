using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Streetcode.DAL.Persistence;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Streetcode.DAL
{
    public class StreetcodeDbContextFactory : IDesignTimeDbContextFactory<StreetcodeDbContext>
    {
        public StreetcodeDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";

            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables("STREETCODE_")
            .AddUserSecrets<StreetcodeDbContextFactory>()
            .Build();

            var connectionString = configuration.GetSection(environment).GetConnectionString("DefaultConnection")
                                  ?? configuration.GetConnectionString("DefaultConnection")
                                  ?? throw new InvalidOperationException($"'{environment}.DefaultConnection' not found!");

            var optionsBuilder = new DbContextOptionsBuilder<StreetcodeDbContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return new StreetcodeDbContext(optionsBuilder.Options);
        }
    }
}
