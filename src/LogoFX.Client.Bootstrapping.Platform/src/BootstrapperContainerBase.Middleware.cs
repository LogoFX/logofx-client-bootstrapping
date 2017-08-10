using System;
using System.Collections.Generic;
using Caliburn.Micro;
using LogoFX.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using LogoFX.Core;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    public partial class
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
    <TIocContainerAdapter, TIocContainer>
    {
        private readonly
            List<IMiddleware<IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>>>
            _middlewares =
                new List<IMiddleware<IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>>>();

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer> Use(
            IMiddleware<IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>> middleware)
        {
            _middlewares.Add(middleware);
            return this;
        }
    }

    public partial class
#if TEST
    TestBootstrapperContainerBase
#else
        BootstrapperContainerBase
#endif
        <TIocContainerAdapter>
    {
        private readonly ExtensibleHelper<IBootstrapperWithRegistrator> _registratorMiddlewareHelper;       

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public IBootstrapperWithRegistrator Use(
            IMiddleware<IBootstrapperWithRegistrator> middleware)
        {
            _registratorMiddlewareHelper.Use(middleware);
            return this;
        }

        private readonly ExtensibleHelper<IBootstrapperWithContainerAdapter<TIocContainerAdapter>> _middlewareHelper;       

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TIocContainerAdapter> Use(
            IMiddleware<IBootstrapperWithContainerAdapter<TIocContainerAdapter>> middleware)
        {
            _middlewareHelper.Use(middleware);
            return this;
        }

        private readonly ExtensibleHelper<
#if TEST
    TestBootstrapperContainerBase
#else
            BootstrapperContainerBase
#endif
            <TIocContainerAdapter>> _concreteMiddlewareHelper;                   

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public
#if TEST
    TestBootstrapperContainerBase
#else
            BootstrapperContainerBase
#endif
            <TIocContainerAdapter> Use(
                IMiddleware<
#if TEST
    TestBootstrapperContainerBase
#else
                    BootstrapperContainerBase
#endif
                    <TIocContainerAdapter>> middleware)
        {
            _concreteMiddlewareHelper.Use(middleware);           
            return this;
        }

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>        
        /// <returns></returns>
        public
#if TEST
    TestBootstrapperContainerBase
#else
            BootstrapperContainerBase
#endif
            <TIocContainerAdapter> Use<TMiddleware>()
            where TMiddleware : class, IMiddleware<
#if TEST
    TestBootstrapperContainerBase
#else
                BootstrapperContainerBase
#endif
                <TIocContainerAdapter>>
        {
            _concreteMiddlewareHelper.Use(
                new InjectableMiddleware<
#if TEST
    TestBootstrapperContainerBase
#else
                    BootstrapperContainerBase
#endif
                    <TIocContainerAdapter>,
                    TMiddleware>(ContainerAdapter));
            return this;
        }
    }

/// <summary>
/// Registers platform-specific services into the ioc container.
/// </summary>
public class RegisterPlatformSpecificMiddleware :
        IMiddleware<IBootstrapperWithRegistrator>
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithRegistrator Apply(
            IBootstrapperWithRegistrator @object)
        {
#if NET45
            @object.Registrator.RegisterSingleton<IWindowManager, WindowManager>();
#endif
            return @object;
        }
    }

    /// <summary>
    /// Registers root object into the ioc container adapter and 
    /// optionally displays it when the application starts.
    /// </summary>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class CreateRootObjectMiddleware<TIocContainerAdapter> :
        IMiddleware<
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            <TIocContainerAdapter>>
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter         
    {
        private readonly Type _rootObjectType;
        private readonly bool _displayView;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateRootObjectMiddleware{TIocContainerAdapter}"/> class.
        /// </summary>
        /// <param name="rootObjectType">Type of the root object.</param>
        /// <param name="displayView">if set to <c>true</c> the root view is displayed.</param>
        public CreateRootObjectMiddleware(Type rootObjectType, bool displayView)
        {
            _rootObjectType = rootObjectType;
            _displayView = displayView;
        }

        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            <TIocContainerAdapter> Apply(
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            <TIocContainerAdapter> @object)
        {
            @object.Registrator.RegisterSingleton(_rootObjectType, _rootObjectType);
            EventHandler strongHandler = ObjectOnInitializationCompleted;
            @object.InitializationCompleted += WeakDelegate.From(strongHandler);
            return @object;
        }

        private void ObjectOnInitializationCompleted(object sender, EventArgs eventArgs)
        {
            var bootstrapper = sender as
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
                <TIocContainerAdapter>;
            if (bootstrapper != null)
            {
                if (_displayView)
                {
#if !TEST
                    bootstrapper.DisplayRootViewForInternal(_rootObjectType);
#endif
                }
                else
                {
                    var rootObject = bootstrapper.ContainerAdapter.Resolve(_rootObjectType);
                    ScreenExtensions.TryActivate(rootObject);
                }
            }
        }
    }    

    /// <summary>
    /// Registers the ioc container adapter.
    /// </summary>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    class RegisterContainerAdapterMiddleware<TIocContainerAdapter> :
        IMiddleware<
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
                <TIocContainerAdapter>>
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
                <TIocContainerAdapter>
            Apply(
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
                <TIocContainerAdapter>
            @object)
        {
            @object.Registrator.RegisterInstance(@object.ContainerAdapter);
            return @object;
        }
    }    

    /// <summary>
    /// Registers the ioc container resolver.
    /// </summary>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterResolverMiddleware<TIocContainerAdapter> :
        IMiddleware<
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
                <TIocContainerAdapter>>
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
                <TIocContainerAdapter>
            Apply(
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
                <TIocContainerAdapter>
            @object)
        {
            var middleware = new LogoFX.Bootstrapping.RegisterResolverMiddleware
                <
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
                <TIocContainerAdapter>
                >(@object.ContainerAdapter);
            middleware.Apply(@object);
            return @object;
        }
    }
}
