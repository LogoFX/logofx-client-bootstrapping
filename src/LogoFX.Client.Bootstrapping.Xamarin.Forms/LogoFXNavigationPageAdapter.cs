using System;
using System.Threading.Tasks;
using Caliburn.Micro.Xamarin.Forms;
using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace LogoFX.Client.Mvvm.Navigation
{
    /// <summary>
    /// A <see cref="NavigationPageAdapter"/> decorator allowing navigation to view model by type.
    /// </summary>
    public class LogoFXNavigationPageAdapter : NavigationPageAdapter, ILogoFXNavigationService
    {
        private readonly NavigationPage _navigationPage;

        /// <summary>
        /// Initializes an instance of <see cref="LogoFXNavigationPageAdapter"/>
        /// </summary>
        /// <param name="navigationPage"></param>
        public LogoFXNavigationPageAdapter(NavigationPage navigationPage)
            : base(navigationPage)
        {
            _navigationPage = navigationPage;
        }

        /// <inheritdoc />
        public Task NavigateToViewModelInstanceAsync<TViewModel>(TViewModel viewModel, bool animated = true)
        {
            Element element;
            try
            {
                element = ViewLocator.LocateForModelType(typeof(TViewModel), null, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            var page = element as Page;
            if (page == null && !(element is ContentView))
                throw new NotSupportedException(
                    $"{element.GetType()} does not inherit from either {typeof(Page)} or {typeof(ContentView)}.");
            try
            {
                ViewModelBinder.Bind(viewModel, element, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            if (element is ContentView view)
                page = CreateContentPage(view, viewModel);
            return _navigationPage.PushAsync(page, animated);
        }

        /// <inheritdoc />
        public bool CanNavigateBack()
        {
            return _navigationPage.Navigation.NavigationStack.Count > 1;
        }
    }
}