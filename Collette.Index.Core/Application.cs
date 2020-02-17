using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using Collette.Commands;

namespace Collette.Index.Core
{
    public class Application
    {
        public Application(AssemblyName name)
        {
            Name = name;
        }

        public AssemblyName Name { get; }

        public void InitializeServices(IConfiguration configuration)
        {
            var factory = new Provider(configuration);

            var assemblies = ScanAssembly("Collette.Index.*.dll").ToList();
            assemblies.AddRange(ScanAssembly("Collette.Utilities.*.dll"));
            var serviceProvider = factory.CreateServiceContainer(assemblies);
            ServiceLocator.SetLocatorProvider(serviceProvider);
        }

        public static IEnumerable<Assembly> ScanAssembly(string searchPattern)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;

            return Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories).Select(Assembly.LoadFrom);
        }
    }
}
