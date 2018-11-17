using System;
using FluentAssertions;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Practices.IoC;
using Solid.Common;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;
using Xunit;

namespace LogoFX.Client.Bootstrapping.Platform.NET.Tests
{    
    public class BootstrapperTests
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
    }

    class TestShellViewModel
    {
        
    }

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

    class TestConcreteBootstrapper : BootstrapperContainerBase<ExtendedSimpleContainerAdapter, ExtendedSimpleContainer>
    {
        public TestConcreteBootstrapper(ExtendedSimpleContainer iocContainer, Func<ExtendedSimpleContainer, ExtendedSimpleContainerAdapter> adapterCreator) : 
            base(iocContainer, adapterCreator, new BootstrapperCreationOptions
            {
                UseApplication = false
            })
        {            
        }
        
        public override string[] Prefixes
        {
            get { return new[] { "LogoFX.Client" };}
        }
    }

    interface IDependency
    {
        
    }

    class Dependency : IDependency
    {
        
    }

    interface IConcreteDependency
    {
        
    }

    class ConcreteDependency : IConcreteDependency
    {
        
    }

    class ServicesModule : ICompositionModule<IDependencyRegistrator>
    {
        public void RegisterModule(IDependencyRegistrator dependencyRegistrator)
        {
            dependencyRegistrator.RegisterSingleton<IDependency, Dependency>();
        }
    }

    class ConcreteModule : ICompositionModule<ExtendedSimpleContainer>
    {
        public void RegisterModule(ExtendedSimpleContainer iocContainer)
        {
            iocContainer.RegisterPerRequest(typeof(IConcreteDependency), null, typeof(ConcreteDependency));
        }
    }
}
