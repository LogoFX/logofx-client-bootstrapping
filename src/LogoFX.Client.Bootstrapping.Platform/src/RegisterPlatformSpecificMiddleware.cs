using Caliburn.Micro;
using LogoFX.Bootstrapping;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Registers platform-specific services into the ioc container.
    /// </summary>
    public class RegisterPlatformSpecificMiddleware :
        IMiddleware<IBootstrapperWithRegistrator>
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapperWithRegistrator Apply(
            IBootstrapperWithRegistrator @object)
        {
#if NET || NETCORE || NETFRAMEWORK
            @object.Registrator.RegisterSingleton<IWindowManager, WindowManager>();
#endif
            return @object;
        }
    }
}