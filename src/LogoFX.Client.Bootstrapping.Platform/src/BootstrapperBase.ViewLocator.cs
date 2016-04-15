using System;
using System.Collections.Generic;
using System.Linq;
#if NET45
using System.Windows.Controls;
#endif
#if NETFX_CORE || WINDOWS_UWP || WIN81
using Windows.UI.Xaml.Controls;
#endif
using Caliburn.Micro;
using LogoFX.Bootstrapping;
using Solid.Practices.Middleware;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Initializes view locator.
    /// </summary>    
    public class InitializeViewLocatorMiddleware : IMiddleware<IBootstrapper>
    {
        /// <summary>
        /// Applies the middleware on the specified object.
        /// </summary>
        /// <param name="object">The object.</param>
        /// <returns></returns>
        public IBootstrapper Apply(IBootstrapper @object)
        {
            InitializeViewLocator(@object);
            return @object;
        }

        private readonly Dictionary<string, Type> _typedic = new Dictionary<string, Type>();

        private void InitializeViewLocator(IBootstrapper bootstrapper)
        {
            //overriden for performance reasons (Assembly caching)
            ViewLocator.LocateTypeForModelType = (modelType, displayLocation, context) =>
            {
                var viewTypeName = modelType.FullName.Substring(0, modelType.FullName.IndexOf("`") < 0
                    ? modelType.FullName.Length
                    : modelType.FullName.IndexOf("`")
                    ).Replace("Model", string.Empty);

                if (context != null)
                {
                    viewTypeName = viewTypeName.Remove(viewTypeName.Length - 4, 4);
                    viewTypeName = viewTypeName + "." + context;
                }

                Type viewType;
                if (!_typedic.TryGetValue(viewTypeName, out viewType))
                {
                    _typedic[viewTypeName] = viewType = (from assembly in bootstrapper.Assemblies
                                                         from type in assembly
#if NET45
                                                         .GetExportedTypes()
#endif
#if WINDOWS_UWP || NETFX_CORE || WIN81
                                                         .DefinedTypes
#endif
                                                         where type.FullName == viewTypeName
                                                         select type
#if WINDOWS_UWP || NETFX_CORE || WIN81
                                                         .AsType()
#endif
                                                         ).FirstOrDefault();
                }

                return viewType;
            };
            ViewLocator.LocateForModelType = (modelType, displayLocation, context) =>
            {
                var viewType = ViewLocator.LocateTypeForModelType(modelType, displayLocation, context);

                return viewType == null
                    ? new TextBlock
                    {
                        Text = $"Cannot find view for\nModel: {modelType}\nContext: {context} ."
                    }
                    : ViewLocator.GetOrCreateViewType(viewType);
            };
        }
    }    
}
