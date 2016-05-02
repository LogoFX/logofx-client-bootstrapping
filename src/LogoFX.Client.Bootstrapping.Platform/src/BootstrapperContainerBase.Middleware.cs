﻿using System;
using System.Collections.Generic;
using Caliburn.Micro;
using LogoFX.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using LogoFX.Core;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    public partial class BootstrapperContainerBase<TIocContainerAdapter, TIocContainer>
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

    public partial class BootstrapperContainerBase<TIocContainerAdapter>
    {
        private readonly List<IMiddleware<IBootstrapperWithContainerAdapter<TIocContainerAdapter>>> _middlewares =
            new List<IMiddleware<IBootstrapperWithContainerAdapter<TIocContainerAdapter>>>();

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TIocContainerAdapter> Use(
            IMiddleware<IBootstrapperWithContainerAdapter<TIocContainerAdapter>> middleware)
        {
            _middlewares.Add(middleware);
            return this;
        }

        private readonly List<IMiddleware<BootstrapperContainerBase<TIocContainerAdapter>>> _concreteMiddlewares =
            new List<IMiddleware<BootstrapperContainerBase<TIocContainerAdapter>>>();

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public BootstrapperContainerBase<TIocContainerAdapter> Use(
            IMiddleware<BootstrapperContainerBase<TIocContainerAdapter>> middleware)
        {
            _concreteMiddlewares.Add(middleware);
            return this;
        }
    }                

    /// <summary>
    /// Registers platform-specific services into the ioc container.
    /// </summary>    
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterPlatformSpecificMiddleware<TIocContainerAdapter> :
        IMiddleware<IBootstrapperWithContainerAdapter<TIocContainerAdapter>>        
        where TIocContainerAdapter : class, IIocContainer
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TIocContainerAdapter> Apply(
            IBootstrapperWithContainerAdapter<TIocContainerAdapter> @object)
        {
#if NET45
            @object.ContainerAdapter.RegisterSingleton<IWindowManager, WindowManager>();
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
        IMiddleware<BootstrapperContainerBase<TIocContainerAdapter>>
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
        public BootstrapperContainerBase<TIocContainerAdapter> Apply(
            BootstrapperContainerBase<TIocContainerAdapter> @object)
        {
            @object.ContainerAdapter.RegisterSingleton(_rootObjectType, _rootObjectType);
            EventHandler strongHandler = ObjectOnInitializationCompleted;
            @object.InitializationCompleted += WeakDelegate.From(strongHandler);
            return @object;
        }

        private void ObjectOnInitializationCompleted(object sender, EventArgs eventArgs)
        {
            var bootstrapper = sender as
                BootstrapperContainerBase<TIocContainerAdapter>;
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
}