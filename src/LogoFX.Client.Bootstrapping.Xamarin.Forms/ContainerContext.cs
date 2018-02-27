using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping.Xamarin.Forms
{
    /// <summary>
    /// An ambient context for the ioc container adapter.
    /// Used as bridge for resolving the Xamarin.Forms app.
    /// </summary>
    public static class ContainerContext
    {
        private static object _instance;

        /// <summary>
        /// Sets the value of the ioc container adapter.
        /// </summary>
        /// <typeparam name="TContainerAdapter">The type of the ioc container adapter.</typeparam>
        /// <param name="containerAdapter">The ioc container adapter.</param>
        public static void SetAdapter<TContainerAdapter>(TContainerAdapter containerAdapter)
            where TContainerAdapter : IDependencyRegistrator, IDependencyResolver, IBootstrapperAdapter 
            => _instance = containerAdapter;

        /// <summary>
        /// Gets the dependency regsitrator.
        /// </summary>
        public static IDependencyRegistrator Registrator => (IDependencyRegistrator)_instance;

        /// <summary>
        /// Gets the dependency resolver.
        /// </summary>
        public static IDependencyResolver Resolver => (IDependencyResolver)_instance;

        /// <summary>
        /// Gets the bootstrapper adapter.
        /// </summary>
        public static IBootstrapperAdapter Adapter => (IBootstrapperAdapter)_instance;
    }
     
}
