using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using Solid.Common;

namespace LogoFX.Client.Bootstrapping.Platform.NETFramework.Tests
{
    class TestBootstrapper : BootstrapperContainerBase<ExtendedSimpleContainerAdapter>
    {
        public TestBootstrapper(ExtendedSimpleContainerAdapter iocContainerAdapter) : base(iocContainerAdapter)
        {
        }

        public TestBootstrapper(ExtendedSimpleContainerAdapter iocContainerAdapter, BootstrapperCreationOptions creationOptions) : 
            base(iocContainerAdapter, creationOptions)
        {
            PlatformProvider.Current = new NetStandardPlatformProvider();
        }
    }
}
