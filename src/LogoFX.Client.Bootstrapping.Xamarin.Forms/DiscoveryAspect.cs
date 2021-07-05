using System;
using System.Collections.Generic;
using System.Reflection;
using Solid.Extensibility;
using Solid.Practices.Composition;
using Solid.Practices.Composition.Contracts;

namespace LogoFX.Client.Bootstrapping.Xamarin.Forms
{
    class DiscoveryAspect : IAspect, IAssemblySourceProvider
    {
        private readonly CompositionOptions _options;

        public DiscoveryAspect(CompositionOptions options)
        {
            _options = options;
        }

        public void Initialize()
        {
            if (_assemblies == null)
            {
                LoadAssemblies();
            }
        }

        private IEnumerable<Assembly> _assemblies;
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public IEnumerable<Assembly> Assemblies => _assemblies ??
                                                   (_assemblies = LoadAssemblies().FilterByPrefixes(_options.Prefixes));

        private IEnumerable<Assembly> LoadAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        public string[] Dependencies => new string[] {};
        public string Id => "Discovery";
    }
}
