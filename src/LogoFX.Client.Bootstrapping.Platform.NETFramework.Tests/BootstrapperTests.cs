using System;
using FluentAssertions;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Client.Testing.Shared.Caliburn.Micro;
using LogoFX.Practices.IoC;
using Xunit;

namespace LogoFX.Client.Bootstrapping.Platform.NETFramework.Tests
{
    public class BootstrapperTests : IDisposable
    {
        [Fact]
        public void Initialization_DoesNotThrow()
        {
            var exception =
                Record.Exception(
                    () => new TestBootstrapper(new ExtendedSimpleContainerAdapter(), new BootstrapperCreationOptions
                    {
                        UseApplication = false
                    }));

            exception.Should().BeNull();
        }

        [Fact]
        public void
            GivenThereIsCompositionModuleWithDependencyRegistration_WhenBootstrapperWithConcreteContainerIsUsedAndDependencyIsResolvedFromConcreteContainer_ResolvedDependencyIsValid
            ()
        {
            var container = new ExtendedSimpleContainer();
            var adapter = new ExtendedSimpleContainerAdapter(container);
            var bootstrapper = new TestConcreteBootstrapper(container, c => adapter);
            bootstrapper.Initialize();

            var dependency = container.GetInstance(typeof (IDependency), null);
            dependency.Should().NotBeNull();            
        }

        [Fact]
        public void
            GivenThereIsCompositionModuleWithConcreteDependencyRegistration_WhenBootstrapperWithConcreteContainerIsUsedAndDependencyIsResolvedFromAdapter_ResolvedDependencyIsValid
            ()
        {
            var container = new ExtendedSimpleContainer();
            var adapter = new ExtendedSimpleContainerAdapter(container);
            var bootstrapper = new TestConcreteBootstrapper(container, c => adapter);
            bootstrapper.Initialize();

            var dependency = adapter.Resolve<IConcreteDependency>();
            dependency.Should().NotBeNull();
        }

        public void Dispose()
        {
            TestHelper.Teardown();
        }
    }
}
