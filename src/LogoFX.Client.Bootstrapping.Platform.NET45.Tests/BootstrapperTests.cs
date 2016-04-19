using System;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Practices.IoC;
using NUnit.Framework;
using Solid.Practices.Composition;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

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

        [Test]
        public void
            GivenThereIsCompositionModuleWithDependencyRegistration_WhenBootstrapperWithConcreteContainerIsUsedAndDependencyIsResolvedFromConcreteContainer_ResolvedDependencyIsValid
            ()
        {
            var container = new ExtendedSimpleContainer();
            var adapter = new ExtendedSimpleContainerAdapter(container);
            var bootstrapper = new TestConcreteBootstrapper(container, c => adapter);
            bootstrapper.Initialize();

            var dependency = container.GetInstance(typeof (IDependency), null);
            Assert.IsNotNull(dependency);
        }

        [Test]
        public void
            GivenThereIsCompositionModuleWithConcreteDependencyRegistration_WhenBootstrapperWithConcreteContainerIsUsedAndDependencyIsResolvedFromAdapter_ResolvedDependencyIsValid
            ()
        {
            var container = new ExtendedSimpleContainer();
            var adapter = new ExtendedSimpleContainerAdapter(container);
            var bootstrapper = new TestConcreteBootstrapper(container, c => adapter);
            bootstrapper.Initialize();

            var dependency = adapter.Resolve<IConcreteDependency>();
            Assert.IsNotNull(dependency);
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

    class TestConcreteBootstrapper : BootstrapperContainerBase<TestShellViewModel, ExtendedSimpleContainerAdapter, ExtendedSimpleContainer>
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

    class ServicesModule : ICompositionModule<IIocContainer>
    {
        public void RegisterModule(IIocContainer iocContainer)
        {
            iocContainer.RegisterSingleton<IDependency, Dependency>();
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
