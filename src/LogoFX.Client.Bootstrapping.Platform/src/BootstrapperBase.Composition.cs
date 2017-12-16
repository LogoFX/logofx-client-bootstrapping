using System.Collections.Generic;
using LogoFX.Bootstrapping;
using Solid.Practices.Composition.Contracts;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping
{
#if TEST
    partial class TestBootstrapperBase
#else
    partial class BootstrapperBase
#endif 
        : ICompositionModulesProvider
    {
        private readonly bool _reuseCompositionInformation;        

        /// <summary>
        /// Gets the path of composition modules that will be discovered during bootstrapper configuration.
        /// </summary>
        public
#if NET
            virtual 
#endif
            string ModulesPath
        {
            get { return "."; }
        }

        /// <summary>
        /// Gets the prefixes of the modules that will be used by the module registrator
        /// during bootstrapper configuration. Use this to implement efficient discovery.
        /// </summary>
        /// <value>
        /// The prefixes.
        /// </value>
        public virtual string[] Prefixes
        {
            get { return new string[] { }; }
        }

        /// <summary>
        /// Gets the list of modules that were discovered during bootstrapper configuration.
        /// </summary>
        /// <value>
        /// The list of modules.
        /// </value>
        public IEnumerable<ICompositionModule> Modules { get; private set; } = new ICompositionModule[] {};                

        private void InitializeCompositionModules()
        {
            Modules = CompositionHelper.GetCompositionModules(ModulesPath, Prefixes,
                    _reuseCompositionInformation);
        }                
    }
}
