using CodeGeneratorToolKit.FeaturesGenerator.ViewModels;
using Microsoft.CodeAnalysis;

namespace CodeGeneratorToolKit.FeaturesGenerator.Templates
{
    public static class TemplateGenerator
    {
        public static string? Code { get; private set; }
        public static Func<TemplateViewModel?, GeneratorViewModel?, string> FuncOperationContent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string? GenerateCommandType(GeneratorViewModel model)
        {
            string[]? genericArgNames = model?.ResponseType?.GetGenericArguments().Select(t => t.Name).ToArray();
            CommandTemplateViewModel handlerTemp = new()
            {
                ResponseTypeName = $"{model?.ResponseType?.Name.Split('`')[0]}<{string.Join(", ", genericArgNames)}>",
                // Build the using directives
                UsingDirectivesBuilder = TemplateBuilder.UsingDirectives(model?.CommandOrQueryVM?.UsingDirectivesNames),
                // Build the Props
                PropsBuilder = TemplateBuilder.ClassProperties(model?.CommandOrQueryVM?.PropsNames),
            };
            string code = handlerTemp.MediatorCommand(model);

            // Compile the code
            // return CodeCompiler.Compilation(model, code);

            return code;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string? GenerateQueryType(GeneratorViewModel model)
        {
            string[]? genericArgNames = model?.ResponseType?.GetGenericArguments().Select(t => t.Name).ToArray();
            CommandTemplateViewModel handlerTemp = new()
            {
                ResponseTypeName = $"{model?.ResponseType?.Name.Split('`')[0]}<{string.Join(", ", genericArgNames)}>",
                // Build the using directives
                UsingDirectivesBuilder = TemplateBuilder.UsingDirectives(model?.CommandOrQueryVM?.UsingDirectivesNames),
                // Build the Props
                PropsBuilder = TemplateBuilder.ClassProperties(model?.CommandOrQueryVM?.PropsNames),
            };
            string code = handlerTemp.MediatorQuery(model);

            // Compile the code
            // return CodeCompiler.Compilation(model, code);

            return code;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string? GenerateHandlerType(GeneratorViewModel model)
        {
            string[]? genericArgNames = model?.ResponseType?.GetGenericArguments().Select(t => t.Name).ToArray();
            var constructor = TemplateBuilder.ConstructorPropertiesAndParameters(model?.HandlerVM?.ConstructorParamTypes);
            TemplateViewModel handlerTemp = new()
            {
                RequestTypeName = model?.HandlerVM?.RequestType?.Name,
                ResponseTypeName = $"{model?.ResponseType?.Name.Split('`')[0]}<{string.Join(", ", genericArgNames)}>",
                // Build the using directives
                UsingDirectivesBuilder = TemplateBuilder.UsingDirectives(model?.HandlerVM?.UsingDirectivesNames),
                // Build the constructor parameters string
                ConstructorPropsBuilder = constructor.Item1,
                ConstructorParamsBuilder = constructor.Item2,
                ConstructorInitsBuilder = constructor.Item3,
            };

            if(model?.HasCommand == true)
            {
                if (model?.HandlerVM?.CommandType == Enumerations.CommandType.Create)
                    FuncOperationContent = CommandBuilder.CreateCommand;
                else
                if (model?.HandlerVM?.CommandType == Enumerations.CommandType.Active)
                    FuncOperationContent = CommandBuilder.ActiveCommand;
                else
                if (model?.HandlerVM?.CommandType == Enumerations.CommandType.Update)
                    FuncOperationContent = CommandBuilder.UpdateCommand;
                else
                if (model?.HandlerVM?.CommandType == Enumerations.CommandType.Archive)
                    FuncOperationContent = CommandBuilder.ArchiveCommand;
                else
                if (model?.HandlerVM?.CommandType == Enumerations.CommandType.Restore)
                    FuncOperationContent = CommandBuilder.RestoreCommand;
                else
                if (model?.HandlerVM?.CommandType == Enumerations.CommandType.Recover)
                    FuncOperationContent = CommandBuilder.RecoverCommand;
                else
                    FuncOperationContent = CommandBuilder.DeleteCommand;

                Code = handlerTemp.MediatorCommandHandler(model, FuncOperationContent);
                
                // Compile the code
                // return CodeCompiler.Compilation(model, code);
            }
            if (model?.HasQuery == true)
            {
                if (model?.HandlerVM?.QueryType == Enumerations.QueryType.Detail)
                    FuncOperationContent = QueryBuilder.DetailQuery;
                else
                    if (model?.HandlerVM?.QueryType == Enumerations.QueryType.List)
                    FuncOperationContent = QueryBuilder.ListQuery;
                else
                if (model?.HandlerVM?.QueryType == Enumerations.QueryType.ActiveList)
                    FuncOperationContent = QueryBuilder.ActiveListQuery;
                else
                    FuncOperationContent = QueryBuilder.ActionStatusListQuery;

                Code = handlerTemp.MediatorQueryHandler(model, FuncOperationContent);

                // Compile the code
                // return CodeCompiler.Compilation(model, code);
            }
            return Code;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string? GenerateCommandOrQueryValidatorType(GeneratorViewModel model)
        {
            ValidatorTemplateViewModel handlerTemp = new()
            {
                CommandOrQueryTypeName = model.ValidatorVM?.CommandOrQueryType?.Name,
                // Build the using directives
                UsingDirectivesBuilder = TemplateBuilder.UsingDirectives(model?.ValidatorVM?.UsingDirectivesName),
                // Build the Props
                RulePropsBuilder = TemplateBuilder.RuleProperties(model?.ValidatorVM?.RuleProps),
            };
            if (model?.HasCommand == true)
            {
                Code = handlerTemp.MediatorCommandValidator(model);
                // Compile the code
                // return CodeCompiler.Compilation(model, code);
            }
            if (model?.HasQuery == true)
            {
                Code = handlerTemp.MediatorQueryValidator(model);

                // Compile the code
                // return CodeCompiler.Compilation(model, code);
            }
            return Code;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string? GenerateClassViewModel(GeneratorViewModel model)
        {
            ClassTemplateViewModel handlerTemp = new()
            {
                // Build the using directives
                UsingDirectivesBuilder = TemplateBuilder.UsingDirectives(model?.ClassVM?.UsingDirectivesName),
                // Build the Props
                PropsBuilder = TemplateBuilder.ClassProperties(model?.ClassVM?.PropsNames),
            };
            string code = handlerTemp.ClassViewModel(model);

            // Compile the code
            // return CodeCompiler.Compilation(model, code);

            return code;
        }
    }
}
