using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Xunit;

namespace Shop.Tests
{
    public class ArchitectureTests
    {
        [Fact]
        public void CrossLayerReferences()
        {
            var wrongReferences = new List<(string From, string To)>
            {
                ("UseCases", "DataAccess.MsSql"),
                ("UseCases", "Infrastructure.Implementation"),
                ("UseCases", "DomainServices.Implementation"),

                ("Controllers", "DataAccess"),
                ("Controllers", "Infrastructure.Interfaces"),
                ("Controllers", "DomainServices.Interfaces"),
                ("Controllers", "Infrastructure.Implementation"),
                ("Controllers", "DomainServices.Implementation"),

                ("Infrastructure.Implementation", "DomainServices.Implementation"),
            };
            
            var location = Assembly.GetExecutingAssembly().Location;
            var assemblies = Directory.EnumerateFiles(Path.GetDirectoryName(location), "Shop*.dll")
                .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                .ToList();

            foreach (var layer in wrongReferences)
            {
                foreach (var assembly in assemblies)
                {
                    foreach (var reference in assembly.GetReferencedAssemblies())
                    {
                        Assert.False(assembly.FullName.Contains(layer.From) && reference.FullName.Contains(layer.To),
                            $"Cross-layer reference from '{assembly.FullName}' to '{reference.FullName}'");
                    }
                }
            }
        }
    }
}
