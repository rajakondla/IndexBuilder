using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Collette.Index.Core
{
    public class Provider
    {
        private readonly IConfiguration _configuration;

        public Provider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceProvider CreateServiceContainer(IEnumerable<Assembly> assemblies)
        {
            IServiceCollection moduleServices = new ServiceCollection();

            moduleServices.AddSingleton(_configuration);

            var loadedAssemblies = assemblies.ToList();

            foreach (var implementation in loadedAssemblies
                .SelectMany(x => x.GetTypes())
                .Where(t =>
                    !t.IsAbstract
                    && !t.IsInterface
                    && (typeof(BaseIndex).IsAssignableFrom(t) || typeof(IPublisher).IsAssignableFrom(t))))
            {
                  moduleServices.AddTransient(implementation);
            }

            foreach (var implementation in loadedAssemblies
                .SelectMany(x => x.GetTypes())
                .Where(t =>
                    !t.IsAbstract
                    && !t.IsInterface
                    && (t.Name.EndsWith("Repository") || t.Name.EndsWith("Comparer"))))
            {
                moduleServices.AddTransient(implementation.GetInterface("I"+implementation.Name), implementation);
            }


            return moduleServices.BuildServiceProvider();
        }
    }
}
