using Solid.Bootstrapping;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping.Xamarin.Forms
{
    public class RegisterRootViewModelMiddleware<TBootstrapper, TRootViewModel> : IMiddleware<TBootstrapper>
        where TBootstrapper : class, IHaveRegistrator
        where TRootViewModel : class
    {
        public TBootstrapper Apply(TBootstrapper @object)
        {
            @object.Registrator.RegisterSingleton<TRootViewModel>();
            return @object;
        }
    }

}
