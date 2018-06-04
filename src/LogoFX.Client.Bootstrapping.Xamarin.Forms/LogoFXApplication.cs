using Caliburn.Micro.Xamarin.Forms;
using LogoFX.Bootstrapping;
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

            var dependencyRegistrator = bootstrapper.Registrator;
            dependencyRegistrator.RegisterTransient(() => NavigationContext.NavigationService);

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
            NavigationContext.NavigationService = new NavigationPageAdapter(navigationPage);
        }
    }
}