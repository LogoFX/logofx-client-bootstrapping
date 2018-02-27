using Caliburn.Micro.Xamarin.Forms;
using LogoFX.Bootstrapping;
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
        /// <param name="bootstrapper"></param>
        /// <param name="dependencyRegistrator"></param>
        public LogoFXApplication(
            BootstrapperBase bootstrapper, 
            IDependencyRegistrator dependencyRegistrator)
        {
            _dependencyRegistrator = dependencyRegistrator;
            Initialize();
            bootstrapper
                .Use(new RegisterCompositionModulesMiddleware<BootstrapperBase>())
                .Use(new RegisterRootViewModelMiddleware<BootstrapperBase, TRootViewModel>())
                .Initialize();
            DisplayRootViewFor<TRootViewModel>();
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="navigationPage"></param>
        protected override void PrepareViewFirst(NavigationPage navigationPage)
        {
            _dependencyRegistrator.RegisterInstance<INavigationService>(new NavigationPageAdapter(navigationPage));
        }
    }
}
