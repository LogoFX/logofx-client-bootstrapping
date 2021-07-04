using System;
using System.Collections.Generic;
using System.Threading;
using LogoFX.Bootstrapping;
#if (NET || NETCORE || NETFRAMEWORK) && !TEST
using System.Windows;
#endif
#if !TEST && (NETFX_CORE || WINDOWS_UWP)
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
#endif
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Bootstrapping;
using Solid.Extensibility;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{    
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
        , IExtensible<
#if TEST
    TestBootstrapperContainerBase
#else
                BootstrapperContainerBase
#endif
                <TIocContainerAdapter>
            >
        , IBootstrapperWithContainerAdapter<TIocContainerAdapter>
#if TEST
        , IHaveResolver
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
                Use(new CreateRootObjectMiddleware<TIocContainerAdapter>(                    
                    typeof(TRootObject),
                    creationOptions.DisplayRootView,
                    true));
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

        /// <summary>
        /// Application bootstrapper with ioc container adapter and root object.
        /// </summary>        
        /// <typeparam name="TRootObject"></typeparam>
        public class WithRootObjectAsContract<TRootObject> :
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
            public WithRootObjectAsContract(TIocContainerAdapter iocContainerAdapter)
                : this(iocContainerAdapter, new BootstrapperCreationOptions
                {
                    ExcludedTypes = new List<Type> {                         
                        typeof(TRootObject) },
                    RegisterRoot = false
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
            public WithRootObjectAsContract(TIocContainerAdapter iocContainerAdapter,
                BootstrapperCreationOptions creationOptions) :
                base(iocContainerAdapter, AddRootObjectAsContract(creationOptions))
            {
                Use(new CreateRootObjectMiddleware<TIocContainerAdapter>(
                    typeof(TRootObject),
                    creationOptions.DisplayRootView,
                    creationOptions.RegisterRoot));
            }            

            private static BootstrapperCreationOptions AddRootObjectAsContract(BootstrapperCreationOptions creationOptions)
            {
                if (creationOptions.ExcludedTypes == null)
                {
                    creationOptions.ExcludedTypes = new List<Type>();
                }
                if (creationOptions.ExcludedTypes.Contains(typeof(TRootObject)) == false)
                {
                    creationOptions.ExcludedTypes.Add(typeof(TRootObject));
                }
                creationOptions.RegisterRoot = false;
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
#if (NET || NETCORE || NETFRAMEWORK) && !TEST
            : base(creationOptions)
#endif
        {            
            ContainerAdapter = iocContainerAdapter;
            _concreteMiddlewaresWrapper = new MiddlewaresWrapper<
#if TEST
    TestBootstrapperContainerBase
#else
                BootstrapperContainerBase
#endif
                <TIocContainerAdapter>>(this);
            _registratorMiddlewaresWrapper = new MiddlewaresWrapper<IBootstrapperWithRegistrator>(this);
            _middlewaresWrapper = new MiddlewaresWrapper<IBootstrapperWithContainerAdapter<TIocContainerAdapter>>(this);
            if (creationOptions.UseDefaultMiddlewares)
            {
                Use(new RegisterCompositionModulesMiddleware<
#if TEST
    TestBootstrapperContainerBase
#else
                            BootstrapperContainerBase
#endif
                            <TIocContainerAdapter>>
                    ())                    
                    .UseBootstrapperComposition()                    
                    .Use(new RegisterPlatformSpecificMiddleware());
                if (creationOptions.RegisterViewModels)
                {
                    Use(new RegisterViewModelsMiddleware(creationOptions.ExcludedTypes));
                }
                if (creationOptions.RegisterViewModelsAsContracts)
                {
                    Use(new RegisterViewModelsAsContractsMiddleware(creationOptions.ExcludedTypes));
                }
            }                      
        }        

        /// <summary>
        /// Gets the container adapter.
        /// </summary>
        /// <value>
        /// The container adapter.
        /// </value>
        internal TIocContainerAdapter ContainerAdapter { get; }

#if (NET || NETCORE || NETFRAMEWORK) && !TEST
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
#if (NETFX_CORE || WINDOWS_UWP) && !TEST

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

#if (NETFX_CORE || WINDOWS_UWP) && !TEST
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
#if (NET || NETCORE || NETFRAMEWORK) // in UWP the dispatcher is initialized later.
            InitializeDispatcher();
#endif
            MiddlewareApplier.ApplyMiddlewares(this, _registratorMiddlewaresWrapper.Middlewares);
            MiddlewareApplier.ApplyMiddlewares(this, _middlewaresWrapper.Middlewares);
            MiddlewareApplier.ApplyMiddlewares(this, _concreteMiddlewaresWrapper.Middlewares);
            OnConfigure(ContainerAdapter);
        }
                
        /// <summary>
        /// Override this method to inject custom logic during bootstrapper configuration.
        /// </summary>
        /// <param name="dependencyRegistrator">The dependency registrator.</param>
        protected virtual void OnConfigure(IDependencyRegistrator dependencyRegistrator)
        {
        }        

        /// <summary>
        /// Initializes the framework dispatcher.
        /// </summary>
        static void InitializeDispatcher()
        {
            Dispatch.Current = new PlatformDispatch();
            Dispatch.Current.InitializeDispatch();
        }

        /// <summary>
        /// Gets the registrator.
        /// </summary>
        /// <value>
        /// The registrator.
        /// </value>
        public IDependencyRegistrator Registrator => ContainerAdapter;

#if TEST
        /// <summary>
        /// Gets the resolver.
        /// </summary>
        /// <value>
        /// The resolver.
        /// </value>
        public IDependencyResolver Resolver => ContainerAdapter;
#endif
    }    
}