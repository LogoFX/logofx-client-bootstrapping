using System;
using System.Collections.Generic;
using LogoFX.Bootstrapping;
using Solid.Common;
using Solid.Practices.Composition;
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
        /// Override to provide custom composition options.
        /// </summary>
        public virtual CompositionOptions CompositionOptions => new CompositionOptions();

        /// <inheritdoc />
        public IEnumerable<ICompositionModule> Modules { get; private set; } = new ICompositionModule[] { };

        /// <inheritdoc />
        public IEnumerable<Exception> Errors { get; private set; } = new Exception[] { };

        private void InitializeCompositionModules()
        {
            var compositionInfo = CompositionHelper.GetCompositionInfo(
                PlatformProvider.Current.GetAbsolutePath(CompositionOptions.RelativePath), _discoveryAspect,
                _reuseCompositionInformation);
            Modules = compositionInfo.Modules;
            Errors = compositionInfo.Errors;
        }
    }
}
