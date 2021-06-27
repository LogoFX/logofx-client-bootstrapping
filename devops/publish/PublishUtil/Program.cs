using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Solid.Core;

namespace PublishUtil
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var single = args != null && args.Length > 0 ? args[0] : null;
            var packages = InitTopology();
            GoUp(3);
            foreach (var package in packages)
            {
                PublishPackage(package.Id);
            }
        }

        private static void PublishPackage(string packageId)
        {            
            Directory.SetCurrentDirectory(Path.Combine(Directory.GetCurrentDirectory(), "pack"));
            var packBat = Path.Combine(Directory.GetCurrentDirectory(), "pack-single.bat");
            var process = Process.Start(packBat, new [] { packageId });
            process.WaitForExit();
            GoUp(1);            
            Directory.SetCurrentDirectory(Path.Combine(Directory.GetCurrentDirectory(), "publish"));
            var copyBat = Path.Combine(Directory.GetCurrentDirectory(), "copy-single.bat");
            //TODO: pass version as well
            process = Process.Start(copyBat, new [] {packageId, "2.2.0", "../../../../packages/Tests-All" });
            process.WaitForExit();
            GoUp(1);
        }

        private static void GoUp(int numberOfLevels)
        {
            var relativePath = new List<string> { Directory.GetCurrentDirectory() };
            relativePath.AddRange(Enumerable.Repeat("..", numberOfLevels));
            Directory.SetCurrentDirectory(Path.Combine(relativePath.ToArray()));
        }

        private static IEnumerable<IPackageGroup> InitTopology()
        {
            var contents = File.ReadAllText("topology.json");
            var data = JsonConvert.DeserializeObject<Topology>(contents).Data;                        
            data = data.SortTopologically().ToList();
            return data;
        }
    }

    internal interface IPackageGroup : IHaveDependencies, IIdentifiable
    {

    }

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

    internal sealed class Topology
    {
        public List<PackageGroup> Data { get; set; }
    }
}
