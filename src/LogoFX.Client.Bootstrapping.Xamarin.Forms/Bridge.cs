using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping.Xamarin.Forms
{
    public class Bridge<TApp, TBootstrapper>
        where TApp : class
        where TBootstrapper : BootstrapperBase
    {
        public static void Initialize<TContainerAdapter>(TContainerAdapter containerAdapter)
            where TContainerAdapter : IDependencyRegistrator, IDependencyResolver, IBootstrapperAdapter
        {
            ContainerContext.SetAdapter(containerAdapter);
            ContainerContext.Registrator.RegisterInstance(ContainerContext.Registrator);
            ContainerContext.Registrator.RegisterInstance(ContainerContext.Resolver);
            ContainerContext.Registrator.RegisterSingleton<TApp>();
            ContainerContext.Registrator.RegisterSingleton<TBootstrapper>();
            ContainerContext.Registrator.RegisterSingleton<BootstrapperBase, TBootstrapper>();
        }
    }
}
