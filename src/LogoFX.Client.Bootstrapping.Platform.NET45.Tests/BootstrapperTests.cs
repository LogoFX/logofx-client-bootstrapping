using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using NUnit.Framework;
using Solid.Practices.Composition;

namespace LogoFX.Client.Bootstrapping.Platform.NET45.Tests
{
    [TestFixture]
    class BootstrapperTests
    {
        [Test]
        public void Initialization_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new TestBootstrapper(new ExtendedSimpleContainerAdapter(),new BootstrapperCreationOptions
            {
                UseApplication = false
            }));
        }
    }

    class TestShellViewModel
    {
        
    }

    class TestBootstrapper : BootstrapperContainerBase<TestShellViewModel, ExtendedSimpleContainerAdapter>
    {
        public TestBootstrapper(ExtendedSimpleContainerAdapter iocContainerAdapter) : base(iocContainerAdapter)
        {
        }

        public TestBootstrapper(ExtendedSimpleContainerAdapter iocContainerAdapter, BootstrapperCreationOptions creationOptions) : 
            base(iocContainerAdapter, creationOptions)
        {
            PlatformProvider.Current = new NetPlatformProvider();
        }
    }
}
