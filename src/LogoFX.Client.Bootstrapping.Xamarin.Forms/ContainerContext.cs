using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping.Xamarin.Forms
{
    public static class ContainerContext
    {
        private static object _instance;

        public static void SetAdapter<TContainerAdapter>(TContainerAdapter containerAdapter)
            where TContainerAdapter : IDependencyRegistrator, IDependencyResolver, IBootstrapperAdapter => _instance = containerAdapter;

        public static IDependencyRegistrator Registrator => (IDependencyRegistrator)_instance;

        public static IDependencyResolver Resolver => (IDependencyResolver)_instance;

        public static IBootstrapperAdapter Adapter => (IBootstrapperAdapter)_instance;
    }
     
}
