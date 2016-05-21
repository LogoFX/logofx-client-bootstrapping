using System;
using System.Collections.Generic;
using LogoFX.Bootstrapping;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Registers automagically the application's view models in the transient lifestyle.
    /// </summary>
    public class RegisterViewModelsMiddleware :
        IMiddleware<IBootstrapperWithContainerRegistrator>
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
        public IBootstrapperWithContainerRegistrator Apply(
            IBootstrapperWithContainerRegistrator @object)
        {
            @object.Registrator.RegisterViewModels(@object.Assemblies, _excludedTypes);            
            return @object;
        }
    }    
}
