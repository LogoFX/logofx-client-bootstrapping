using System;
using System.Reflection;
using System.Windows.Threading;
#if NET45
using System.Windows;
#endif
#if NETFX_CORE || WINDOWS_UWP
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
#endif
using Caliburn.Micro;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.Composition;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <typeparam name="TIocContainer">The type of the ioc container.</typeparam>
    /// <seealso cref="Bootstrapping.BootstrapperContainerBase{TRootViewModel, TIocContainerAdapter}" />
    public class BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer> :
                    BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter<TIocContainer>, IBootstrapperAdapter, new() 
        where TIocContainer : class
    {
        private readonly TIocContainer _iocContainer;        

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="BootstrapperContainerBase{TRootViewModel, TIocContainerAdapter, TIocContainer}"/> class.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        /// <param name="adapterCreator">The adapter creator function.</param>
        public BootstrapperContainerBase(
            TIocContainer iocContainer, 
            Func<TIocContainer, TIocContainerAdapter> adapterCreator) : 
            this(iocContainer, adapterCreator, new BootstrapperCreationOptions
            {
               InitializeOnCreation = false
            })
        {
            _iocContainer = iocContainer;
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="BootstrapperContainerBase{TRootViewModel, TIocContainerAdapter, TIocContainer}"/> class.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        /// /// <param name="adapterCreator">The adapter creator function.</param>
        /// <param name="creationOptions">The bootstrapper creation options.</param>
        public BootstrapperContainerBase(
            TIocContainer iocContainer,
            Func<TIocContainer, TIocContainerAdapter> adapterCreator,
            BootstrapperCreationOptions creationOptions) : base(adapterCreator(iocContainer), 
                ForceInitializeOnCreationFalseValue(creationOptions))
        {
            _iocContainer = iocContainer;
        }

        private static BootstrapperCreationOptions ForceInitializeOnCreationFalseValue(BootstrapperCreationOptions creationOptions)
        {
            creationOptions.InitializeOnCreation = false;
            return creationOptions;
        }

        /// <summary>
        /// Override this method to inject custom logic during bootstrapper configuration.
        /// </summary>
        /// <param name="iocContainerAdapter">IoC container adapter</param>
        protected override void OnConfigure(TIocContainerAdapter iocContainerAdapter)
        {
            base.OnConfigure(iocContainerAdapter);
            ModuleRegistrationHelper.RegisterCompositionModules(_iocContainer, Modules, () => CurrentLifetimeScope);
        }
    }

    /// <summary>
    /// Base class for application and test boostrappers.
    /// Used when no navigation logic is needed.
    /// </summary>
    /// <typeparam name="TRootViewModel">Type of Root ViewModel.</typeparam>
    /// <typeparam name="TIocContainerAdapter">Type of IoC container adapter.</typeparam>
    public partial class BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> :
#if NET45
        BootstrapperBase
#endif
#if WINDOWS_UWP || NETFX_CORE
        CaliburnApplication
#endif               
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {        
        private readonly TIocContainerAdapter _iocContainerAdapter;
        private readonly BootstrapperCreationOptions _creationOptions;
        private bool _isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperContainerBase{TRootViewModel, TIocContainerAdapter}"/> class.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>        
        public BootstrapperContainerBase(
            TIocContainerAdapter iocContainerAdapter)
            :this(iocContainerAdapter, new BootstrapperCreationOptions())            
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperContainerBase{TRootViewModel, TIocContainerAdapter}"/> class.
        /// </summary>
        /// <param name="iocContainerAdapter">The ioc container adapter.</param>
        /// <param name="creationOptions">The creation options.</param>
        public BootstrapperContainerBase(
            TIocContainerAdapter iocContainerAdapter,
            BootstrapperCreationOptions creationOptions)
#if NET45
            :base(creationOptions.UseApplication)
#endif
        {
            _iocContainerAdapter = iocContainerAdapter;
            _reuseCompositionInformation = creationOptions.ReuseCompositionInformation;
            _creationOptions = creationOptions;
            if (creationOptions.InitializeOnCreation)
            {
                Initialize();
            }                              
        }

        /// <summary>
        /// Initializes the framework.
        /// </summary>
        public new void Initialize()
        {
            if (_isInitialized)
                return;
            _isInitialized = true;
            InitializeInternal();
        }

        private void InitializeInternal()
        {            
            if (_creationOptions.DiscoverCompositionModules || _creationOptions.InspectAssemblies)
            {
                Solid.Practices.Composition.PlatformProvider.Current =
#if NET45
                    new NetPlatformProvider()
#endif
#if NETFX_CORE || WINDOWS_UWP
                    new UniversalPlatformProvider()
#endif
                    ;
            }
            if (_creationOptions.DiscoverCompositionModules)
            {
                InitializeCompositionModules();
            }
            if (_creationOptions.InspectAssemblies)
            {
                InitializeInspectedAssemblies();
            }
            base.Initialize();
        }

#if NET45
        /// <summary>
        /// Override this to add custom behavior to execute after the application starts.
        /// </summary>
        /// <param name="sender">The sender.</param><param name="e">The args.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            DisplayRootView();
        }
#endif
#if NETFX_CORE || WINDOWS_UWP

        /// <summary>
        /// Override this method to inject custom functionality before the app is launched.
        /// </summary>
        /// <param name="e">The <see cref="LaunchActivatedEventArgs"/> instance containing the event data.</param>
        protected virtual void BeforeOnLaunched(LaunchActivatedEventArgs e)
        {
            
        }

        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            BeforeOnLaunched(e);          
            DisplayRootView();
        }
#endif

        private void DisplayRootView()
        {
            DisplayRootViewFor<TRootViewModel>();
        }

#if NETFX_CORE || WINDOWS_UWP
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
            BootstrapperHelper<TRootViewModel, TIocContainerAdapter>.RegisterCore(_iocContainerAdapter);            
            BootstrapperHelper<TRootViewModel, TIocContainerAdapter>.RegisterViewsAndViewModels(_iocContainerAdapter,
                Assemblies);
            ModuleRegistrationHelper.RegisterCompositionModules(_iocContainerAdapter,
                Modules);
            InitializeViewLocator();
            InitializeAdapter();      
            InitializeDispatcher();
            RegisterPlatformSpecificServices(_iocContainerAdapter);                        
            OnConfigure(_iocContainerAdapter);
        }
        
        private static void RegisterPlatformSpecificServices(TIocContainerAdapter iocContainerAdapter)
        {
#if NET45
                  iocContainerAdapter.RegisterSingleton<IWindowManager, WindowManager>();
#endif
        }

        /// <summary>
        /// Override this method to inject custom logic during bootstrapper configuration.
        /// </summary>
        /// <param name="iocContainerAdapter">IoC container adapter</param>
        protected virtual void OnConfigure(TIocContainerAdapter iocContainerAdapter)
        {
        }

        private readonly object _defaultLifetimeScope = new object();

        /// <summary>
        /// Gets the current lifetime scope.
        /// </summary>
        /// <value>
        /// The current lifetime scope.
        /// </value>
        public virtual object CurrentLifetimeScope
        {
            get { return _defaultLifetimeScope; }
        }
        
        /// <summary>
        /// Gets the assemblies that will be inspected for the application components.
        /// </summary>
        /// <value>
        /// The assemblies.
        /// </value>
        protected Assembly[] Assemblies { get; private set; }

        /// <summary>
        /// Initializes the framework dispatcher.
        /// </summary>
        public static void InitializeDispatcher()
        {
            Dispatch.Current.InitializeDispatch();
        }
    }    
}
