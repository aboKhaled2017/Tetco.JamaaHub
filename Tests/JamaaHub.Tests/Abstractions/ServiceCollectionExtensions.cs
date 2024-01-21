using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamaaHub.API.Tests.Abstractions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RemoveService<TService>(this IServiceCollection services)
        {
            // Create a new service collection excluding the specified service
            var newServices = new ServiceCollection();
            foreach (var serviceDescriptor in services)
            {
                if (serviceDescriptor.ServiceType != typeof(TService))
                {
                    newServices.Add(serviceDescriptor);
                }
            }

            return newServices;
        }
    }
}
