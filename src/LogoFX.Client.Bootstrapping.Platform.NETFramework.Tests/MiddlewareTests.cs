using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace LogoFX.Client.Bootstrapping.Platform.NETFramework.Tests
{
    [TestFixture]
    class MiddlewareTests
    {
        [Test]
        public void WhenRegisterCoreMiddlewareIsApplied_ThenCoreElementsAreRegistered()
        {
            var container = new FakeIocContainer();
            var bootstrapper = new FakeBootstrapperWithContainerAdapter(container);

            var middleware = new RegisterContainerAdapterMiddleware<FakeIocContainer>();
            middleware.Apply(bootstrapper);

            var instances = container.Instances;
            var actualTypeContainerRegistration = instances.First();
            actualTypeContainerRegistration.Instance.ShouldBe(container);
            actualTypeContainerRegistration.InstanceType.ShouldBe(typeof(FakeIocContainer));            
        }
    }
}
