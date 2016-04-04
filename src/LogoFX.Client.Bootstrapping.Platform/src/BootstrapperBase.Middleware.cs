using System.Collections.Generic;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    partial class BootstrapperBase
    {
        private readonly
            List<IMiddleware<IBootstrapper>>
            _middlewares =
                new List<IMiddleware<IBootstrapper>>();

        /// <summary>
        /// Uses the specified middleware.
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
