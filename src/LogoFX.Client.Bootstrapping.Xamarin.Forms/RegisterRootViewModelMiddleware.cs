using Solid.Bootstrapping;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping.Xamarin.Forms
{
    /// <summary>
    /// Registers the root view model.
    /// </summary>
    /// <typeparam name="TBootstrapper"></typeparam>
    /// <typeparam name="TRootViewModel"></typeparam>
    public class RegisterRootViewModelMiddleware<TBootstrapper, TRootViewModel> : IMiddleware<TBootstrapper>
        where TBootstrapper : class, IHaveRegistrator
        where TRootViewModel : class
    {
        /// <summary>
        /// <inheritdoc />
        /// </summary>        
        public TBootstrapper Apply(TBootstrapper @object)
        {
            @object.Registrator.RegisterSingleton<TRootViewModel>();
            return @object;
        }
    }

}
