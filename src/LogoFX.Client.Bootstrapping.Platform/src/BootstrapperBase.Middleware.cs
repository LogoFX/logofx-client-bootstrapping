using System.Collections.Generic;
using LogoFX.Bootstrapping;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
#if TEST
    partial class TestBootstrapperBase
#else
    partial class BootstrapperBase
#endif
    {
        private readonly
            List<IMiddleware<IBootstrapper>>
            _middlewares =
                new List<IMiddleware<IBootstrapper>>();

        /// <summary>
        /// Extends the functionality by using the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        public IBootstrapper Use(
            IMiddleware<IBootstrapper> middleware)
        {
            _middlewares.Add(middleware);
            return this;
        }
    }
}
