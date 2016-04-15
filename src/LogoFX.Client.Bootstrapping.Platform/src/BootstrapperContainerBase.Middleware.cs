using System.Collections.Generic;
using LogoFX.Bootstrapping;
#if NET45
using Caliburn.Micro;
#endif
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    public partial class BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer>
    {
        private readonly
            List<IMiddleware<IBootstrapperWithContainer<TRootViewModel, TIocContainerAdapter, TIocContainer>>>
            _middlewares =
                new List<IMiddleware<IBootstrapperWithContainer<TRootViewModel, TIocContainerAdapter, TIocContainer>>>();

        /// <summary>
        /// Uses the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public IBootstrapperWithContainer<TRootViewModel, TIocContainerAdapter, TIocContainer> Use(
            IMiddleware<IBootstrapperWithContainer<TRootViewModel, TIocContainerAdapter, TIocContainer>> middleware)
        {
            _middlewares.Add(middleware);
            return this;
        }
    }

    public partial class BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>
    {
        private readonly List<IMiddleware<IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter>>> _middlewares =
            new List<IMiddleware<IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter>>>();

        /// <summary>
        /// Uses the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter> Use(
            IMiddleware<IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter>> middleware)
        {
            _middlewares.Add(middleware);
            return this;
        }
    }                

    /// <summary>
    /// Registers platform-specific services into the ioc container.
    /// </summary>
    /// <typeparam name="TRootObject">The type of the root object.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterPlatformSpecificMiddleware<TRootObject, TIocContainerAdapter> :
        IMiddleware<IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter>>
        where TRootObject : class
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter, new()
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> Apply(
            IBootstrapperWithContainerAdapter<TRootObject, TIocContainerAdapter> @object)
        {
#if NET45
            @object.ContainerAdapter.RegisterSingleton<IWindowManager, WindowManager>();
#endif
            return @object;
        }
    }
}
