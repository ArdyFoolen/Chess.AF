using AF.Bootstrapper;
using Chess.AF.ChessForm.Extensions;
using Chess.AF.Controllers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chess.AF.ChessForm.Helpers
{
    public class ConfigurationHelper
    {
        private static IConfigurationRoot config;
        static ConfigurationHelper()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true);
            config = builder.Build();
        }

        public static void RegisterInterfaces()
        {
            var loadAssembliesSection = config.GetSection("LoadAssemblies");
            var loadAssemblies = config.GetInstance<List<LoadAssembly>>(loadAssembliesSection.Key);
            foreach (var loadAssembly in loadAssemblies)
            {
                Assembly assembly = Assembly.LoadFile(loadAssembly.AssemblyPath);
                var interfaceType = Type.GetType(loadAssembly.InterfaceType);
                var implementationType = assembly.GetTypes().Where(p => p.GetInterfaces().Any(t => t.FullName.Equals(interfaceType.FullName))).FirstOrDefault();

                Container.Instance.Register(interfaceType, implementationType);
            }
        }
    }
}
