using LogoFX.Bootstrapping;
using Solid.Extensibility;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    public partial class
#if TEST
    TestBootstrapperContainerBase
#else
        BootstrapperContainerBase
#endif
        <TIocContainerAdapter>
    {
        private readonly MiddlewaresWrapper<IBootstrapperWithRegistrator> _registratorMiddlewaresWrapper;       

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public IBootstrapperWithRegistrator Use(
            IMiddleware<IBootstrapperWithRegistrator> middleware)
        {
            _registratorMiddlewaresWrapper.Use(middleware);
            return this;
        }

        private readonly MiddlewaresWrapper<IBootstrapperWithContainerAdapter<TIocContainerAdapter>> _middlewaresWrapper;       

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public IBootstrapperWithContainerAdapter<TIocContainerAdapter> Use(
            IMiddleware<IBootstrapperWithContainerAdapter<TIocContainerAdapter>> middleware)
        {
            _middlewaresWrapper.Use(middleware);
            return this;
        }

        private readonly MiddlewaresWrapper<
#if TEST
    TestBootstrapperContainerBase
#else
            BootstrapperContainerBase
#endif
            <TIocContainerAdapter>> _concreteMiddlewaresWrapper;                   

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public
#if TEST
    TestBootstrapperContainerBase
#else
            BootstrapperContainerBase
#endif
            <TIocContainerAdapter> Use(
                IMiddleware<
#if TEST
    TestBootstrapperContainerBase
#else
                    BootstrapperContainerBase
#endif
                    <TIocContainerAdapter>> middleware)
        {
            _concreteMiddlewaresWrapper.Use(middleware);           
            return this;
        }

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>        
        /// <returns></returns>
        public
#if TEST
    TestBootstrapperContainerBase
#else
            BootstrapperContainerBase
#endif
            <TIocContainerAdapter> Use<TMiddleware>()
            where TMiddleware : class, IMiddleware<
#if TEST
    TestBootstrapperContainerBase
#else
                BootstrapperContainerBase
#endif
                <TIocContainerAdapter>>
        {
            _concreteMiddlewaresWrapper.Use(
                new MiddlewareDecorator<
#if TEST
    TestBootstrapperContainerBase
#else
                    BootstrapperContainerBase
#endif
                    <TIocContainerAdapter>,
                    TMiddleware>(ContainerAdapter));
            return this;
        }
    }
}