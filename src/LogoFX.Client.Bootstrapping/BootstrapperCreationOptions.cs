using System;
using System.Collections.Generic;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Represents various settings for bootstrapper creation.
    /// </summary>
    public class BootstrapperCreationOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperCreationOptions"/> class.
        /// </summary>
        public BootstrapperCreationOptions()
        {
            UseApplication = true;
            ReuseCompositionInformation = true;
            UseCompositionModules = true;
            DiscoverAssemblies = true;
            UseDefaultMiddlewares = true;
            DisplayRootView = true;
            ExcludedTypes = new List<Type>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the real application is used.
        /// The default value is <c>true</c>. Use <c>false</c> for tests.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the real application is used; otherwise, <c>false</c>.
        /// </value>
        public bool UseApplication { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the composition information is re-used. Use <c>true</c>
        /// when running lots of integration client-side tests.
        /// The default value is <c>true</c>.
        /// </summary>
        /// <value>
        /// <c>true</c> if the composition information is re-used; otherwise, <c>false</c>.
        /// </value>
        public bool ReuseCompositionInformation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the bootstrapper 
        /// should look for composition modules.
        /// The default value is <c>true</c>.
        /// </summary>
        /// <value>
        /// <c>true</c> if the composition modules should be looked for; otherwise, <c>false</c>.
        /// </value>
        public bool UseCompositionModules { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the bootstrapper
        /// should look for potential application-component assemblies.
        /// The default value is <c>true</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the assemblies should be looked for; otherwise, <c>false</c>.
        /// </value>
        public bool DiscoverAssemblies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the default middlewares
        /// are used. The default value is <c>true</c>
        /// </summary>
        /// <value>
        ///   <c>true</c> if the default middlewares are used; otherwise, <c>false</c>.
        /// </value>
        public bool UseDefaultMiddlewares { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the root view is displayed upon
        /// successful initialization. This value is used only when there is a root object.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the root view is displayed; otherwise, <c>false</c>.
        /// </value>
        public bool DisplayRootView { get; set; }

        /// <summary>
        /// Gets or sets the types to be excluded from the registration.
        /// </summary>
        /// <value>
        /// The type of the root object.
        /// </value>
        public List<Type> ExcludedTypes { get; set; }
    }
}