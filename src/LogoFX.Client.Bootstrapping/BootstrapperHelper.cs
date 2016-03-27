using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Contains cross-platform bootstrapper utilities.
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container.</typeparam>
    public static class BootstrapperHelper<TRootViewModel, TIocContainerAdapter> 
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer
    {        
        /// <summary>
        /// Registers the IoC container and root view model.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc containeradapter .</param>
        public static void RegisterCore(TIocContainerAdapter iocContainerAdapter)                                    
        {
            iocContainerAdapter.RegisterSingleton<TRootViewModel, TRootViewModel>();
            iocContainerAdapter.RegisterInstance(iocContainerAdapter);
            iocContainerAdapter.RegisterInstance<IIocContainer>(iocContainerAdapter);            
        }

        /// <summary>
        /// Registers the views and view models.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>
        /// <param name="assemblies">The assemblies.</param>
        public static void RegisterViewsAndViewModels(IIocContainerRegistrator iocContainerAdapter,
            IEnumerable<Assembly> assemblies)
        {
            var viewModelTypes = assemblies
                .SelectMany(assembly => assembly.ExportedTypes)
                .Where(type => type != typeof(TRootViewModel) && type.Name.EndsWith("ViewModel"))
                .Where(
                    type =>
                        !string.IsNullOrWhiteSpace(type.Namespace) && type.Namespace != null &&
                        type.Namespace.EndsWith("ViewModels"))
                .Where(type => type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(INotifyPropertyChanged)));
            foreach (var viewModelType in viewModelTypes)
            {
                iocContainerAdapter.RegisterTransient(viewModelType, viewModelType);
            }            
        }        
    }
}
