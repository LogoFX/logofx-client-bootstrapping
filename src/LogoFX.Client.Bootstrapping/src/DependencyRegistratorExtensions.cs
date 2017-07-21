using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// The dependency registrator extension methods.
    /// </summary>
    public static class DependencyRegistratorExtensions
    {
        /// <summary>
        /// Registers the view models.
        /// </summary>
        /// <param name="dependencyRegistrator">The dependency registrator.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="excludedTypes">The types to be excluded from the registration.</param>
        public static void RegisterViewModels(
            this IDependencyRegistrator dependencyRegistrator,
            IEnumerable<Assembly> assemblies,
            IEnumerable<Type> excludedTypes)
        {
            var viewModelTypes = assemblies
                .SelectMany(assembly => assembly.ExportedTypes)
                .Where(type => excludedTypes.Contains(type) == false && type.Name.EndsWith("ViewModel"))                
                .Where(type => type.GetTypeInfo().ImplementedInterfaces.Contains(typeof (INotifyPropertyChanged)));

            foreach (var viewModelType in viewModelTypes)
            {
                dependencyRegistrator.RegisterTransient(viewModelType, viewModelType);
            }
        }
    }
}