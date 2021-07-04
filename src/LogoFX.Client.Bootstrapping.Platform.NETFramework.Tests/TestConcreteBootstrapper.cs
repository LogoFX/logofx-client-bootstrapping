using System;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using LogoFX.Practices.IoC;
using Solid.Practices.Composition;

namespace LogoFX.Client.Bootstrapping.Platform.NETFramework.Tests
{
    class TestConcreteBootstrapper : BootstrapperContainerBase<ExtendedSimpleContainerAdapter, ExtendedSimpleContainer>
    {
        public TestConcreteBootstrapper(ExtendedSimpleContainer iocContainer, Func<ExtendedSimpleContainer, ExtendedSimpleContainerAdapter> adapterCreator) : 
            base(iocContainer, adapterCreator, new BootstrapperCreationOptions
            {
                UseApplication = false
            })
        {            
        }

        public override CompositionOptions CompositionOptions { get; } = new CompositionOptions
        {
            Prefixes = new[] {"LogoFX.Client"}
        };
    }
}
