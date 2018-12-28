using LogoFX.Bootstrapping;
using Solid.Extensibility;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
#if TEST
    partial class TestBootstrapperBase
#else
    partial class BootstrapperBase
#endif
    {
        private readonly MiddlewaresWrapper<IBootstrapper> _middlewaresWrapper;      

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public IBootstrapper Use(
            IMiddleware<IBootstrapper> middleware)
        {
            _middlewaresWrapper.Use(middleware);
            return this;
        }
    }
}
