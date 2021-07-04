using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LogoFX.Client.Bootstrapping.Xamarin.Forms
{
    //TODO: Check whether there's a suitable solution inside infra packages
    internal static class AssembliesExtensions
    {
        internal static IEnumerable<Assembly> FilterByPrefixes(this IEnumerable<Assembly> assemblies, string[] prefixes) => prefixes?.Length == 0
                ? assemblies
                : assemblies.Where(t => prefixes.Any(k => t.GetName().Name.StartsWith(k)));
    }   
}
