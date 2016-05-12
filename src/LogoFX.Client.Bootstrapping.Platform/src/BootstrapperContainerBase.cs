﻿using System;
using System.Collections.Generic;
using System.Windows.Threading;
using LogoFX.Bootstrapping;
#if NET45 && !TEST
using System.Windows;
#endif
#if !TEST && (NETFX_CORE || WINDOWS_UWP || WIN81)
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
#endif
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Application bootstrapper with ioc container and its adapter.
    /// </summary>    
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <typeparam name="TIocContainer">The type of the ioc container.</typeparam>    
    public partial class
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
        <TIocContainerAdapter, TIocContainer> :
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
        <TIocContainerAdapter>,
                    IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>        
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter<TIocContainer>, IBootstrapperAdapter 
        where TIocContainer : class
    {
        /// <summary>
        /// Application bootstrapper with ioc container, its adapter and root object.
        /// </summary>
        /// <typeparam name="TRootObject"></typeparam>
        public new class WithRootObject<TRootObject> :
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            <TIocContainerAdapter, TIocContainer>
        {
#if TEST
            /// <summary>
            /// Initializes a new instance of <see cref="TestBootstrapperContainerBase{TIocContainerAdapter, TIocContainer}.WithRootObject{TRootObject}"/>
            /// </summary>
            /// <param name="iocContainer">The ioc container.</param>
            /// <param name="adapterCreator">The adapter creation function.</param>
#else
            /// <summary>
            /// Initializes a new instance of <see cref="BootstrapperContainerBase{TIocContainerAdapter, TIocContainer}.WithRootObject{TRootObject}"/>
            /// </summary>
            /// <param name="iocContainer">The ioc container.</param>
            /// <param name="adapterCreator">The adapter creation function.</param>
#endif
            public WithRootObject(TIocContainer iocContainer,
            Func<TIocContainer, TIocContainerAdapter> adapterCreator)
                : this(iocContainer, adapterCreator, new BootstrapperCreationOptions
                {
                    ExcludedTypes = new List<Type> { typeof(TRootObject) }
                })
            {
            }

#if TEST
            /// <summary>
            /// Initializes a new instance of <see cref="TestBootstrapperContainerBase{TIocContainerAdapter, TIocContainer}.WithRootObject{TRootObject}"/>
            /// </summary>
            /// <param name="iocContainer">The ioc container.</param>
            /// <param name="adapterCreator">The adapter creation function.</param>
            /// <param name="creationOptions">The bootstrapper creation options.</param>
#else
            /// <summary>
            /// Initializes a new instance of <see cref="BootstrapperContainerBase{TIocContainerAdapter, TIocContainer}.WithRootObject{TRootObject}"/>
            /// </summary>
            /// <param name="iocContainer">The ioc container.</param>
            /// <param name="adapterCreator">The adapter creation function.</param>
            /// <param name="creationOptions">The bootstrapper creation options.</param>
#endif            
            public WithRootObject(TIocContainer iocContainer,
            Func<TIocContainer, TIocContainerAdapter> adapterCreator,
                BootstrapperCreationOptions creationOptions) : base(iocContainer, adapterCreator, AddRootObject(creationOptions))
            {
                Use(new CreateRootObjectMiddleware<TIocContainerAdapter>(typeof(TRootObject),
                    creationOptions.DisplayRootView));
            }

            private static BootstrapperCreationOptions AddRootObject(BootstrapperCreationOptions creationOptions)
            {
                if (creationOptions.ExcludedTypes == null)
                {
                    creationOptions.ExcludedTypes = new List<Type>();
                }
                if (creationOptions.ExcludedTypes.Contains(typeof(TRootObject)) == false)
                {
                    creationOptions.ExcludedTypes.Add(typeof(TRootObject));
                }
                return creationOptions;
            }
        }
#if TEST
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="TestBootstrapperContainerBase{TIocContainerAdapter, TIocContainer}"/> class.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        /// <param name="adapterCreator">The adapter creator function.</param>
#else
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="BootstrapperContainerBase{TIocContainerAdapter, TIocContainer}"/> class.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        /// <param name="adapterCreator">The adapter creator function.</param>
#endif
        public
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            (
            TIocContainer iocContainer, 
            Func<TIocContainer, TIocContainerAdapter> adapterCreator) : 
            this(iocContainer, adapterCreator, new BootstrapperCreationOptions())
        {           
        }

#if TEST
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="TestBootstrapperContainerBase{TIocContainerAdapter, TIocContainer}"/> class.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        /// <param name="adapterCreator">The adapter creator function.</param>
        /// <param name="creationOptions">The bootstrapper creation options.</param>
#else
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="BootstrapperContainerBase{TIocContainerAdapter, TIocContainer}"/> class.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        /// <param name="adapterCreator">The adapter creator function.</param>
        /// <param name="creationOptions">The bootstrapper creation options.</param>
#endif
        public
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            (
            TIocContainer iocContainer,
            Func<TIocContainer, TIocContainerAdapter> adapterCreator,
            BootstrapperCreationOptions creationOptions) : base(adapterCreator(iocContainer), 
                creationOptions)
        {
            Container = iocContainer;
            if (creationOptions.UseDefaultMiddlewares)
            {
                Use(new RegisterCompositionModulesMiddleware<TIocContainerAdapter, TIocContainer>());
            }
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public TIocContainer Container { get; }

        /// <summary>
        /// Override this method to inject custom logic during bootstrapper configuration.
        /// </summary>
        /// <param name="iocContainerAdapter">IoC container adapter</param>
        protected override void OnConfigure(TIocContainerAdapter iocContainerAdapter)
        {
            base.OnConfigure(iocContainerAdapter);            
            MiddlewareApplier.ApplyMiddlewares(this, _middlewares);
        }
    }

    /// <summary>
    /// Application bootstrapper with ioc container adapter. 
    /// </summary>    
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    public partial class
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
        <TIocContainerAdapter> 
        :
#if TEST
    TestBootstrapperBase
#else
    BootstrapperBase
#endif
        , IBootstrapperWithContainerAdapter<TIocContainerAdapter>
#if TEST
        , Solid.Bootstrapping.IHaveContainerResolver
#endif                    
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter
    {
        /// <summary>
        /// Application bootstrapper with ioc container adapter and root object.
        /// </summary>
        /// <typeparam name="TRootObject"></typeparam>
        public class WithRootObject<TRootObject> :
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            <TIocContainerAdapter>
        {
#if TEST
            /// <summary>
            /// Initializes a new instance of <see cref="TestBootstrapperContainerBase{TIocContainerAdapter}.WithRootObject{TRootObject}"/>
            /// </summary>
            /// <param name="iocContainerAdapter">The ioc container adapter</param>
#else
            /// <summary>
            /// Initializes a new instance of <see cref="BootstrapperContainerBase{TIocContainerAdapter}.WithRootObject{TRootObject}"/>
            /// </summary>
            /// <param name="iocContainerAdapter">The ioc container adapter.</param>
#endif
            public WithRootObject(TIocContainerAdapter iocContainerAdapter)
                : this(iocContainerAdapter, new BootstrapperCreationOptions
                {
                    ExcludedTypes = new List<Type> {typeof (TRootObject)}
                })
            {
            }
#if TEST
            /// <summary>
            /// Initializes a new instance of the <see cref="TestBootstrapperContainerBase{TIocContainerAdapter}.WithRootObject{TRootObject}"/> class.
            /// </summary>
            /// <param name="iocContainerAdapter">The ioc container adapter.</param>
            /// <param name="creationOptions">The creation options.</param>
#else
            /// <summary>
            /// Initializes a new instance of the <see cref="BootstrapperContainerBase{TIocContainerAdapter}.WithRootObject{TRootObject}"/> class.
            /// </summary>
            /// <param name="iocContainerAdapter">The ioc container adapter.</param>
            /// <param name="creationOptions">The creation options.</param>
#endif
            public WithRootObject(TIocContainerAdapter iocContainerAdapter, 
                BootstrapperCreationOptions creationOptions) :
                base(iocContainerAdapter, AddRootObject(creationOptions))
            {
                Use(new CreateRootObjectMiddleware<TIocContainerAdapter>(typeof (TRootObject),
                    creationOptions.DisplayRootView));
            }

            private static BootstrapperCreationOptions AddRootObject(BootstrapperCreationOptions creationOptions)
            {
                if (creationOptions.ExcludedTypes == null)
                {
                    creationOptions.ExcludedTypes = new List<Type>();
                }
                if (creationOptions.ExcludedTypes.Contains(typeof (TRootObject)) == false)
                {
                    creationOptions.ExcludedTypes.Add(typeof(TRootObject));
                }
                return creationOptions;
            }
        }

#if TEST
        /// <summary>
        /// Initializes a new instance of the <see cref="TestBootstrapperContainerBase{TIocContainerAdapter}"/> class.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperContainerBase{TIocContainerAdapter}"/> class.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>
#endif
        public
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            (
            TIocContainerAdapter iocContainerAdapter)
            :this(iocContainerAdapter, new BootstrapperCreationOptions())            
        {            
        }

#if TEST
        /// <summary>
        /// Initializes a new instance of the <see cref="TestBootstrapperContainerBase{TIocContainerAdapter}"/> class.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>
        /// <param name="creationOptions">The creation options.</param>
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperContainerBase{TIocContainerAdapter}"/> class.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>
        /// <param name="creationOptions">The creation options.</param>
#endif
        public
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            (
            TIocContainerAdapter iocContainerAdapter,
            BootstrapperCreationOptions creationOptions)
#if NET45 && !TEST
            :base(creationOptions)
#endif
        {            
            ContainerAdapter = iocContainerAdapter;
            if (creationOptions.UseDefaultMiddlewares)
            {
                Use(new RegisterViewModelsMiddleware<TIocContainerAdapter>(creationOptions.ExcludedTypes))
                    .Use(new RegisterCompositionModulesMiddleware<TIocContainerAdapter>())
                    .Use(new RegisterPlatformSpecificMiddleware<TIocContainerAdapter>());
            }           
        }        

        /// <summary>
        /// Gets the container adapter.
        /// </summary>
        /// <value>
        /// The container adapter.
        /// </value>
        internal TIocContainerAdapter ContainerAdapter { get; }

#if NET45 && !TEST
        /// <summary>
        /// Override this to add custom behavior to execute after the application starts.
        /// </summary>
        /// <param name="sender">The sender.</param><param name="e">The args.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);            
            RaiseInitializationCompleted();            
        }
#endif
#if (NETFX_CORE || WINDOWS_UWP || WIN81) && !TEST

        /// <summary>
        /// Override this method to inject custom functionality before the app is launched.
        /// </summary>
        /// <param name="e">The <see cref="LaunchActivatedEventArgs"/> instance containing the event data.</param>
        protected virtual void BeforeOnLaunched(LaunchActivatedEventArgs e)
        {
            
        }

        /// <summary>
        /// Override this method to inject custom launch activation arguments checking logic.
        /// </summary>
        /// <param name="e">The <see cref="LaunchActivatedEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        protected virtual bool CheckLaunchActivationArguments(LaunchActivatedEventArgs e)
        {
            return e.PreviousExecutionState != ApplicationExecutionState.Running;
        }
        
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var canLaunch = CheckLaunchActivationArguments(e);
            if (canLaunch == false)
            {
                return;
            }                            
            OnLaunchedCore();
            BeforeOnLaunched(e);
            RaiseInitializationCompleted();                   
        }

        private void OnLaunchedCore()
        {
            InitializeDispatcher();                   
        }
