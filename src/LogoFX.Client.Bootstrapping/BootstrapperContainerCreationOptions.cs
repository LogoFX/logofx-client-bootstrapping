using System;

namespace LogoFX.Client.Bootstrapping
{
    /// <summary>
    /// Represents various settings for bootstrapper with container creation.
    /// </summary>
    public class BootstrapperContainerCreationOptions : BootstrapperCreationOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BootstrapperContainerCreationOptions"/> class.
        /// </summary>
        public BootstrapperContainerCreationOptions()
        {
            UseDefaultMiddlewares = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the default middlewares
        /// are used. The default value is <c>true</c>
        /// </summary>
        /// <value>
        ///   <c>true</c> if the default middlewares are used; otherwise, <c>false</c>.
        /// </value>
        public bool UseDefaultMiddlewares { get; set; }

        [Obsolete("Added for compatibility reasons.")]
        public static BootstrapperContainerCreationOptions From(BootstrapperCreationOptions creationOptions)
        {
            return new BootstrapperContainerCreationOptions
            {
                DiscoverCompositionModules = creationOptions.DiscoverCompositionModules,
                InspectAssemblies = creationOptions.InspectAssemblies,
                ReuseCompositionInformation = creationOptions.ReuseCompositionInformation,
                UseApplication = creationOptions.UseApplication,
            };
        }
    }
}