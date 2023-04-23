using CodeGeneratorToolKit.FeaturesGenerator.Templates;
using CodeGeneratorToolKit.FeaturesGenerator.ViewModels;

namespace CodeGeneratorToolKit.Utilities
{
    public static class CodeGeneratorUtility
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Type? GetViewModelAssembly(GeneratorViewModel model)
        {
            return AssemblybuilderUtility.CreateAssemblyBuilder(model?.Assembly, "Model", $"{model?.ClassVM?.ClassName}{model?.FileName}ViewModel", model?.ClassVM?.IsList);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task GenerateAndWriteToCommandFeatureFile(GeneratorViewModel model, Type responseViewModel, Type iUnitOfWorkVM)
        {
            #region Create Directory
            //Create Directory
            string filePathCommand = Path.Combine(Directory.GetCurrentDirectory(), model?.FolderName, $"{model?.HandlerVM?.OprationType}s", $"{model?.HandlerVM?.CommandType}");
            if (!Directory.Exists(filePathCommand))
                Directory.CreateDirectory(filePathCommand);

            string filePathViewModel = Path.Combine(Directory.GetCurrentDirectory(), model?.FolderName, "ViewModels");
            if (!Directory.Exists(filePathViewModel))
                Directory.CreateDirectory(filePathViewModel);
            #endregion

            #region Response view model class
            string generateClassType = TemplateGenerator.GenerateClassViewModel(model);
            // Write the ViewModel generated code to a file
            using StreamWriter classWriter = new($"{filePathViewModel}\\{model?.ClassVM?.ClassName}{model?.FileName}ViewModel.cs");
            await classWriter.WriteAsync(generateClassType);
            Type? getViewModel = GetViewModelAssembly(model);
            #endregion

            #region Command class
            model.ResponseType = responseViewModel;
            model?.CommandOrQueryVM?.UsingDirectives?.Add(getViewModel?.Assembly);
            model?.CommandOrQueryVM?.UsingDirectivesNames?.Add($"{model.Namespace}.ViewModels");

            string generatedCommandType = TemplateGenerator.GenerateCommandType(model);
            // Write the command generated code to a file
            using StreamWriter commandWriter = new($"{filePathCommand}\\{model?.HandlerVM?.CommandType}{model?.FileName}Command.cs");
            await commandWriter.WriteAsync(generatedCommandType);
            Type? getCommand = AssemblybuilderUtility.CreateAssemblyBuilder(model?.Assembly, "Model", $"{model?.HandlerVM?.CommandType}{model?.FileName}Command");
            #endregion

            #region Set view model in handler
            model.HandlerVM.RequestType = getCommand;
            model?.HandlerVM?.UsingDirectives?.Add(getViewModel?.Assembly);
            model?.HandlerVM?.UsingDirectives?.Add(getCommand?.Assembly);
            model?.HandlerVM?.UsingDirectivesNames?.Add($"{model.Namespace}.ViewModels");
            //model?.HandlerVM?.UsingDirectivesName?.Add($"{model.Namespace}.{model?.HandlerVM?.OprationType}s.{model?.HandlerVM?.CommandType}");
            model?.HandlerVM?.ConstructorParamTypes?.Add(iUnitOfWorkVM);
            if (model?.HasValidator == true)
            {
                model.ValidatorVM.CommandOrQueryType = getCommand;
                model?.ValidatorVM?.UsingDirectives?.Add(getCommand?.Assembly);
                model?.ValidatorVM?.UsingDirectivesName?.Add($"{model.Namespace}.{model?.HandlerVM?.OprationType}s.{model?.HandlerVM?.CommandType}");
            }
            #endregion

            #region Handler generated code
            // Write the handler generated code to a file
            string generatedHandlerType = TemplateGenerator.GenerateHandlerType(model);
            using StreamWriter handlerWriter = new($"{filePathCommand}\\{model?.HandlerVM?.CommandType}{model?.FileName}Handler.cs");
            await handlerWriter.WriteAsync(generatedHandlerType);
            #endregion
            if (model?.HasValidator == true)
            {
                #region Validator generated code
                // Write the validator generated code to a file
                string generateValidatorType = TemplateGenerator.GenerateCommandOrQueryValidatorType(model);
                using StreamWriter validatorWriter = new($"{filePathCommand}\\{model?.HandlerVM?.CommandType}{model?.FileName}Validator.cs");
                await validatorWriter.WriteAsync(generateValidatorType);
            }
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="responseViewModel"></param>
        /// <param name="iUnitOfWorkVM"></param>
        /// <returns></returns>
        public static async Task GenerateAndWriteToQueryFeatureFile(GeneratorViewModel model, Type responseViewModel, Type iUnitOfWorkVM)
        {
            #region Create Directory
            //Create Directory
            string filePathQuery = Path.Combine(Directory.GetCurrentDirectory(), model?.FolderName, $"{model?.HandlerVM?.OprationType?.ToString()[..4]}ies", $"Get{model?.HandlerVM?.QueryType}");
            if (!Directory.Exists(filePathQuery))
                Directory.CreateDirectory(filePathQuery);

            string filePathViewModel = Path.Combine(Directory.GetCurrentDirectory(), model?.FolderName, "ViewModels");
            if (!Directory.Exists(filePathViewModel))
                Directory.CreateDirectory(filePathViewModel);
            #endregion

            #region Response view model class
            string generateClassType = TemplateGenerator.GenerateClassViewModel(model);
            // Write the ViewModel generated code to a file
            using StreamWriter classWriter = new($"{filePathViewModel}\\{model?.ClassVM?.ClassName}{model?.FileName}ViewModel.cs");
            await classWriter.WriteAsync(generateClassType);
            Type? getViewModel = GetViewModelAssembly(model);
            #endregion

            #region Query class
            model.ResponseType = responseViewModel;
            model?.CommandOrQueryVM?.UsingDirectives?.Add(getViewModel?.Assembly);
            model?.CommandOrQueryVM?.UsingDirectivesNames?.Add($"{model.Namespace}.ViewModels");

            string generatedQueryType = TemplateGenerator.GenerateQueryType(model);
            // Write the command generated code to a file
            using StreamWriter queryWriter = new($"{filePathQuery}\\{model?.HandlerVM?.QueryType}{model?.FileName}Query.cs");
            await queryWriter.WriteAsync(generatedQueryType);
            Type? getQuery = AssemblybuilderUtility.CreateAssemblyBuilder(model?.Assembly, "Model", $"{model?.HandlerVM?.QueryType}{model?.FileName}Query");
            #endregion

            #region Set view model in handler
            model.HandlerVM.RequestType = getQuery;
            model?.HandlerVM?.UsingDirectives?.Add(getViewModel?.Assembly);
            model?.HandlerVM?.UsingDirectives?.Add(getQuery?.Assembly);
            model?.HandlerVM?.UsingDirectivesNames?.Add($"{model.Namespace}.ViewModels");
            model?.HandlerVM?.ConstructorParamTypes?.Add(iUnitOfWorkVM);
            if (model.HasValidator == true)
            {
                model.ValidatorVM.CommandOrQueryType = getQuery;
                model?.ValidatorVM?.UsingDirectives?.Add(getQuery?.Assembly);
                model?.ValidatorVM?.UsingDirectivesName?.Add($"{model.Namespace}.{model?.HandlerVM?.OprationType?.ToString()[..4]}ies.Get{model?.HandlerVM?.QueryType}");
            }
            #endregion

            #region Handler generated code
            // Write the handler generated code to a file
            string generatedHandlerType = TemplateGenerator.GenerateHandlerType(model);
            using StreamWriter handlerWriter = new($"{filePathQuery}\\{model?.HandlerVM?.QueryType}{model?.FileName}Handler.cs");
            await handlerWriter.WriteAsync(generatedHandlerType);
            #endregion

            #region Validator generated code
            if (model.HasValidator == true)
            {
                // Write the validator generated code to a file
                string generateValidatorType = TemplateGenerator.GenerateCommandOrQueryValidatorType(model);
                using StreamWriter validatorWriter = new($"{filePathQuery}\\{model?.HandlerVM?.QueryType}{model?.FileName}Validator.cs");
                await validatorWriter.WriteAsync(generateValidatorType);
            }
            #endregion
        }
    }
}
