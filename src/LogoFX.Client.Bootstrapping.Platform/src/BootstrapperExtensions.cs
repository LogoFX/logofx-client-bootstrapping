using System;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Bootstrapper extension methods.
    /// </summary>
    public static class BootstrapperExtensions
    {
        /// <summary>
        /// Uses the root object creation middleware.
        /// </summary>
        /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <param name="rootObjectType">The type of the root object.</param>
        /// <param name="displayView">if set to <c>true</c> view is displayed.</param>
        public static
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            <TIocContainerAdapter> 
            UseRootObject<TIocContainerAdapter>(this
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            <TIocContainerAdapter> bootstrapper,
            Type rootObjectType, bool displayView) 
            where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter
        {
            bootstrapper.Use(new CreateRootObjectMiddleware<TIocContainerAdapter>(rootObjectType, displayView));
            return bootstrapper;
        }

        /// <summary>
        /// Uses the resolver registration middleware.
        /// </summary>
        /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
        /// <param name="bootstrapper">The bootstrapper.</param>        
        public static
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            <TIocContainerAdapter>
            UseResolver<TIocContainerAdapter>(this
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            <TIocContainerAdapter> bootstrapper)
            where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter
        {
            bootstrapper.Use(new RegisterResolverMiddleware<TIocContainerAdapter>());
            return bootstrapper;            
        }
    }
}
