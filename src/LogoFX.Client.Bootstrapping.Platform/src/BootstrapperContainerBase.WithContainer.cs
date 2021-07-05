using System;
using System.Collections.Generic;
using LogoFX.Bootstrapping;
#if (NET || NETCORE || NETFRAMEWORK) && !TEST
#endif
#if !TEST && (NETFX_CORE || WINDOWS_UWP)
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
#endif
using LogoFX.Client.Bootstrapping.Adapters.Contracts;
using Solid.Extensibility;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Application bootstrapper with ioc container and its adapter.
    /// </summary>    
    /// <typeparam name="TIocContainerAdapter">The type of the ioc container adapter.</typeparam>
    /// <typeparam name="TIocContainer">The type of the ioc container.</typeparam>    
    public partial class
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
        <TIocContainerAdapter, TIocContainer> :
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
        <TIocContainerAdapter>,
                    IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>        
        where TIocContainerAdapter : class, IIocContainer, IIocContainerAdapter<TIocContainer>, IBootstrapperAdapter 
        where TIocContainer : class
    {
        /// <summary>
        /// Application bootstrapper with ioc container, its adapter and root object.
        /// </summary>
        /// <typeparam name="TRootObject"></typeparam>
        public new class WithRootObject<TRootObject> :
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            <TIocContainerAdapter, TIocContainer>
        {
#if TEST
            /// <summary>
            /// Initializes a new instance of <see cref="TestBootstrapperContainerBase{TIocContainerAdapter, TIocContainer}.WithRootObject{TRootObject}"/>
            /// </summary>
            /// <param name="iocContainer">The ioc container.</param>
            /// <param name="adapterCreator">The adapter creation function.</param>
#else
            /// <summary>
            /// Initializes a new instance of <see cref="BootstrapperContainerBase{TIocContainerAdapter, TIocContainer}.WithRootObject{TRootObject}"/>
            /// </summary>
            /// <param name="iocContainer">The ioc container.</param>
            /// <param name="adapterCreator">The adapter creation function.</param>
#endif
            public WithRootObject(TIocContainer iocContainer,
            Func<TIocContainer, TIocContainerAdapter> adapterCreator)
                : this(iocContainer, adapterCreator, new BootstrapperCreationOptions
                {
                    ExcludedTypes = new List<Type> { typeof(TRootObject) }
                })
            {
            }

#if TEST
            /// <summary>
            /// Initializes a new instance of <see cref="TestBootstrapperContainerBase{TIocContainerAdapter, TIocContainer}.WithRootObject{TRootObject}"/>
            /// </summary>
            /// <param name="iocContainer">The ioc container.</param>
            /// <param name="adapterCreator">The adapter creation function.</param>
            /// <param name="creationOptions">The bootstrapper creation options.</param>
#else
            /// <summary>
            /// Initializes a new instance of <see cref="BootstrapperContainerBase{TIocContainerAdapter, TIocContainer}.WithRootObject{TRootObject}"/>
            /// </summary>
            /// <param name="iocContainer">The ioc container.</param>
            /// <param name="adapterCreator">The adapter creation function.</param>
            /// <param name="creationOptions">The bootstrapper creation options.</param>
#endif            
            public WithRootObject(TIocContainer iocContainer,
            Func<TIocContainer, TIocContainerAdapter> adapterCreator,
                BootstrapperCreationOptions creationOptions) : base(iocContainer, adapterCreator, AddRootObject(creationOptions))
            {
                Use(new CreateRootObjectMiddleware<TIocContainerAdapter>(                    
                    typeof(TRootObject),
                    creationOptions.DisplayRootView,
                    true));
            }

            private static BootstrapperCreationOptions AddRootObject(BootstrapperCreationOptions creationOptions)
            {
                if (creationOptions.ExcludedTypes == null)
                {
                    creationOptions.ExcludedTypes = new List<Type>();
                }
                if (creationOptions.ExcludedTypes.Contains(typeof(TRootObject)) == false)
                {
                    creationOptions.ExcludedTypes.Add(typeof(TRootObject));
                }
                return creationOptions;
            }
        }

        /// <summary>
        /// Application bootstrapper with ioc container, its adapter and root object.
        /// </summary>        
        /// <typeparam name="TRootObject"></typeparam>
        public new class WithRootObjectAsContract<TRootObject> :
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            <TIocContainerAdapter, TIocContainer>
        {
#if TEST
            /// <summary>
            /// Initializes a new instance of <see cref="TestBootstrapperContainerBase{TIocContainerAdapter, TIocContainer}.WithRootObject{TRootObject}"/>
            /// </summary>
            /// <param name="iocContainer">The ioc container.</param>
            /// <param name="adapterCreator">The adapter creation function.</param>
#else
            /// <summary>
            /// Initializes a new instance of <see cref="BootstrapperContainerBase{TIocContainerAdapter, TIocContainer}.WithRootObject{TRootObject}"/>
            /// </summary>
            /// <param name="iocContainer">The ioc container.</param>
            /// <param name="adapterCreator">The adapter creation function.</param>
#endif
            public WithRootObjectAsContract(TIocContainer iocContainer,
            Func<TIocContainer, TIocContainerAdapter> adapterCreator)
                : this(iocContainer, adapterCreator, new BootstrapperCreationOptions
                {
                    ExcludedTypes = new List<Type> { typeof(TRootObject) },
                    RegisterRoot = false
                })
            {
            }

#if TEST
            /// <summary>
            /// Initializes a new instance of <see cref="TestBootstrapperContainerBase{TIocContainerAdapter, TIocContainer}.WithRootObject{TRootObject}"/>
            /// </summary>
            /// <param name="iocContainer">The ioc container.</param>
            /// <param name="adapterCreator">The adapter creation function.</param>
            /// <param name="creationOptions">The bootstrapper creation options.</param>
#else
            /// <summary>
            /// Initializes a new instance of <see cref="BootstrapperContainerBase{TIocContainerAdapter, TIocContainer}.WithRootObject{TRootObject}"/>
            /// </summary>
            /// <param name="iocContainer">The ioc container.</param>
            /// <param name="adapterCreator">The adapter creation function.</param>
            /// <param name="creationOptions">The bootstrapper creation options.</param>
#endif
            public WithRootObjectAsContract(TIocContainer iocContainer,
            Func<TIocContainer, TIocContainerAdapter> adapterCreator,
                BootstrapperCreationOptions creationOptions) : base(iocContainer, adapterCreator, AddRootObjectAsContract(creationOptions))
            {
                Use(new CreateRootObjectMiddleware<TIocContainerAdapter>(                    
                    typeof(TRootObject),
                    creationOptions.DisplayRootView, 
                    creationOptions.RegisterRoot));
            }

            private static BootstrapperCreationOptions AddRootObjectAsContract(BootstrapperCreationOptions creationOptions)
            {
                if (creationOptions.ExcludedTypes == null)
                {
                    creationOptions.ExcludedTypes = new List<Type>();
                }               
                if (creationOptions.ExcludedTypes.Contains(typeof(TRootObject)) == false)
                {
                    creationOptions.ExcludedTypes.Add(typeof(TRootObject));
                }
                creationOptions.RegisterRoot = false;
                return creationOptions;
            }
        }

#if TEST
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="TestBootstrapperContainerBase{TIocContainerAdapter, TIocContainer}"/> class.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        /// <param name="adapterCreator">The adapter creator function.</param>
#else
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="BootstrapperContainerBase{TIocContainerAdapter, TIocContainer}"/> class.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        /// <param name="adapterCreator">The adapter creator function.</param>
#endif
        public
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            (
            TIocContainer iocContainer, 
            Func<TIocContainer, TIocContainerAdapter> adapterCreator) : 
            this(iocContainer, adapterCreator, new BootstrapperCreationOptions())
        {           
        }

#if TEST
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="TestBootstrapperContainerBase{TIocContainerAdapter, TIocContainer}"/> class.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        /// <param name="adapterCreator">The adapter creator function.</param>
        /// <param name="creationOptions">The bootstrapper creation options.</param>
#else
        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="BootstrapperContainerBase{TIocContainerAdapter, TIocContainer}"/> class.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        /// <param name="adapterCreator">The adapter creator function.</param>
        /// <param name="creationOptions">The bootstrapper creation options.</param>
#endif
        public
#if TEST
    TestBootstrapperContainerBase
#else
    BootstrapperContainerBase
#endif
            (
            TIocContainer iocContainer,
            Func<TIocContainer, TIocContainerAdapter> adapterCreator,
            BootstrapperCreationOptions creationOptions) : base(adapterCreator(iocContainer), 
                creationOptions)
        {
            Container = iocContainer;
            _middlewaresWrapper =
                new MiddlewaresWrapper<IBootstrapperWithContainer<TIocContainerAdapter, TIocContainer>>(this);
            if (creationOptions.UseDefaultMiddlewares)
            {
                Use(new RegisterCompositionModulesMiddleware<TIocContainerAdapter, TIocContainer>());
            }            
        }

        /// <summary>
        /// Gets the container.
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public TIocContainer Container { get; }

        /// <summary>
        /// Override this method to inject custom logic during bootstrapper configuration.
        /// </summary>
        /// <param name="dependencyRegistrator">The dependency registrator.</param>
        protected override void OnConfigure(IDependencyRegistrator dependencyRegistrator)
        {
            base.OnConfigure(dependencyRegistrator);            
            MiddlewareApplier.ApplyMiddlewares(this, _middlewaresWrapper.Middlewares);
        }
    }
}