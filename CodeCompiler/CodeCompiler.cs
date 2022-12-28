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

        /*var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        var assemblies = trustedAssembliesPaths.Append(Path.Combine(exePath, "AbstractionProvider.dll"));*/
        
        MetadataReference[] references = domainAssemblies.Select(assembly => AssemblyMetadata.Create(GetMetadata(assembly)).GetReference()).ToArray();

        var compilation = CSharpCompilation.Create($"{moduleName}.dll",
            trees,
            references,
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