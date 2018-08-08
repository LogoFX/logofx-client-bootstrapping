using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping.Xamarin.Forms
{
    /// <summary>
    /// Used to bridge between the setup of the Xamarin.Forms app
    /// and the containing platform application.
    /// </summary>
    /// <typeparam name="TApp">The type of the Xamarin.Forms app.</typeparam>
    /// <typeparam name="TBootstrapper">The type of the bootstrapper.</typeparam>
    public class Bridge<TApp, TBootstrapper>
        where TApp : class
        where TBootstrapper : BootstrapperBase
    {
        /// <summary>
        /// Initializes and registers the root app components.
        /// </summary>
        /// <typeparam name="TContainerAdapter">The type of the ioc container adapter.</typeparam>
        /// <param name="containerAdapter">The ioc container adapter.</param>
        public static void Initialize<TContainerAdapter>(TContainerAdapter containerAdapter)
            where TContainerAdapter : IDependencyRegistrator, IDependencyResolver, IBootstrapperAdapter
        {
            ContainerContext.SetAdapter(containerAdapter);
            ContainerContext.Registrator
                .AddInstance(ContainerContext.Registrator)
                .AddInstance(ContainerContext.Resolver)
                .AddTransient<TApp>()
                .AddTransient<TBootstrapper>();           
        }
    }
}
