using System;
using System.Collections.Generic;
using Caliburn.Micro;
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

    class RegisterScopedMiddleware<TRootViewModel, TIocContainerAdapter, TIocContainer> :
        IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer>>
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter<TIocContainer>, IBootstrapperAdapter, new()
        where TIocContainer : class
    {
        public BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer> Apply(
            BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer> @object)
        {
            ModuleRegistrationHelper.RegisterCompositionModules(@object.Container, @object.Modules, () => @object.CurrentLifetimeScope);
            return @object;
        }
    }
    
    class RegisterCoreMiddleware<TRootViewModel, TIocContainerAdapter> :
        IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>>
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()        
    {
        public BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> Apply(
            BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> @object)
        {
            BootstrapperHelper<TRootViewModel, TIocContainerAdapter>.RegisterCore(@object.ContainerAdapter);
            return @object;
        }
    }

    class RegisterViewAndViewModelsMiddleware<TRootViewModel, TIocContainerAdapter> :
        IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>>
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {
        public BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> Apply(
            BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> @object)
        {
            BootstrapperHelper<TRootViewModel, TIocContainerAdapter>.RegisterViewsAndViewModels(
                @object.ContainerAdapter, @object.Assemblies);
            return @object;
        }
    }

    class RegisterCompositionModulesMiddleware<TRootViewModel, TIocContainerAdapter> :
        IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>>
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {
        public BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> Apply(
            BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> @object)
        {
            ModuleRegistrationHelper.RegisterCompositionModules(@object.ContainerAdapter,
                @object.Modules);
            return @object;
        }
    }

    class RegisterPlatformSpecificMiddleware<TRootViewModel, TIocContainerAdapter> :
        IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>>
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {
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
