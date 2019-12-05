using Czeum.DAL;
using Czeum.Web;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Czeum.Tests.IntegrationTests.Infrastructure
{
    public class TestStartup : Startup
    {
        private SqliteConnection sqliteConnection;
        
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
            sqliteConnection = new SqliteConnection("Data Source=:memory:");
        }

        public override void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication()
                .AddTestAuthentication();
        }

        public override void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<CzeumContext>(options =>
                options.UseSqlite(sqliteConnection));
        }

        public override void ConfigureControllers(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson()
                .AddApplicationPart(Assembly.Load("Czeum.Web"))
                .AddControllersAsServices();
        }
    }
}