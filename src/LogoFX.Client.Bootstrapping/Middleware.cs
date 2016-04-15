using LogoFX.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Registers the core application components (root object, ioc container etc.) into the ioc container.
    /// </summary>
    /// <typeparam name="TRootObject">The type of the root object.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterCoreMiddleware<TRootObject, TIocContainerAdapter> :
        IMiddleware<IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter>>
        where TRootObject : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> Apply(
            IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> @object)
        {
            BootstrapperHelper<TRootObject, TIocContainerAdapter>.RegisterCore(@object.ContainerAdapter);
            return @object;
        }
    }    

    /// <summary>
    /// Registers automagically the application's view models in the transient lifestyle.
    /// </summary>
    /// <typeparam name="TRootObject">The type of the root object.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterViewModelsMiddleware<TRootObject, TIocContainerAdapter> :
        IMiddleware<IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter>>
        where TRootObject : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> Apply(
            IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> @object)
        {
            BootstrapperHelper<TRootObject, TIocContainerAdapter>.RegisterViewModels(
                @object.ContainerAdapter, @object.Assemblies);
            return @object;
        }
    }

    /// <summary>
    /// Registers composition modules into the ioc container adapter.
    /// </summary>
    /// <typeparam name="TRootObject">The type of the root object.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterCompositionModulesMiddleware<TRootObject, TIocContainerAdapter> :
        IMiddleware<IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter>>
        where TRootObject : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> Apply(
            IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> @object)
        {
            ModuleRegistrationHelper.RegisterCompositionModules(@object.ContainerAdapter,
                @object.Modules);
            return @object;
        }
    }

    /// <summary>
    /// Registers composition modules into the ioc container.
    /// </summary>
    /// <typeparam name="TRootObject">The type of the root object.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <typeparam name="TIocContainer">The type of the ioc container.</typeparam>    
    public class RegisterCompositionModulesMiddleware<TRootObject, TIocContainerAdapter, TIocContainer> :
        IMiddleware<IBootstrapperWithContainer<TRootObject, TIocContainerAdapter, TIocContainer>>
        where TRootObject : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new() 
        where TIocContainer : class
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainer<TRootObject, TIocContainerAdapter, TIocContainer> Apply(
            IBootstrapperWithContainer<TRootObject, TIocContainerAdapter, TIocContainer> @object)
        {
            ModuleRegistrationHelper.RegisterCompositionModules(@object.Container,
                @object.Modules);
            return @object;
        }
    }
}