#endif

#if !TEST
        internal void DisplayRootViewForInternal(Type rootObjectType)
        {
            DisplayRootViewFor(rootObjectType);
        }
#endif

#if (NETFX_CORE || WINDOWS_UWP || WIN81) && !TEST
        ///<summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>

        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        protected override void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
#endif

        /// <summary>
        /// Configures the framework and executes boiler-plate registrations.
        /// </summary>
        protected sealed override void Configure()
        {
            base.Configure();                                                                   
            InitializeAdapter(ContainerAdapter);
#if NET45 // in UWP the dispatcher is initialized later.
            InitializeDispatcher();
#endif
            MiddlewareApplier.ApplyMiddlewares(this, _middlewares);
            MiddlewareApplier.ApplyMiddlewares(this, _concreteMiddlewares);
            OnConfigure(ContainerAdapter);
        }
                
        /// <summary>
        /// Override this method to inject custom logic during bootstrapper configuration.
        /// </summary>
        /// <param name="iocContainerAdapter">IoC container adapter</param>
        protected virtual void OnConfigure(TIocContainerAdapter iocContainerAdapter)
        {
        }        

        /// <summary>
        /// Initializes the framework dispatcher.
        /// </summary>
        static void InitializeDispatcher()
        {
            Dispatch.Current.InitializeDispatch();
        }

        /// <summary>
        /// Gets the registrator.
        /// </summary>
        /// <value>
        /// The registrator.
        /// </value>
        public IIocContainerRegistrator Registrator => ContainerAdapter;

#if TEST
        /// <summary>
        /// Gets the resolver.
        /// </summary>
        /// <value>
        /// The resolver.
        /// </value>
        public IIocContainerResolver Resolver => ContainerAdapter;
#endif
    }    
}
