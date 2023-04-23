using System.Reflection.Emit;
using System.Reflection;

namespace CodeGeneratorToolKit.Utilities
{
    internal static class AssemblybuilderUtility
    {
        public static Type? CreateAssemblyBuilder(string assembName, string moduleName, string className, bool? IsList = null)
        {
            // Create a new assembly builder
            AssemblyName assemblyName = new(assembName);
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);

            // Define a new module
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName);

            if (IsList == true)
            {
                // Define the List<className> type
                TypeBuilder listTypeBuilder = moduleBuilder.DefineType($"List<{className}>", TypeAttributes.Public | TypeAttributes.Class);
                return listTypeBuilder?.CreateType();
            }
            else
            {
                // Define a new class
                TypeBuilder? typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public);
                return typeBuilder?.CreateType();
            }
        }
    }
}
