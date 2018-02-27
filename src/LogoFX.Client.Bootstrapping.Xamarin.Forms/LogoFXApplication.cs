using Caliburn.Micro.Xamarin.Forms;
using LogoFX.Bootstrapping;
using Solid.Practices.IoC;
using Xamarin.Forms;

namespace LogoFX.Client.Bootstrapping.Xamarin.Forms
{
    public class LogoFXApplication<TRootViewModel> : FormsApplication
        where TRootViewModel : class
    {
        private readonly IDependencyRegistrator _dependencyRegistrator;

        public LogoFXApplication(BootstrapperBase bootstrapper, IDependencyRegistrator dependencyRegistrator)
        {
            _dependencyRegistrator = dependencyRegistrator;
            Initialize();
            bootstrapper
                .Use(new RegisterCompositionModulesMiddleware<BootstrapperBase>())
                .Use(new RegisterRootViewModelMiddleware<BootstrapperBase, TRootViewModel>())
                .Initialize();
            DisplayRootViewFor<TRootViewModel>();
        }

        protected override void PrepareViewFirst(NavigationPage navigationPage)
        {
            _dependencyRegistrator.RegisterInstance<INavigationService>(new NavigationPageAdapter(navigationPage));
        }
    }
}
