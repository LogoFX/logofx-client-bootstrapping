using System.Collections.Generic;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    public partial class BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer>
    {
        private readonly
            List<IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer>>>
            _middlewares =
                new List<IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer>>>();

        /// <summary>
        /// Uses the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer> Use(
            IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter, TIocContainer>> middleware)
        {
            _middlewares.Add(middleware);
            return this;
        }
    }

    public partial class BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>
    {
        private readonly List<IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>>> _middlewares =
            new List<IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>>>();

        /// <summary>
        /// Uses the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter> Use(
            IMiddleware<BootstrapperContainerBase<TRootViewModel, TIocContainerAdapter>> middleware)
        {
            _middlewares.Add(middleware);
            return this;
        }
    }
}
