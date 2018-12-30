using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Solid.Common;
using Solid.Practices.Composition;
using Solid.Practices.Composition.Client;
using Solid.Practices.Composition.Contracts;

namespace LogoFX.Client.Bootstrapping
{
#if TEST
    partial class TestBootstrapperBase
#else
    partial class BootstrapperBase
#endif
    {
        /// <summary>
        /// Override to tell the framework where to find assemblies to inspect for application components.
        /// </summary>
        /// <returns>
        /// A list of assemblies to inspect.
        /// </returns>
        protected sealed override IEnumerable<Assembly> SelectAssemblies()
        {
            return Assemblies;
        }

        private Assembly[] _assemblies;

        /// <summary>
        /// Gets the assemblies that will be inspected for the application components.
        /// </summary>
        /// <value>
        /// The assemblies.
        /// </value>
        public IEnumerable<Assembly> Assemblies => _assemblies ?? (_assemblies = CreateAssemblies());

        private Assembly[] CreateAssemblies() => _creationOptions.DiscoverAssemblies ? GetAssemblies() : new[] {GetType().GetTypeInfo().Assembly};

        private Assembly[] GetAssemblies()
        {
            OnConfigureAssemblyResolution();
            var assembliesResolver = new AssembliesResolver(
                new ClientAssemblySourceProvider(PlatformProvider.Current.GetRootPath()), GetType());
            return ((IAssembliesReadOnlyResolver) assembliesResolver).GetAssemblies().ToArray();
        }

        /// <summary>
        /// Override this to provide custom assembly namespaces collection.
        /// </summary>
        protected virtual void OnConfigureAssemblyResolution()
        {
        }
    }
}
