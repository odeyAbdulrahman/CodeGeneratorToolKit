using CodeGeneratorToolKit.FeaturesGenerator.ViewModels;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using System.Reflection;
using CodeGeneratorToolKit.Utilities;

namespace CodeGeneratorToolKit.FeaturesGenerator.Compilation
{
    public static class CodeCompilation
    {
        public static Type? HandlerCompilation(GeneratorViewModel model, string code)
        {
            CSharpCompilation compilation = CSharpCompilation.Create(
            model?.Assembly,
            new[] { CSharpSyntaxTree.ParseText(code) },
            model?.HandlerVM?.UsingDirectives?.Select(a => MetadataReference.CreateFromFile(a.Location)),
                    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            // Emit the assembly into memory
            using MemoryStream stream = new ();
            EmitResult emitResult = compilation.Emit(stream);

            if (!emitResult.Success)
                CodeCompilerUtility.CheckFailed(emitResult);

            // Load the assembly into memory
            stream.Seek(0, SeekOrigin.Begin);
            Assembly assembly = Assembly.Load(stream.ToArray());

            // Find the generated class
            Type type = assembly?.GetType($"{model?.Namespace}.{model?.HandlerVM?.CommandType}{model?.FileName}Handler");
            return type;
        }
    }
}
