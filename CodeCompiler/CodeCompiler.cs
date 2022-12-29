using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeCompiler;

public class CodeCompiler
{
    public CompilationResult CompileCSharp(string code, string moduleName)
    {
        var trees = new List<SyntaxTree>();
        var tree = CSharpSyntaxTree.ParseText(code);
        trees.Add(tree);

        var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        var exePath = Assembly.GetExecutingAssembly().Location;

        var builtInGaAssemblies = Array.Empty<Assembly>();

        try
        {
            var abstractionAssembly = Assembly.LoadFrom(Path.Combine(exePath, "AbstractionProvider.dll"));
            var nearestNeighborAssembly = Assembly.LoadFrom(Path.Combine(exePath, "NearestNeighbor.dll"));

            builtInGaAssemblies = new[]
            {
                abstractionAssembly,
                nearestNeighborAssembly
            };
        }
        catch
        {
            Console.WriteLine("Could not find dlls for built in GA assemblies");
        }

        domainAssemblies = domainAssemblies.Concat(builtInGaAssemblies).ToArray();

        var modulesMetadata = new List<MetadataReference>();

        foreach (var assembly in domainAssemblies)
        {
            try
            {
                modulesMetadata.Add(AssemblyMetadata.Create(GetMetadata(assembly)).GetReference());
            }
            catch
            {
                Console.WriteLine($"Failed to load metadata for {assembly.FullName}");
            }
        }
        
        var compilation = CSharpCompilation.Create($"{moduleName}.dll",
            trees,
            modulesMetadata,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var result = compilation.Emit(Path.Combine("Modules", $"{moduleName}.dll"));
        var messages = result.Diagnostics.Select(x => x.GetMessage()).ToArray();
        var compilationResult = new CompilationResult()
        {
            Messages = messages,
            IsSuccessful = result.Success
        };
        return compilationResult;
    }

    private ModuleMetadata GetMetadata(Assembly assembly)
    {
        unsafe
        {
            return assembly.TryGetRawMetadata(out var blob, out var len)
                ? ModuleMetadata.CreateFromMetadata((IntPtr)blob, len)
                : throw new InvalidOperationException($"Could not get metadata from {assembly.FullName}");
        }
    }

}