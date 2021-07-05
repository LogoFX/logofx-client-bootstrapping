using System;
using Caliburn.Micro;
using LogoFX.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using LogoFX.Core;
using Solid.Extensibility;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    public partial class
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
    <TIocContainerAdapter, TIocContainer>
    {
        private readonly MiddlewaresWrapper<IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>> _middlewaresWrapper;

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer> Use(
            IMiddleware<IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>> middleware)
        {
            _middlewaresWrapper.Use(middleware);
            return this;
        }        
    }
}