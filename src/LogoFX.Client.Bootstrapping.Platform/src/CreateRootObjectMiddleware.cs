using System;
using Caliburn.Micro;
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using LogoFX.Core;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Registers root object into the ioc container adapter and 
    /// optionally displays it when the application starts.
    /// </summary>
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>    
    public class CreateRootObjectMiddleware<TIocContainerAdapter> :
        IMiddleware<
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            <TIocContainerAdapter>>
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter, IBootstrapperAdapter         
    {        
        private readonly Type _rootObjectType;
        private readonly bool _displayView;
        private readonly bool _registerRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateRootObjectMiddleware{TIocContainerAdapter}"/> class.
        /// </summary>        
        /// <param name="rootObjectType">Type of the root object.</param>
        /// <param name="displayView">if set to <c>true</c> the root view is displayed.</param>
        /// <param name="registerRoot">if set to <c>true</c> the root is registered.</param>
        public CreateRootObjectMiddleware(            
            Type rootObjectType,
            bool displayView, 
            bool registerRoot)
        {           
            _rootObjectType = rootObjectType;
            _displayView = displayView;
            _registerRoot = registerRoot;
        }

        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            <TIocContainerAdapter> Apply(
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            <TIocContainerAdapter> @object)
        {
            if (_registerRoot)
            {
                @object.Registrator.RegisterSingleton(_rootObjectType, _rootObjectType);
            }
            
            EventHandler strongHandler = ObjectOnInitializationCompleted;
            @object.InitializationCompleted += WeakDelegate.From(strongHandler);
            return @object;
        }

        private void ObjectOnInitializationCompleted(object sender, EventArgs eventArgs)
        {
            var bootstrapper = sender as
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
                <TIocContainerAdapter>;
            if (bootstrapper != null)
            {
                if (_displayView)
                {
#if !TEST
                    bootstrapper.DisplayRootViewForInternal(_rootObjectType);
#endif
                }
                else
                {
                    var rootObject = bootstrapper.ContainerAdapter.Resolve(_rootObjectType);
                    ScreenExtensions.TryActivate(rootObject);
                }
            }
        }
    }    
}