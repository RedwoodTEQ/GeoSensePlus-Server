using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GeoSensePlus.Data
{
    public static class DataExtension
    {
        public static void AddPostgres(this IServiceCollection services, IConfiguration Configuration)
        {
            bool config_UnitTestConnectionEnabled = Configuration.GetValue<bool>("UnitTestConnectionEnabled");

            string postgresConnStr = "PostgresConnection";
            if (config_UnitTestConnectionEnabled)
            {
                postgresConnStr = "PostgresConnection_UnitTest";
            }

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(
                Configuration.GetConnectionString(postgresConnStr),
                postgresOption => postgresOption.MigrationsAssembly("GeoSensePlus.Data")
            ));

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<ApplicationDbContext>();
        }
    }
}
