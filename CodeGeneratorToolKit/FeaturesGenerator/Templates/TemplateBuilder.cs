using CodeGeneratorToolKit.FeaturesGenerator.Enumerations;
using CodeGeneratorToolKit.FeaturesGenerator.ViewModels;
using System.Text;

namespace CodeGeneratorToolKit.FeaturesGenerator.Templates
{
    public static class TemplateBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usingDirectivesName"></param>
        /// <returns></returns>
        public static StringBuilder UsingDirectives(List<string>? usingDirectivesName)
        {
            StringBuilder usingDirectivesBuilder = new();
            foreach (string name in usingDirectivesName)
            {
                usingDirectivesBuilder.AppendLine($"using {name};");
            }
            return usingDirectivesBuilder;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propsNames"></param>
        /// <returns></returns>
        public static StringBuilder ClassProperties(List<string>? propsNames)
        {
            StringBuilder propsBuilder = new();
            foreach (string propertyName in propsNames)
            {
                propsBuilder.AppendLine($"{propertyName}");
            }
            return propsBuilder;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleNames"></param>
        /// <returns></returns>
        public static StringBuilder RuleProperties(List<string>? ruleNames)
        {
            StringBuilder ruleBuilder = new();
            foreach (string ruleName in ruleNames)
            {
                ruleBuilder.AppendLine($"{ruleName};");
            }
            return ruleBuilder;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ConstructorParamTypes"></param>
        /// <returns></returns>
        public static (StringBuilder, StringBuilder, StringBuilder) ConstructorPropertiesAndParameters(List<Type>? ConstructorParamTypes)
        {
            StringBuilder PropertiesBuilder = new();
            StringBuilder ParametersBuilder = new();
            StringBuilder InitBuilder = new();
            for (int i = 0; i < ConstructorParamTypes.Count; i++)
            {
                Type paramType = ConstructorParamTypes[i];
                string genTypeName = paramType.IsGenericType ? $"{paramType.Name.Split('`')[0]}<{string.Join(", ", paramType.GetGenericArguments().Select(t => t.Name))}>" : paramType.Name;
                string paramTypeName = paramType.IsGenericType ? genTypeName[1..genTypeName.IndexOf("<")]: genTypeName[1..];
                paramTypeName = paramTypeName == "UnitOfWorkVM" ? "UnitOfWork" : paramTypeName;
                ParametersBuilder.Append($"{genTypeName} {paramTypeName.ToLower()}");
                PropertiesBuilder.Append($"private readonly {genTypeName} {paramTypeName};   ");
                InitBuilder.Append($"{paramTypeName} = {paramTypeName.ToLower()}");


                if (i != ConstructorParamTypes.Count - 1)
                {
                    ParametersBuilder.Append(", ");
                    InitBuilder.Append("; ");
                }
            }
            return (PropertiesBuilder, ParametersBuilder, InitBuilder);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateModel"></param>
        /// <param name="generatorModel"></param>
        /// <returns></returns>
        public static string? MediatorCommand(this CommandTemplateViewModel? templateModel, GeneratorViewModel? generatorModel)
        {
            return
$@"{templateModel?.UsingDirectivesBuilder}
namespace {generatorModel?.Namespace}.{generatorModel?.HandlerVM?.OprationType}s.{generatorModel?.HandlerVM?.CommandType}
{{
    public class {generatorModel?.HandlerVM?.CommandType}{generatorModel?.FileName}Command {(generatorModel?.CommandOrQueryVM?.HasActionVM == true ? ": ActionViewModel," : ":")} IRequest<{templateModel?.ResponseTypeName}>
    {{
        {templateModel?.PropsBuilder}
    }}
}}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateModel"></param>
        /// <param name="generatorModel"></param>
        /// <returns></returns>
        public static string? MediatorQueryHandler(this CommandTemplateViewModel? templateModel, GeneratorViewModel? generatorModel)
        {
            return
$@"{templateModel?.UsingDirectivesBuilder}
namespace {generatorModel?.Namespace}.{generatorModel?.HandlerVM?.OprationType.ToString()[..4]}ies.Get{generatorModel?.HandlerVM?.QueryType}
{{
    public class {generatorModel?.HandlerVM?.QueryType}{generatorModel?.FileName}Query {(generatorModel?.CommandOrQueryVM?.HasActionVM == true ? ": ActionViewModel," : ":")} IRequest<{templateModel?.ResponseTypeName}>
    {{
        {templateModel?.PropsBuilder}
    }}
}}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateModel"></param>
        /// <param name="generatorModel"></param>
        /// <returns></returns>
        public static string? MediatorCommandHandler(this TemplateViewModel? templateModel, GeneratorViewModel? generatorModel, Func<TemplateViewModel?, GeneratorViewModel?, string> funcOperationContent)
        {
            return
$@"{templateModel?.UsingDirectivesBuilder}
namespace {generatorModel?.Namespace}.{generatorModel?.HandlerVM?.OprationType}s.{generatorModel?.HandlerVM?.CommandType}
{{
    internal class {generatorModel?.HandlerVM?.CommandType}{generatorModel?.FileName}Handle : IRequestHandler<{templateModel?.RequestTypeName}, {templateModel?.ResponseTypeName}>
    {{
        {templateModel?.ConstructorPropsBuilder}
        public {generatorModel?.HandlerVM?.CommandType}{generatorModel?.FileName}Handle({templateModel?.ConstructorParamsBuilder})
        {{
            {templateModel?.ConstructorInitsBuilder};
        }}

        public async Task<{templateModel?.ResponseTypeName}> Handle({templateModel?.RequestTypeName} request, CancellationToken cancellationToken)
        {{
            // Implement the logic for handling the {templateModel?.RequestTypeName} here
            {funcOperationContent(templateModel, generatorModel)}
        }}
    }}
}}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateModel"></param>
        /// <param name="generatorModel"></param>
        /// <param name="funcOperationContent"></param>
        /// <returns></returns>
        public static string? MediatorQueryHandler(this TemplateViewModel? templateModel, GeneratorViewModel? generatorModel, Func<TemplateViewModel?, GeneratorViewModel?, string> funcOperationContent)
        {
            return
$@"{templateModel?.UsingDirectivesBuilder}
namespace {generatorModel?.Namespace}.{generatorModel?.HandlerVM?.OprationType?.ToString()[..4]}ies.Get{generatorModel?.HandlerVM?.QueryType}
{{
    internal class {generatorModel?.HandlerVM?.QueryType}{generatorModel?.FileName}Handle : IRequestHandler<{templateModel?.RequestTypeName}, {templateModel?.ResponseTypeName}>
    {{
        {templateModel?.ConstructorPropsBuilder}
        public {generatorModel?.HandlerVM?.QueryType}{generatorModel?.FileName}Handle({templateModel?.ConstructorParamsBuilder})
        {{
            {templateModel?.ConstructorInitsBuilder};
        }}

        public async Task<{templateModel?.ResponseTypeName}> Handle({templateModel?.RequestTypeName} request, CancellationToken cancellationToken)
        {{
            // Implement the logic for handling the {templateModel?.RequestTypeName} here
            {funcOperationContent(templateModel, generatorModel)}
        }}
    }}
}}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateModel"></param>
        /// <param name="generatorModel"></param>
        /// <returns></returns>
        public static string? MediatorCommandValidator(this ValidatorTemplateViewModel? templateModel, GeneratorViewModel? generatorModel)
        {
            return
$@"{templateModel?.UsingDirectivesBuilder}
namespace {generatorModel?.Namespace}.{generatorModel?.HandlerVM?.OprationType}s.{generatorModel?.HandlerVM?.CommandType}
{{
    public class {generatorModel?.HandlerVM?.CommandType}{generatorModel?.FileName}Validator: AbstractValidator<{templateModel?.CommandOrQueryTypeName}>
    {{
       public {generatorModel?.HandlerVM?.CommandType}{generatorModel?.FileName}Validator()
        {{
            {templateModel?.RulePropsBuilder}
        }}
    }}
}}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateModel"></param>
        /// <param name="generatorModel"></param>
        /// <returns></returns>
        public static string? MediatorQueryValidator(this ValidatorTemplateViewModel? templateModel, GeneratorViewModel? generatorModel)
        {
            return
$@"{templateModel?.UsingDirectivesBuilder}
namespace {generatorModel?.Namespace}.{generatorModel?.HandlerVM?.OprationType.ToString()[..4]}ies.Get{generatorModel?.HandlerVM?.QueryType}
{{
    public class {generatorModel?.HandlerVM?.QueryType}{generatorModel?.FileName}Validator: AbstractValidator<{templateModel?.CommandOrQueryTypeName}>
    {{
       public {generatorModel?.HandlerVM?.QueryType}{generatorModel?.FileName}Validator()
        {{
            {templateModel?.RulePropsBuilder}
        }}
    }}
}}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateModel"></param>
        /// <param name="generatorModel"></param>
        /// <returns></returns>
        public static string? ClassViewModel(this ClassTemplateViewModel? templateModel, GeneratorViewModel? generatorModel)
        {
            return
$@"{templateModel?.UsingDirectivesBuilder}
namespace {generatorModel?.Namespace}.ViewModels
{{
    public class {generatorModel?.ClassVM?.ClassName}{generatorModel?.FileName}ViewModel
    {{
        {templateModel?.PropsBuilder}
    }}
}}";
        }
    }
}
