#if !TEST && (WINDOWS_UWP || NETFX_CORE)
using Caliburn.Micro;
#endif
using System;
using LogoFX.Bootstrapping;
using Solid.Bootstrapping;
using Solid.Practices.Composition;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Base bootstrapper, responsible for modules composition.
    /// </summary>    
    /// <seealso cref="Solid.Practices.Composition.Contracts.ICompositionModulesProvider" />
#if TEST
    public abstract partial class TestBootstrapperBase
#else
    public abstract partial class BootstrapperBase
#endif
        :
#if TEST
        IntegrationTestBootstrapperBase
#else
#if NET
        Caliburn.Micro.BootstrapperBase
#else
#if WINDOWS_UWP || NETFX_CORE
        CaliburnApplication
#else
#endif
#endif
#endif
        , IBootstrapper
    {
        private readonly BootstrapperCreationOptions _creationOptions;

#if TEST
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="TestBootstrapperBase"/> 
        /// </summary>
#else
            /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="BootstrapperBase"/> 
        /// </summary>
#endif        
        protected
#if TEST
            TestBootstrapperBase
#else
            BootstrapperBase
#endif

            ()
            :this(new BootstrapperCreationOptions())
        {
            
        }
#if TEST
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="TestBootstrapperBase"/> 
        /// </summary>
        /// <param name="creationOptions">The creation options.</param>
#else
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="BootstrapperBase"/> 
        /// </summary>
        /// <param name="creationOptions">The creation options.</param>
#endif        
        protected
#if TEST
            TestBootstrapperBase
#else
            BootstrapperBase
#endif

            (BootstrapperCreationOptions creationOptions)
#if NET && !TEST
            :base(creationOptions.UseApplication)
#endif
        {
            _reuseCompositionInformation = creationOptions.ReuseCompositionInformation;
            _creationOptions = creationOptions;
            if (creationOptions.DiscoverCompositionModules || creationOptions.InspectAssemblies)
            {
                Solid.Practices.Composition.PlatformProvider.Current = new
#if WINDOWS_UWP
                    UniversalPlatformProvider
#else
                    NetStandardPlatformProvider
#endif
                    ();
            }   
            _middlewaresWrapper = new MiddlewaresWrapper<IBootstrapper>(this);
            if (creationOptions.UseDefaultMiddlewares)
            {
                Use(new InitializeViewLocatorMiddleware());
            }
        }        

        /// <summary>
        /// Override to configure the framework and setup your IoC container.
        /// </summary>
        protected override void Configure()
        {
            base.Configure();
            if (_creationOptions.DiscoverCompositionModules)
            {
                InitializeCompositionModules();
            }
            MiddlewareApplier.ApplyMiddlewares(this, _middlewaresWrapper.Middlewares);
        }

        void IInitializable.Initialize()
        {
            Initialize();
        }

        /// <summary>
        /// Occurs when initialization is completed and the application starts.
        /// </summary>
        public event EventHandler InitializationCompleted;

        /// <summary>
        /// Raises the initialization completed.
        /// </summary>
        protected internal void RaiseInitializationCompleted()
        {
            InitializationCompleted?.Invoke(this, new EventArgs());
        }
    }    
}