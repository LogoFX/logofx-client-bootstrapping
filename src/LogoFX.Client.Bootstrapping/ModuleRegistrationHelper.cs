using System;
using System.Collections.Generic;
using System.Linq;
using Solid.Practices.IoC;
using Solid.Practices.Middleware;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Bootstrapping
{    
    /// <summary>
    /// Provides means for registering composition modules into the IoC container.
    /// </summary>    
    public static class ModuleRegistrationHelper
    {
        /// <summary>
        /// Registers the composition modules.
        /// </summary>
        /// <typeparam name="TIocContainer"></typeparam>
        /// <param name="iocContainer">The ioc container.</param>
        /// <param name="modules">The modules.</param>
        /// <param name="lifetimeScopeProvider">The lifetime scope provider.</param>
        public static void RegisterCompositionModules<TIocContainer>(
            TIocContainer iocContainer,
            IEnumerable<ICompositionModule> modules,
            Func<object> lifetimeScopeProvider)
            where TIocContainer : class
        {            
            var compositionModules = modules as ICompositionModule[] ?? modules.ToArray();
            if (iocContainer is IIocContainer)
            {
                RegisterGeneralContainerCompositionModulesInternal((IIocContainer)iocContainer, compositionModules);
            }
            else
            {
                RegisterConcreteContainerCompositionModulesInternal(iocContainer, compositionModules);
            }
            
            if (iocContainer is IIocContainerScoped)
            {
                var scopedMiddlewares = new IMiddleware<IIocContainerScoped>[]
                {
                    new ContainerScopedRegistrationMiddleware<IIocContainerScoped>(compositionModules,
                        lifetimeScopeProvider)
                };
                MiddlewareApplier.ApplyMiddlewares((IIocContainerScoped)iocContainer, scopedMiddlewares);
            }
        }

        /// <summary>
        /// Registers the composition modules.
        /// </summary>
        /// <param name="iocContainer">The ioc container.</param>
        /// <param name="modules">The modules.</param>        
        public static void RegisterCompositionModules<TIocContainer>(
            TIocContainer iocContainer,
            IEnumerable<ICompositionModule> modules) where TIocContainer : class
        {            
            var compositionModules = modules as ICompositionModule[] ?? modules.ToArray();
            if (iocContainer is IIocContainer)
            {
                RegisterGeneralContainerCompositionModulesInternal((IIocContainer)iocContainer, compositionModules);
            }
            else
            {
                RegisterConcreteContainerCompositionModulesInternal(iocContainer, compositionModules);
            }
        }        

        private static void RegisterGeneralContainerCompositionModulesInternal<TIocContainer>(TIocContainer iocContainer, 
            ICompositionModule[] compositionModules)
            where TIocContainer : class, IIocContainer
        {
            
            var middlewares = new List<IMiddleware<TIocContainer>>(new IMiddleware<TIocContainer>[]
            {
                new ContainerRegistrationMiddleware<TIocContainer, IIocContainer>(compositionModules),
                new ContainerPlainRegistrationMiddleware<TIocContainer>(compositionModules), 
                new ContainerHierarchicalRegistrationMiddleware<TIocContainer>(compositionModules)
            });

            MiddlewareApplier.ApplyMiddlewares(iocContainer, middlewares);
        }

        private static void RegisterConcreteContainerCompositionModulesInternal<TIocContainer>(TIocContainer iocContainer,
            ICompositionModule[] compositionModules)
            where TIocContainer : class
        {

            var middlewares = new List<IMiddleware<TIocContainer>>(new IMiddleware<TIocContainer>[]
            {
                new ContainerRegistrationMiddleware<TIocContainer, TIocContainer>(compositionModules),
                new ContainerPlainRegistrationMiddleware<TIocContainer>(compositionModules),
                new ContainerHierarchicalRegistrationMiddleware<TIocContainer>(compositionModules)
            });

            MiddlewareApplier.ApplyMiddlewares(iocContainer, middlewares);
        }
    }
}