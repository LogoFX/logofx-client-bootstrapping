using System.Collections.Generic;
using System.Reflection;
using Solid.Practices.Middleware;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Represents the app bootstrapper.
    /// </summary>
    public interface IBootstrapper
    {
        /// <summary>
        /// Gets the list of modules that were discovered during bootstrapper configuration.
        /// </summary>
        /// <value>
        /// The list of modules.
        /// </value>
        IEnumerable<ICompositionModule> Modules { get; }

        /// <summary>
        /// Gets the assemblies that will be inspected for the application components.
        /// </summary>
        /// <value>
        /// The assemblies.
        /// </value>
        Assembly[] Assemblies { get; }

        /// <summary>
        /// Uses the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        IBootstrapper Use(
            IMiddleware<IBootstrapper> middleware);
    }

    /// <summary>
    /// Represents app bootstrapper with ioc container adapter.
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <seealso cref="IBootstrapper" />
    public interface IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter> : IBootstrapper
    {
        /// <summary>
        /// Gets the container adapter.
        /// </summary>
        /// <value>
        /// The container adapter.
        /// </value>
        TIocContainerAdapter ContainerAdapter { get; }

        /// <summary>
        /// Uses the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter> Use(
            IMiddleware<IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter>> middleware);
    }

    /// <summary>
    /// Represents app bootstrapper with ioc container.
    /// </summary>
    /// <typeparam name="TRootViewModel">The type of the root view model.</typeparam>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <typeparam name="TIocContainer">The type of the ioc container.</typeparam>
    /// <seealso cref="IBootstrapperWithContainerAdapter{TRootViewModel, TIocContainerAdapter}" />
    public interface IBootstrapperWithContainer<TRootViewModel, TIocContainerAdapter, TIocContainer> : 
        IBootstrapperWithContainerAdapter<TRootViewModel, TIocContainerAdapter>
    {
        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        TIocContainer Container { get; }

        /// <summary>
        /// Uses the specified middleware.
        /// </summary>
        /// <param name="middleware">The middleware.</param>
        /// <returns></returns>
        IBootstrapperWithContainer<TRootViewModel, TIocContainerAdapter, TIocContainer> Use(
            IMiddleware<IBootstrapperWithContainer<TRootViewModel, TIocContainerAdapter, TIocContainer>> middleware);
    }    
}
