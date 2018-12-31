using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Solid.Practices.Composition;

namespace LogoFX.Client.Bootstrapping
{
#if TEST
    partial class TestBootstrapperBase
#else
    partial class BootstrapperBase
#endif
    {
        private DiscoveryAspect _discoveryAspect;
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
            _discoveryAspect.Initialize();
            return _discoveryAspect.Assemblies.ToArray();
        }        
    }
}
