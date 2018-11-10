using Caliburn.Micro.Xamarin.Forms;
using LogoFX.Client.Mvvm.Navigation;
using Solid.Bootstrapping;
using Solid.Practices.IoC;
using Xamarin.Forms;

namespace LogoFX.Client.Bootstrapping.Xamarin.Forms
{
    /// <summary>
    /// Represents a LogoFX Xamarin.Forms app.
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    public class LogoFXApplication<TRootViewModel> : FormsApplication
        where TRootViewModel : class
    {
        private readonly IDependencyRegistrator _dependencyRegistrator;

        /// <summary>
        /// Creates an instance of the <see cref="LogoFXApplication{TRootViewModel}"/>
        /// </summary>
        /// <param name="bootstrapper">The app boostrapper.</param>
        /// <param name="viewFirst">Use true to enable built-in navigation, false otherwise. The default value is true.</param>
        public LogoFXApplication(
            BootstrapperBase bootstrapper,
            bool viewFirst = true)
        {
            Initialize();

            bootstrapper
                .Use(new RegisterCompositionModulesMiddleware<BootstrapperBase>())
                .Use(new RegisterRootViewModelMiddleware<BootstrapperBase, TRootViewModel>())                
                .Initialize();

            _dependencyRegistrator = bootstrapper.Registrator;

            if (viewFirst)
            {
                var viewType = ViewLocator.LocateTypeForModelType(typeof(TRootViewModel), null, null);
                DisplayRootView(viewType);
            }
            else
            {
                //Default navigation does not work in this case
                DisplayRootViewFor<TRootViewModel>();
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="navigationPage"></param>
        protected override void PrepareViewFirst(NavigationPage navigationPage)
        {
            var navigationService = new LogoFXNavigationPageAdapter(navigationPage);
            _dependencyRegistrator
                .AddInstance(typeof(INavigationService), navigationService)
                .AddInstance(typeof(ILogoFXNavigationService), navigationService);
        }
    }
}