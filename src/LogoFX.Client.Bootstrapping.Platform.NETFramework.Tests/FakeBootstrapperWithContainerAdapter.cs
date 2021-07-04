namespace LogoFX.Client.Bootstrapping.Platform.NETFramework.Tests
{
    class FakeBootstrapperWithContainerAdapter : BootstrapperContainerBase<FakeIocContainer>
    {
        public FakeBootstrapperWithContainerAdapter(FakeIocContainer iocContainerAdapter) : base(iocContainerAdapter)
        {
        }

        public FakeBootstrapperWithContainerAdapter(FakeIocContainer iocContainerAdapter,
            BootstrapperCreationOptions creationOptions) : base(iocContainerAdapter, creationOptions)
        {
        }
    }
}