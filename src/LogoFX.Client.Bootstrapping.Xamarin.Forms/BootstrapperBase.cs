using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Solid.Bootstrapping;
using Solid.Extensibility;
using Solid.Practices.Composition;
using Solid.Practices.Composition.Container;
using Solid.Practices.Composition.Contracts;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping.Xamarin.Forms
{
    /// <summary>
    /// Base class that enables the following capabiities:
    /// Modularity, 
    /// Assembly inspection, 
    /// Extensibility.
    /// </summary>
    public class BootstrapperBase : IInitializable,
        IExtensible<BootstrapperBase>,
        ICompositionModulesProvider,
        IHaveRegistrator,
        IAssemblySourceProvider
    {
        private readonly
            List<IMiddleware<BootstrapperBase>>
            _middlewares =
                new List<IMiddleware<BootstrapperBase>>();

        /// <summary>
        /// Creates an instance of <see cref="BootstrapperBase"/>
        /// </summary>
        /// <param name="dependencyRegistrator">The dependency registrator.</param>
        public BootstrapperBase(IDependencyRegistrator dependencyRegistrator)
        {
            Registrator = dependencyRegistrator;
            PlatformProvider.Current = new NetStandardPlatformProvider();
        }

        /// <summary>
        /// Gets the prefixes of the modules that will be used by the module registrator
        /// during bootstrapper configuration. Use this to implement efficient discovery.
        /// </summary>
        /// <value>
        /// The prefixes.
        /// </value>
        public virtual string[] Prefixes => new string[] { };

        /// <summary>
        /// Gets the additional types which can extend the list of assemblies
        /// to be inspected for app components. Use this to add dynamic assemblies.
        /// </summary>
        /// <value>The additional types.</value>
        public virtual Type[] AdditionalTypes => new Type[] { };

        /// <summary>
        /// Gets the list of modules that were discovered during bootstrapper configuration.
        /// </summary>
        /// <value>
        /// The list of modules.
        /// </value>
        public IEnumerable<ICompositionModule> Modules { get; private set; } = new ICompositionModule[] { };

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public IDependencyRegistrator Registrator { get; }

        private IEnumerable<Assembly> _assemblies;
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public IEnumerable<Assembly> Assemblies => _assemblies ??
            (_assemblies = LoadAssemblies().FilterByPrefixes(Prefixes));

        private IEnumerable<Assembly> LoadAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        private void InitializeCompositionModules()
        {
            ICompositionContainer<ICompositionModule> innerContainer = new SimpleCompositionContainer<ICompositionModule>(
                Assemblies,
                new ActivatorCreationStrategy());
            innerContainer.Compose();
            Modules = innerContainer.Modules.ToArray();
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public BootstrapperBase Use(
            IMiddleware<BootstrapperBase> middleware)
        {
            _middlewares.Add(middleware);
            return this;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public void Initialize()
        {
            InitializeCompositionModules();
            MiddlewareApplier.ApplyMiddlewares(this, _middlewares);
        }
    }

    internal static class AssembliesExtensions
    {
        internal static IEnumerable<Assembly> FilterByPrefixes(this IEnumerable<Assembly> assemblies, string[] prefixes) => prefixes?.Length == 0
                ? assemblies
                : assemblies.Where(t => prefixes.Any(k => t.GetName().Name.StartsWith(k)));
    }
}
