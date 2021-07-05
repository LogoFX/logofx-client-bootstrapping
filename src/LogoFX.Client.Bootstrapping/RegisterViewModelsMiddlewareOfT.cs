using System;
using System.Collections.Generic;
using Solid.Bootstrapping;
using Solid.Practices.Composition.Contracts;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Registers automagically the application's view models in the transient lifestyle.
    /// </summary>
    public class RegisterViewModelsMiddleware<TBootstrapper> :
        IMiddleware<TBootstrapper> 
        where TBootstrapper : class, IHaveRegistrator, IAssemblySourceProvider
    {
        private readonly IEnumerable<Type> _excludedTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterViewModelsMiddleware"/> class.
        /// </summary>
        /// <param name="excludedTypes">The type of the root object.</param>
        public RegisterViewModelsMiddleware(IEnumerable<Type> excludedTypes)
        {
            _excludedTypes = excludedTypes;
        }

        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public TBootstrapper Apply(
            TBootstrapper @object)
        {
            @object.Registrator.RegisterViewModels(@object.Assemblies, _excludedTypes);
            return @object;
        }
    }
}