#if WINDOWS_UWP || NETFX_CORE || WIN81
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
    public abstract partial class BootstrapperBase :
#if TEST
        IntegrationTestBootstrapperBase
#else
#if NET45
        Caliburn.Micro.BootstrapperBase
#else
#if WINDOWS_UWP || NETFX_CORE || WIN81
        CaliburnApplication
#else
#endif
#endif
#endif
        , IBootstrapper
    {
        private readonly BootstrapperCreationOptions _creationOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperBase"/> class.
        /// </summary>
        protected BootstrapperBase()
            :this(new BootstrapperCreationOptions())
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperBase"/> class.
        /// </summary>
        /// <param name="creationOptions">The creation options.</param>
        protected BootstrapperBase(BootstrapperCreationOptions creationOptions)
#if NET45 && !TEST
            :base(creationOptions.UseApplication)
#endif
        {
            _reuseCompositionInformation = creationOptions.ReuseCompositionInformation;
            _creationOptions = creationOptions;
            if (creationOptions.DiscoverCompositionModules || creationOptions.InspectAssemblies)
            {
                Solid.Practices.Composition.PlatformProvider.Current =
#if NET45
                    new NetPlatformProvider()
#endif
#if NETFX_CORE || WINDOWS_UWP
                    new UniversalPlatformProvider()
#endif
#if WIN81
                    new WinRTPlatformProvider()
#endif
                    ;
            }
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
            MiddlewareApplier.ApplyMiddlewares(this, _middlewares);
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