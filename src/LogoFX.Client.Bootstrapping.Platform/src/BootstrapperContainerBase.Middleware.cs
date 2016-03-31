using System.Collections.Generic;
#if NET45
using Caliburn.Micro;
#endif
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    public partial class BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer>
    {
        private readonly
            List<IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer>>>
            _middlewares =
                new List<IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer>>>();

        /// <summary>
        /// Uses the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer> Use(
            IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer>> middleware)
        {
            _middlewares.Add(middleware);
            return this;
        }
    }

    public partial class BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>
    {
        private readonly List<IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>>> _middlewares =
            new List<IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>>>();

        /// <summary>
        /// Uses the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> Use(
            IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>> middleware)
        {
            _middlewares.Add(middleware);
            return this;
        }
    }

    /// <summary>
    /// Registers composition modules into the ioc container using an external lifetime scope provider.
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <typeparam name="TIocContainer">The type of the ioc container.</typeparam>    
    public class RegisterScopedMiddleware<TRootViewModel, TIocContainerAdapter, TIocContainer> :
        IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer>>
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter<TIocContainer>, IBootstrapperAdapter, new()
        where TIocContainer : class
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer> Apply(
            BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer> @object)
        {
            ModuleRegistrationHelper.RegisterCompositionModules(@object.Container, @object.Modules, () => @object.CurrentLifetimeScope);
            return @object;
        }
    }

    /// <summary>
    /// Registers the core application components (root view model, ioc container etc.) into the ioc container.
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterCoreMiddleware<TRootViewModel, TIocContainerAdapter> :
        IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>>
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()        
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> Apply(
            BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> @object)
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
        IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>>
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> Apply(
            BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> @object)
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
        IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>>
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> Apply(
            BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> @object)
        {
            ModuleRegistrationHelper.RegisterCompositionModules(@object.ContainerAdapter,
                @object.Modules);
            return @object;
        }
    }

    /// <summary>
    /// Registers platform-specific services into the ioc container.
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterPlatformSpecificMiddleware<TRootViewModel, TIocContainerAdapter> :
        IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>>
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> Apply(
            BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> @object)
        {
#if NET45
            @object.ContainerAdapter.RegisterSingleton<IWindowManager, WindowManager>();
#endif
            return @object;
        }
    }
}
