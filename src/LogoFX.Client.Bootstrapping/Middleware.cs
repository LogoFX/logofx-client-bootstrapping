using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Registers the core application components (root view model, ioc container etc.) into the ioc container.
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterCoreMiddleware<TRootViewModel, TIocContainerAdapter> :
        IMiddleware<IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter>>
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter> Apply(
            IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter> @object)
        {
            BootstrapperHelper<TRootViewModel, TIocContainerAdapter>.RegisterCore(@object.ContainerAdapter);
            return @object;
        }
    }    

    /// <summary>
    /// Registers automagically the application's view models in the transient lifestyle.
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterViewModelsMiddleware<TRootViewModel, TIocContainerAdapter> :
        IMiddleware<IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter>>
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter> Apply(
            IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter> @object)
        {
            BootstrapperHelper<TRootViewModel, TIocContainerAdapter>.RegisterViewModels(
                @object.ContainerAdapter, @object.Assemblies);
            return @object;
        }
    }

    /// <summary>
    /// Registers composition modules into the ioc container.
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterCompositionModulesMiddleware<TRootViewModel, TIocContainerAdapter> :
        IMiddleware<IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter>>
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter> Apply(
            IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter> @object)
        {
            ModuleRegistrationHelper.RegisterCompositionModules(@object.ContainerAdapter,
                @object.Modules);
            return @object;
        }
    }    
}
