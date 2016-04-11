using System;
using System.Windows.Threading;
#if NET45
using System.Windows;
#endif
#if NETFX_CORE || WINDOWS_UWP || WIN81
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
#endif
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Application bootstrapper. Used when there is need in IoC-container-specific logic.
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <typeparam name="TIocContainer">The type of the ioc container.</typeparam>
    /// <seealso cref="Bootstrapping.BootstrapperContainerBase{TRootViewModel, TIocContainerAdapter}" />
    public partial class BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer> :
                    BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>,
                    IBootstrapperWithContainer<TRootViewModel, TIocContainerAdapter, TIocContainer>
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter<TIocContainer>, IBootstrapperAdapter, new() 
        where TIocContainer : class
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="BootstrapperContainerBase{TRootViewModel, TIocContainerAdapter, TIocContainer}"/> class.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        /// <param name="adapterCreator">The adapter creator function.</param>
        public BootstrapperContainerBase(
            TIocContainer iocContainer, 
            Func<TIocContainer, TIocContainerAdapter> adapterCreator) : 
            this(iocContainer, adapterCreator, new BootstrapperCreationOptions())
        {           
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
                creationOptions)
        {
            Container = iocContainer;                     
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
    /// Application bootstrapper. Used when no IoC-container-specific logic is needed.    
    /// </summary>
    /// <typeparam name="TRootViewModel">Type of Root ViewModel.</typeparam>
    /// <typeparam name="TIocContainerAdapter">Type of IoC container adapter.</typeparam>
    public partial class BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> : 
        BootstrapperBase, IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter>               
        where TRootViewModel : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {

        private readonly BootstrapperCreationOptions _creationOptions;

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
            :base(creationOptions)
#endif
        {
            _creationOptions = creationOptions;
            ContainerAdapter = iocContainerAdapter;
            if (creationOptions.UseDefaultMiddlewares)
            {
                Use(new RegisterCoreMiddleware<TRootViewModel, TIocContainerAdapter>())
               .Use(new RegisterViewModelsMiddleware<TRootViewModel, TIocContainerAdapter>())
               .Use(new RegisterCompositionModulesMiddleware<TRootViewModel, TIocContainerAdapter>())
               .Use(new RegisterPlatformSpecificMiddleware<TRootViewModel, TIocContainerAdapter>());
            }           
        }

        /// <summary>
        /// Gets the container adapter.
        /// </summary>
        /// <value>
        /// The container adapter.
        /// </value>
        public TIocContainerAdapter ContainerAdapter { get; }

#if NET45
        /// <summary>
        /// Override this to add custom behavior to execute after the application starts.
        /// </summary>
        /// <param name="sender">The sender.</param><param name="e">The args.</param>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);
            if (_creationOptions.DisplayRootView)
            {
                DisplayRootView();
            }    
            else
            {
                CreateRootViewModel();
            }    
        }
#endif
#if NETFX_CORE || WINDOWS_UWP || WIN81

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
            OnLaunchedCore();
            BeforeOnLaunched(e);
            if (_creationOptions.DisplayRootView)
            {
                DisplayRootView();
            }
            else
            {
                CreateRootViewModel();
            }     
        }

        private void OnLaunchedCore()
        {
            InitializeDispatcher();
        }
#endif

        private void DisplayRootView()
        {
            DisplayRootViewFor<TRootViewModel>();
        }

        private void CreateRootViewModel()
        {
            //this mimics the Caliburn.Micro IoC.GetInstance 
            //which is executed inside the DisplayRootViewFor
            ContainerAdapter.Resolve<TRootViewModel>();
        }

#if NETFX_CORE || WINDOWS_UWP || WIN81
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
        /// Gets the current lifetime scope.
        /// </summary>
        /// <value>
        /// The current lifetime scope.
        /// </value>
        public virtual object CurrentLifetimeScope { get; } = new object();

        /// <summary>
        /// Initializes the framework dispatcher.
        /// </summary>
        static void InitializeDispatcher()
        {
            Dispatch.Current.InitializeDispatch();
        }
    }    
}
