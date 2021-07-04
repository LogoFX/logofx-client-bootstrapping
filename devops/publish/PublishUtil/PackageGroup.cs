using System;
using Newtonsoft.Json;

namespace PublishUtil
{
    internal sealed class PackageGroup : IPackageGroup
    {
        [JsonConstructor]
        public PackageGroup(
            string id, 
            string[] dependencies)
        {
            Dependencies = dependencies;
            Id = id;
        }

        public PackageGroup(
            string id)
        {
            Dependencies = Array.Empty<string>();
            Id = id;
        }

        public override string ToString()
        {
            return Id;
        }

        public string[] Dependencies { get; }
        public string Id { get; }
    }
}
