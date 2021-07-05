using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Registers the ioc container adapter.
    /// </summary>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    class RegisterContainerAdapterMiddleware<TIocContainerAdapter> :
        IMiddleware<
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
                <TIocContainerAdapter>>
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
                <TIocContainerAdapter>
            Apply(
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
                <TIocContainerAdapter>
            @object)
        {
            @object.Registrator.RegisterInstance(@object.ContainerAdapter);
            return @object;
        }
    }    
}