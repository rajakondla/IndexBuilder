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
                  moduleServices.AddSingleton(implementation);
            }

            return moduleServices.BuildServiceProvider();
        }
    }
}
