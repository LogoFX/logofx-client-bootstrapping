using System;
using Caliburn.Micro;
using LogoFX.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using LogoFX.Core;
using Solid.Extensibility;
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
        private readonly MiddlewaresWrapper<IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>> _middlewaresWrapper;

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer> Use(
            IMiddleware<IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>> middleware)
        {
            _middlewaresWrapper.Use(middleware);
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
        private readonly MiddlewaresWrapper<IBootstrapperWithRegistrator> _registratorMiddlewaresWrapper;       

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public IBootstrapperWithRegistrator Use(
            IMiddleware<IBootstrapperWithRegistrator> middleware)
        {
            _registratorMiddlewaresWrapper.Use(middleware);
            return this;
        }

        private readonly MiddlewaresWrapper<IBootstrapperWithContainerAdapter<TIocContainerAdapter>> _middlewaresWrapper;       

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TIocContainerAdapter> Use(
            IMiddleware<IBootstrapperWithContainerAdapter<TIocContainerAdapter>> middleware)
        {
            _middlewaresWrapper.Use(middleware);
            return this;
        }

        private readonly MiddlewaresWrapper<
#if TEST
    TestBootstrapperContainerBase
#else
            BootstrapperContainerBase
#endif
            <TIocContainerAdapter>> _concreteMiddlewaresWrapper;                   

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
            _concreteMiddlewaresWrapper.Use(middleware);           
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
            _concreteMiddlewaresWrapper.Use(
                new MiddlewareDecorator<
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
#if NET || NETCORE || NETFRAMEWORK
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
        private readonly bool _registerRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateRootObjectMiddleware{TIocContainerAdapter}"/> class.
        /// </summary>        
        /// <param name="rootObjectType">Type of the root object.</param>
        /// <param name="displayView">if set to <c>true</c> the root view is displayed.</param>
        /// <param name="registerRoot">if set to <c>true</c> the root is registered.</param>
        public CreateRootObjectMiddleware(            
            Type rootObjectType,
            bool displayView, 
            bool registerRoot)
        {           
            _rootObjectType = rootObjectType;
            _displayView = displayView;
            _registerRoot = registerRoot;
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
            if (_registerRoot)
            {
                @object.Registrator.RegisterSingleton(_rootObjectType, _rootObjectType);
            }
            
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
            var middleware = new Solid.Bootstrapping.RegisterResolverMiddleware
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
