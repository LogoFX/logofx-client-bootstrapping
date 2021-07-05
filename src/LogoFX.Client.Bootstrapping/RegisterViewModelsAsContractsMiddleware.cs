using System;
using System.Collections.Generic;
using LogoFX.Bootstrapping;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Registers automagically the application's view models as contracts in the transient lifestyle.
    /// </summary>
    public class RegisterViewModelsAsContractsMiddleware :
        IMiddleware<IBootstrapperWithRegistrator>
    {
        private readonly IEnumerable<Type> _excludedTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterViewModelsMiddleware"/> class.
        /// </summary>
        /// <param name="excludedTypes">The type of the root object.</param>
        public RegisterViewModelsAsContractsMiddleware(IEnumerable<Type> excludedTypes)
        {
            _excludedTypes = excludedTypes;
        }

        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithRegistrator Apply(
            IBootstrapperWithRegistrator @object)
        {
            var internalMiddleware = new RegisterViewModelsAsContractsMiddleware<IBootstrapperWithRegistrator>(_excludedTypes);
            return internalMiddleware.Apply(@object);
        }
    }
}
