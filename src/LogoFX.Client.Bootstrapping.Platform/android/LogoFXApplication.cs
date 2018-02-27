using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Android.Runtime;
using Caliburn.Micro;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using LogoFX.Client.Bootstrapping.Xamarin.Forms;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Holds the responsibility for proper platform and
    /// Xamarin.Forms initialization. 
    /// </summary>
    /// <typeparam name="TApp">The type of the Xamarin.Forms app to be launched.</typeparam>
    /// <typeparam name="TBootstrapper">The type of the bootstrapper to be invoked.</typeparam>
    /// <typeparam name="TContainerAdapter">The type of the inversion-of-control container adapter to be used.</typeparam>
    public class LogoFXApplication<TApp, TBootstrapper, TContainerAdapter> : CaliburnApplication
        where TApp : class
        where TBootstrapper : BootstrapperBase
        where TContainerAdapter : IDependencyRegistrator, IDependencyResolver, IBootstrapperAdapter, new()
    {
        /// <summary>
        /// <inheritdoc />
        /// </summary>        
        public LogoFXApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {

        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>  
        public override void OnCreate()
        {
            base.OnCreate();
            Initialize();
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>  
        protected override void Configure()
        {
            Bridge<TApp, TBootstrapper>.Initialize(new TContainerAdapter());
            Dispatch.Current = new PlatformDispatch();
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>  
        protected override void BuildUp(object instance)
        {
            ContainerContext.Adapter.BuildUp(instance);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>  
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return ContainerContext.Adapter.GetAllInstances(service);
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>  
        protected override object GetInstance(Type service, string key)
        {
            return ContainerContext.Adapter.GetInstance(service, key);
        }
    }
}