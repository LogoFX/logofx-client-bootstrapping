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
        private const string ViewModelEnding = "ViewModel";

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
                .Where(type => excludedTypes.Contains(type) == false && type.Name.EndsWith(ViewModelEnding))                
                .Where(type => type.GetTypeInfo().ImplementedInterfaces.Contains(typeof (INotifyPropertyChanged)));

            viewModelTypes.Aggregate(dependencyRegistrator, (seed, next) => seed.AddTransient(next, next));            
        }

        /// <summary>
        /// Registers the view models as contracts.
        /// </summary>
        /// <param name="dependencyRegistrator">The dependency registrator.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="excludedTypes">The types to be excluded from the registration.</param>
        public static void RegisterViewModelsAsContracts(
            this IDependencyRegistrator dependencyRegistrator,
            IEnumerable<Assembly> assemblies,
            IEnumerable<Type> excludedTypes)
        {
            var allTypes = assemblies.SelectMany(assembly => assembly.ExportedTypes);
            var contractCandidates = allTypes.Where(type => excludedTypes.Contains(type) == false && 
            type.Name.EndsWith(ViewModelEnding)
            && type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(INotifyPropertyChanged)));
            foreach (var contract in contractCandidates)
            {
                var match = allTypes.FirstOrDefault(impl => 
                contract.Name == "I" + impl 
                && excludedTypes.Contains(impl) == false
                && impl.GetTypeInfo().ImplementedInterfaces.Contains(contract));
                if (match != null)
                {
                    dependencyRegistrator.AddTransient(contract, match);
                }
            }            
        }
    }
}