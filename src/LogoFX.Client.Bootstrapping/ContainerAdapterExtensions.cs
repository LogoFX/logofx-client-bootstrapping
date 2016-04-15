using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// The ioc container adapter extension methods.
    /// </summary>
    public static class ContainerAdapterExtensions
    {
        /// <summary>
        /// Registers the view models.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="rootObjectType">The root object type.</param>
        public static void RegisterViewModels(
            this IIocContainerRegistrator iocContainerAdapter,
            IEnumerable<Assembly> assemblies,
            Type rootObjectType)
        {
            var viewModelTypes = assemblies
                .SelectMany(assembly => assembly.ExportedTypes)
                .Where(type => type != rootObjectType && type.Name.EndsWith("ViewModel"))
                .Where(
                    type =>
                        !string.IsNullOrWhiteSpace(type.Namespace) && type.Namespace != null &&
                        type.Namespace.EndsWith("ViewModels"))
                .Where(type => type.GetTypeInfo().ImplementedInterfaces.Contains(typeof (INotifyPropertyChanged)));

            foreach (var viewModelType in viewModelTypes)
            {
                iocContainerAdapter.RegisterTransient(viewModelType, viewModelType);
            }
        }
    }
}