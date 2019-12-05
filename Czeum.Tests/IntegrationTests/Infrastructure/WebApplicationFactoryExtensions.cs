using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czeum.Tests.IntegrationTests.Infrastructure
{
    public static class WebApplicationFactoryExtensions
    {
        public static Task RunWithInjectionAsync<TService>(this CzeumFactory factory, Func<TService, Task> action)
        {
            using var scope = factory.Services.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<TService>();
            return action(service);
        }
    }
}
