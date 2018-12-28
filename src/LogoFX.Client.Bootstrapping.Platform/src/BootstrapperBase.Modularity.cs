using System;
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

        /// <inheritdoc />
        public IEnumerable<ICompositionModule> Modules { get; private set; } = new ICompositionModule[] {};

        /// <inheritdoc />
        public IEnumerable<Exception> Errors { get; private set; } = new Exception[] { };

        private void InitializeCompositionModules()
        {
            var compositionInfo = CompositionHelper.GetCompositionInfo(ModulesPath, Prefixes,
                    _reuseCompositionInformation);
            Modules = compositionInfo.Modules;
            Errors = compositionInfo.Errors;
        }                
    }
}
