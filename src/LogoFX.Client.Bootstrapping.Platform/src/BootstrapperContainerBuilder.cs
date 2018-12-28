using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// The builder enables creating bootstrapper instances using Fluent API.
    /// </summary>    
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    public class BootstrapperContainerBuilder<TIocContainerAdapter>         
        where TIocContainerAdapter : class, IIocContainer, IBootstrapperAdapter, IIocContainerAdapter
    {
        private readonly TIocContainerAdapter _container;
        private readonly BootstrapperCreationOptions _options = new BootstrapperCreationOptions();

        private BootstrapperContainerBuilder(TIocContainerAdapter container)
        {
            _container = container;
        }

        /// <summary>
        /// Creates a new instance of the builder.
        /// </summary>
        /// <param name="container">Container adapter.</param>
        /// <returns></returns>
        public static BootstrapperContainerBuilder<TIocContainerAdapter> CreateBuilder(TIocContainerAdapter container)
        {
            return new BootstrapperContainerBuilder<TIocContainerAdapter>(container);
        }

        /// <summary>
        /// Sets a value indicating whether the real application is used.
        /// </summary>
        /// <param name="useApplication"><c>true</c> if the real application is used; otherwise, <c>false</c>.</param>
        /// <returns></returns>
        public BootstrapperContainerBuilder<TIocContainerAdapter> UseApplication(bool useApplication)
        {
            _options.UseApplication = useApplication;
            return this;
        }

        /// <summary>
        /// Sets a value indicating whether the composition information is re-used. 
        /// Use <c>true</c>
        /// when running lots of integration client-side tests.
        /// </summary>
        /// <param name="reuseCompositionInformation">
        /// <c>true</c> if the composition information is re-used; otherwise, <c>false</c>.
        /// </param>
        /// <returns></returns>
        public BootstrapperContainerBuilder<TIocContainerAdapter> ReuseCompositionInformation(bool reuseCompositionInformation)
        {
            _options.ReuseCompositionInformation = reuseCompositionInformation;
            return this;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the bootstrapper
        /// should look for potential application-component assemblies.
        /// </summary>
        /// <param name="inspectAssemblies">
        /// <c>true</c> if the assemblies should be looked for; otherwise, <c>false</c>.
        /// </param>
        /// <returns></returns>
        public BootstrapperContainerBuilder<TIocContainerAdapter> DiscoverAssemblies(bool inspectAssemblies)
        {
            _options.DiscoverAssemblies = inspectAssemblies;
            return this;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the bootstrapper 
        /// should look for instances of <see cref="ICompositionModule"/>.
        /// </summary>
        /// <param name="discoverCompositionModules">
        /// <c>true</c> if the composition modules should be looked for; otherwise, <c>false</c>.
        /// </param>
        /// <returns></returns>
        public BootstrapperContainerBuilder<TIocContainerAdapter> UseCompositionModules(bool discoverCompositionModules)
        {
            _options.UseCompositionModules = discoverCompositionModules;
            return this;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the default middlewares
        /// are used.
        /// </summary>       
        /// <param name="useDefaultMiddlewares">
        /// <c>true</c> if the composition modules should be looked for; otherwise, <c>false</c>.
        /// </param>
        /// <returns></returns>
        public BootstrapperContainerBuilder<TIocContainerAdapter> UseDefaultMiddlewares(bool useDefaultMiddlewares)
        {
            _options.UseDefaultMiddlewares = useDefaultMiddlewares;
            return this;
        }

        /// <summary>
        /// Builds a new instance of <see cref="BootstrapperContainerBase{TIocContainerAdapter}"/>
        /// </summary>
        /// <returns></returns>
        public BootstrapperContainerBase<TIocContainerAdapter> Build()
        {
            return new BootstrapperContainerBase<TIocContainerAdapter>(_container, _options);
        }
    }
}
