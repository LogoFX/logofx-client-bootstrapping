using LogoFX.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping.Platform.NETFramework.Tests
{
    class ConcreteModule : ICompositionModule<ExtendedSimpleContainer>
    {
        public void RegisterModule(ExtendedSimpleContainer iocContainer)
        {
            iocContainer.RegisterPerRequest(typeof(IConcreteDependency), null, typeof(ConcreteDependency));
        }
    }
}