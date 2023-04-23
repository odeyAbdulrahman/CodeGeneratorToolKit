using CodeGeneratorToolKit.FeaturesGenerator.ViewModels;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace CodeGeneratorToolKit.Utilities
{
    public static class CodeCompilerUtility
    {
        public static void CheckFailed(EmitResult emitResult)
        {
            IEnumerable<Diagnostic> failures = emitResult.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

            var sb = new StringBuilder();
            sb.AppendLine("Failed to generate code:");

            foreach (Diagnostic diagnostic in failures)
            {
                sb.AppendLine(diagnostic.ToString());
            }

            throw new InvalidOperationException(sb.ToString());
        }
        public static Type? HandlerCompilation(GeneratorViewModel model, string code)
        {
            CSharpCompilation compilation = CSharpCompilation.Create(
            model?.Assembly,
            new[] { CSharpSyntaxTree.ParseText(code) },
            model?.HandlerVM?.UsingDirectives?.Select(a => MetadataReference.CreateFromFile(a.Location)),
                    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            // Emit the assembly into memory
            using var stream = new System.IO.MemoryStream();
            EmitResult emitResult = compilation.Emit(stream);

            if (!emitResult.Success)
            {
                IEnumerable<Diagnostic> failures = emitResult.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                var sb = new StringBuilder();
                sb.AppendLine("Failed to generate code:");

                foreach (Diagnostic diagnostic in failures)
                {
                    sb.AppendLine(diagnostic.ToString());
                }

                throw new InvalidOperationException(sb.ToString());
            }

            // Load the assembly into memory
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            Assembly assembly = Assembly.Load(stream.ToArray());

            // Find the generated class
            Type type = assembly?.GetType($"{model?.Namespace}.{model?.HandlerVM?.CommandType}{model?.FileName}Handler");
            return type;
        }
    }
}
