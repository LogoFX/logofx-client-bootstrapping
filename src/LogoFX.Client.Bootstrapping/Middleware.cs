using System;
using System.Collections.Generic;
using LogoFX.Bootstrapping;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{    
    /// <summary>
    /// Registers automagically the application's view models in the transient lifestyle.
    /// </summary>    
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class RegisterViewModelsMiddleware<TIocContainerAdapter> :
        IMiddleware<IBootstrapperWithContainerAdapter<TIocContainerAdapter>>        
        where TIocContainerAdapter : class, IIocContainer
    {
        private readonly IEnumerable<Type> _excludedTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterViewModelsMiddleware{TIocContainerAdapter}"/> class.
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
        public IBootstrapperWithContainerAdapter<TIocContainerAdapter> Apply(
            IBootstrapperWithContainerAdapter<TIocContainerAdapter> @object)
        {
            @object.ContainerAdapter.RegisterViewModels(@object.Assemblies, _excludedTypes);            
            return @object;
        }
    }    
}
