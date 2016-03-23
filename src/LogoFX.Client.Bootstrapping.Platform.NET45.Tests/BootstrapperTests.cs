using LogoFX.Client.Bootstrapping.Adapters.Unity;
using NUnit.Framework;

namespace LogoFX.Client.Bootstrapping.Platform.NET45.Tests
{
    [TestFixture]
    class BootstrapperTests
    {
        [Test]
        public void Initialization_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => new TestBootstrapper(new UnityContainerAdapter(),new BootstrapperCreationOptions
            {
                UseApplication = false
            }));
        }
    }

    class TestShellViewModel
    {
        
    }

    class TestBootstrapper : BootstrapperContainerBase<TestShellViewModel, UnityContainerAdapter>
    {
        public TestBootstrapper(UnityContainerAdapter iocContainerAdapter) : base(iocContainerAdapter)
        {
        }

        public TestBootstrapper(UnityContainerAdapter iocContainerAdapter, BootstrapperCreationOptions creationOptions) : 
            base(iocContainerAdapter, creationOptions)
        {
        }
    }
}
