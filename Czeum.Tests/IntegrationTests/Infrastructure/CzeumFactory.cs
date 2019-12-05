using System;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Czeum.DAL;
using Czeum.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Czeum.Tests.IntegrationTests.Infrastructure
{
    public class CzeumFactory : WebApplicationFactory<TestStartup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webHost => webHost.UseStartup<TestStartup>());
        }

        public async Task SeedUsersAsync()
        {
            using var scope = Server.Services.CreateScope();
            
            var context = scope.ServiceProvider.GetRequiredService<CzeumContext>();
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            
            var test1 = new User
            {
                UserName = "teszt1",
                Email = "teszt1@teszt.hu"
            };

            var test2 = new User
            {
                UserName = "teszt2",
                Email = "teszt2@teszt.hu"
            };

            await userManager.CreateAsync(test1, "Alma123");
            await userManager.CreateAsync(test2, "Alma123");
        }
    }
}