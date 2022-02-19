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

        var trustedAssembliesPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator);
        MetadataReference[] references = trustedAssembliesPaths.Select(x => MetadataReference.CreateFromFile(x)).ToArray();

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
    
}