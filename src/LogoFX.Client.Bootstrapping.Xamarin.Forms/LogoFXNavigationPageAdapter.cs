using System;
using System.Threading.Tasks;
using Caliburn.Micro.Xamarin.Forms;
using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace LogoFX.Client.Mvvm.Navigation
{
    internal class LogoFXNavigationPageAdapter : NavigationPageAdapter, ILogoFXNavigationService
    {
        private readonly NavigationPage _navigationPage;

        public LogoFXNavigationPageAdapter(NavigationPage navigationPage)
            : base(navigationPage)
        {
            _navigationPage = navigationPage;
        }

        public Task NavigateToViewModelInstanceAsync<TViewModel>(TViewModel viewModel, bool animated = true)
        {
            Element view;

            try
            {
                view = ViewLocator.LocateForModelType(typeof(TViewModel), null, null);
            }

            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw;
            }

            Page page = view as Page;
            if (page == null && !(view is ContentView))
            {
                throw new NotSupportedException($"{view.GetType()} does not inherit from either {typeof(Page)} or {typeof(ContentView)}.");
            }

            try
            {
                ViewModelBinder.Bind(viewModel, view, null);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                throw;
            }

            ContentView view1 = view as ContentView;
            if (view1 != null)
            {
                page = CreateContentPage(view1, viewModel);
            }

            return _navigationPage.PushAsync(page, animated);
        }
    }
}