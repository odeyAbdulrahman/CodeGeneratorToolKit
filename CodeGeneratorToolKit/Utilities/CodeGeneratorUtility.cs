using CodeGeneratorToolKit.FeaturesGenerator.Enumerations;
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
            Type? getViewModel = AssemblybuilderUtility.CreateAssemblyBuilder(model?.Assembly, "Model", $"{model?.ClassVM?.ClassName}{model?.FileName}ViewModel");
            #endregion

            #region Command class
            model.ResponseType = responseViewModel;// typeof(responseViewModel)?.MakeGenericType(getViewModel);
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
            model?.HandlerVM?.ConstructorParamTypes?.Add(iUnitOfWorkVM); //typeof(iUnitOfWorkVM).MakeGenericType(getViewModel)
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
            Type? getViewModel = AssemblybuilderUtility.CreateAssemblyBuilder(model?.Assembly, "Model", $"{model?.ClassVM?.ClassName}{model?.FileName}ViewModel",model?.ClassVM?.IsList);
            #endregion

            #region Query class
            model.ResponseType = responseViewModel;//typeof(ResponseViewModel<>)?.MakeGenericType(getViewModel);
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
            model?.HandlerVM?.ConstructorParamTypes?.Add(iUnitOfWorkVM);//typeof(IUnitOfWorkVM<>).MakeGenericType(getViewModel)
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

        //public static async void RunCommandCode()
        //{
        //    var collection = new List<GeneratorViewModel>
        //    {
        //        new GeneratorViewModel
        //        {
        //            //
        //            Assembly = Assembly.GetEntryAssembly().GetName().Name,
        //            Namespace = "Application.Features.Departments",
        //            //
        //            FileName = $"Department",
        //            FolderName = "Features\\Departments\\",
        //            //
        //            HasHandler = true,
        //            HasCommand = true,
        //            CommandOrQueryVM = new CommandOrQueryGeneratorViewModel
        //            {
        //                UsingDirectives = new List<Assembly>
        //            {
        //               typeof(ResponseViewModel<>).Assembly,
        //               typeof(IMediator).Assembly, // MediatR
        //               typeof(IRequest).Assembly,
        //            },
        //                UsingDirectivesNames = new List<string>
        //            {
        //               typeof(ResponseViewModel<>).Namespace,
        //               typeof(IMediator).Namespace,
        //            },
        //                HasActionVM = true,
        //                PropsNames = new List<string>
        //            {
        //                "public string Name { get; set; }",
        //                "public string NameAr { get; set; }",
        //                "public DateTime DateCreated { get; set; }"
        //            }
        //            },
        //            HandlerVM = new HandlerGeneratorViewModel
        //            {
        //                ConstructorParamTypes = new List<Type>
        //            {
        //               typeof(ICoreUnitOfWork),
        //               typeof(IMapper)
        //            },
        //                UsingDirectives = new List<Assembly>
        //            {
        //                typeof(ResponseViewModel<>).Assembly,
        //                typeof(CancellationToken).Assembly,
        //                typeof(IUnitOfWorkVM<>).Assembly,
        //                typeof(ICoreUnitOfWork).Assembly,
        //                typeof(IMediator).Assembly, // MediatR
        //                typeof(IRequest).Assembly,
        //                typeof(IMapper).Assembly, // AutoMapper // MyProject.Services
        //                typeof(FeedBackCode).Assembly,
        //                typeof(Department).Assembly,

        //            },
        //                UsingDirectivesNames = new List<string>
        //            {
        //                typeof(ResponseViewModel<>).Namespace,
        //                typeof(CancellationToken).Namespace,
        //                typeof(IUnitOfWorkVM<>).Namespace,
        //                typeof(ICoreUnitOfWork).Namespace,
        //                typeof(IMediator).Namespace,
        //                typeof(IMapper).Namespace, // AutoMapper // MyProject.Services
        //                typeof(FeedBackCode).Namespace,
        //                typeof(Department).Namespace,
        //            },
        //                OprationType = OprationType.Command,
        //                CommandType = CommandType.Create,
        //            },
        //            HasValidator = true,
        //            ValidatorVM = new ValidatorGeneratorViewModel
        //            {
        //                RuleProps = new List<string>
        //            {
        //                "RuleFor(c => c.Name)",
        //                "RuleFor(c => c.NameAr)"
        //            },
        //                UsingDirectives = new List<Assembly>
        //            {
        //                typeof(AbstractValidator<>).Assembly, // MyProject.Data
        //            },
        //                UsingDirectivesName = new List<string>
        //            {
        //                typeof(AbstractValidator<>).Namespace, // MyProject.Data
        //            },
        //            },
        //            ClassVM = new ClassViewModel
        //            {
        //                ClassName = "Detail",
        //                UsingDirectives = new List<Assembly>
        //                {
        //                },
        //                UsingDirectivesName = new List<string>
        //                {
        //                },
        //                PropsNames = new List<string>()
        //            {
        //                "public int Id { get; set; }",
        //                "public string Name { get; set; }",
        //                "public DateTime DateCreated { get; set; }"
        //            }
        //        } 
        //        },
        //        new GeneratorViewModel
        //        {
        //            //
        //            Assembly = Assembly.GetEntryAssembly().GetName().Name,
        //            Namespace = "Application.Features.Departments",
        //            //
        //            FileName = $"Department",
        //            FolderName = "Features\\Departments\\",
        //            //
        //            HasHandler = true,
        //            HasCommand = true,
        //            CommandOrQueryVM = new CommandOrQueryGeneratorViewModel
        //            {
        //                UsingDirectives = new List<Assembly>
        //            {
        //               typeof(ResponseViewModel<>).Assembly,
        //               typeof(IMediator).Assembly, // MediatR
        //               typeof(IRequest).Assembly,
        //            },
        //                UsingDirectivesNames = new List<string>
        //            {
        //               typeof(ResponseViewModel<>).Namespace,
        //               typeof(IMediator).Namespace,
        //            },
        //                HasActionVM = true,
        //                PropsNames = new List<string>
        //            {
        //                "public int Id { get; set; }",
        //                "public string Name { get; set; }",
        //                "public string NameAr { get; set; }",
        //                "public DateTime DateCreated { get; set; }"
        //            }
        //            },
        //            HandlerVM = new HandlerGeneratorViewModel
        //            {
        //                ConstructorParamTypes = new List<Type>
        //            {
        //               typeof(ICoreUnitOfWork),
        //               typeof(IMapper)
        //            },
        //                UsingDirectives = new List<Assembly>
        //            {
        //                typeof(ResponseViewModel<>).Assembly,
        //                typeof(CancellationToken).Assembly,
        //                typeof(IUnitOfWorkVM<>).Assembly,
        //                typeof(ICoreUnitOfWork).Assembly,
        //                typeof(IMediator).Assembly, // MediatR
        //                typeof(IRequest).Assembly,
        //                typeof(IMapper).Assembly, // AutoMapper // MyProject.Services
        //                typeof(FeedBackCode).Assembly,
        //                typeof(Department).Assembly,
        //            },
        //                UsingDirectivesNames = new List<string>
        //            {
        //                typeof(ResponseViewModel<>).Namespace,
        //                typeof(CancellationToken).Namespace,
        //                typeof(IUnitOfWorkVM<>).Namespace,
        //                typeof(ICoreUnitOfWork).Namespace,
        //                typeof(IMediator).Namespace,
        //                typeof(IMapper).Namespace, // AutoMapper // MyProject.Services
        //                typeof(FeedBackCode).Namespace,
        //                typeof(Department).Namespace,
        //            },
        //                OprationType = OprationType.Command,
        //                CommandType = CommandType.Update,
        //            },
        //            HasValidator = true,
        //            ValidatorVM = new ValidatorGeneratorViewModel
        //            {
        //                RuleProps = new List<string>
        //            {
        //                "RuleFor(c => c.Name)",
        //                "RuleFor(c => c.NameAr)"
        //            },
        //                UsingDirectives = new List<Assembly>
        //            {
        //                typeof(AbstractValidator<>).Assembly, // MyProject.Data
        //            },
        //                UsingDirectivesName = new List<string>
        //            {
        //                typeof(AbstractValidator<>).Namespace, // MyProject.Data
        //            },
        //            },
        //            ClassVM = new ClassViewModel
        //            {
        //                ClassName = "Detail",
        //                UsingDirectives = new List<Assembly>
        //                {
        //                },
        //                UsingDirectivesName = new List<string>
        //                {
        //                },
        //                PropsNames = new List<string>()
        //            {
        //                "public int Id { get; set; }",
        //                "public string Name { get; set; }",
        //                "public DateTime DateCreated { get; set; }"
        //            }
        //            }
        //        }
        //    };
        //    foreach (var item in collection)
        //    {
        //        await GenerateAndWriteToCommandFeatureFile(item);
        //    }
        //}

        //public static async void RunQueryCode()
        //{
        //    var collection = new List<GeneratorViewModel>
        //    {
        //        new GeneratorViewModel
        //        {
        //            //
        //            Assembly = Assembly.GetEntryAssembly().GetName().Name,
        //            Namespace = "Application.Features.Departments",
        //            //
        //            FileName = $"Department",
        //            FolderName = "Features\\Departments\\",
        //            //
        //            HasQuery = true,
        //            CommandOrQueryVM = new CommandOrQueryGeneratorViewModel
        //            {
        //                UsingDirectives = new List<Assembly>
        //                {
        //                   typeof(ResponseViewModel<>).Assembly,
        //                   typeof(IMediator).Assembly, // MediatR
        //                   typeof(IRequest).Assembly,
        //                },
        //                UsingDirectivesNames = new List<string>
        //                {
        //                   typeof(ResponseViewModel<>).Namespace,
        //                   typeof(IMediator).Namespace,
        //                },
        //                PropsNames = new List<string>
        //                {
        //                    "public int? Id  { get; set; }"
        //                }
        //            },
        //            HandlerVM = new HandlerGeneratorViewModel
        //            {
        //                ConstructorParamTypes = new List<Type>
        //                {
        //                   typeof(ICoreUnitOfWork),
        //                   typeof(IMapper)
        //                },
        //                UsingDirectives = new List<Assembly>
        //                {
        //                    typeof(ResponseViewModel<>).Assembly,
        //                    typeof(CancellationToken).Assembly,
        //                    typeof(IUnitOfWorkVM<>).Assembly,
        //                    typeof(ICoreUnitOfWork).Assembly,
        //                    typeof(IMediator).Assembly, // MediatR
        //                    typeof(IRequest).Assembly,
        //                    typeof(IMapper).Assembly, // AutoMapper // MyProject.Services
        //                    typeof(FeedBackCode).Assembly,
        //                    typeof(Department).Assembly,
        //                    typeof(ActionStatus).Assembly,
        //                },
        //                UsingDirectivesNames = new List<string>
        //                {
        //                    typeof(ResponseViewModel<>).Namespace,
        //                    typeof(CancellationToken).Namespace,
        //                    typeof(IUnitOfWorkVM<>).Namespace,
        //                    typeof(ICoreUnitOfWork).Namespace,
        //                    typeof(IMediator).Namespace,
        //                    typeof(IMapper).Namespace, // AutoMapper // MyProject.Services
        //                    typeof(FeedBackCode).Namespace,
        //                    typeof(Department).Namespace,
        //                    "static " + typeof(ActionStatus).Namespace + ".Type",
        //                },
        //                HasIncludesNames = false,
        //                IncludesNames = "",
        //                OprationType = OprationType.Query,
        //                QueryType = QueryType.Detail,
        //            },
        //            HasValidator = true,
        //            ValidatorVM = new ValidatorGeneratorViewModel
        //            {
        //                RuleProps = new List<string>
        //                {
        //                    "RuleFor(c => c.Id)"
        //                },
        //                    UsingDirectives = new List<Assembly>
        //                {
        //                    typeof(AbstractValidator<>).Assembly, // MyProject.Data
        //                },
        //                    UsingDirectivesName = new List<string>
        //                {
        //                    typeof(AbstractValidator<>).Namespace, // MyProject.Data
        //                }
        //            },
        //            ClassVM = new ClassViewModel
        //            {
        //                IsList = true,
        //                ClassName =  QueryType.Detail.ToString(),
        //                UsingDirectives = new List<Assembly>
        //                {
        //                },
        //                UsingDirectivesName = new List<string>
        //                {
        //                },
        //                PropsNames = new List<string>()
        //                {
        //                            "public int Id { get; set; }",
        //                            "public string? Code { get; set; }",
        //                            "public string? Name { get; set; }",
        //                            "public string? NameAr { get; set; }",

        //                            "public string? Description { get; set; }",
        //                            "public string? DescriptionAr { get; set; }",

        //                            "public string? Address { get; set; }",
        //                            "public string? AddressAr { get; set; }",


        //                            "public decimal? Longitude { get; set; }",
        //                            "public decimal? Latitude { get; set; }",

        //                            "public string? LogoUrl { get; set; }",

        //                            "public string? ShjSysCode { get; set; }",

        //                            "public string? PhoneNumber { get; set; }",
        //                            "public string? Email { get; set; }",

        //                            "public string? WebSite { get; set; }",

        //                            "public string? Facebook { get; set; }",

        //                            "public string? Twitter { get; set; }",

        //                            "public string? Instagram { get; set; }",
        
        //                            "public string? Linkedin { get; set; }",

        //                            "public DateTime CreatedDate { get; set; }",
        //                            "public DateTime UpdatedDate { get; set; }"
        //                }
        //            }
        //        },
        //        new GeneratorViewModel
        //        {
        //            //
        //            Assembly = Assembly.GetEntryAssembly().GetName().Name,
        //            Namespace = "Application.Features.Departments",
        //            //
        //            FileName = $"Department",
        //            FolderName = "Features\\Departments\\",
        //            //
        //            HasQuery = true,
        //            CommandOrQueryVM = new CommandOrQueryGeneratorViewModel
        //            {
        //                UsingDirectives = new List<Assembly>
        //                {
        //                   typeof(ResponseViewModel<>).Assembly,
        //                   typeof(IMediator).Assembly, // MediatR
        //                   typeof(IRequest).Assembly,
        //                },
        //                UsingDirectivesNames = new List<string>
        //                {
        //                   typeof(ResponseViewModel<>).Namespace,
        //                   typeof(IMediator).Namespace,
        //                },
        //                PropsNames = new List<string>
        //                {
        //                    "public int PageIndex { get; set; }",
        //                    "public int PageSize { get; set; }",
        //                }
        //            },
        //            HandlerVM = new HandlerGeneratorViewModel
        //            {
        //                ConstructorParamTypes = new List<Type>
        //                {
        //                   typeof(ICoreUnitOfWork),
        //                   typeof(IMapper)
        //                },
        //                UsingDirectives = new List<Assembly>
        //                {
        //                    typeof(ResponseViewModel<>).Assembly,
        //                    typeof(CancellationToken).Assembly,
        //                    typeof(IUnitOfWorkVM<>).Assembly,
        //                    typeof(ICoreUnitOfWork).Assembly,
        //                    typeof(IMediator).Assembly, // MediatR
        //                    typeof(IRequest).Assembly,
        //                    typeof(IMapper).Assembly, // AutoMapper // MyProject.Services
        //                    typeof(FeedBackCode).Assembly,
        //                    typeof(Department).Assembly,
        //                    typeof(ActionStatus).Assembly,
        //                },
        //                UsingDirectivesNames = new List<string>
        //                {
        //                    typeof(ResponseViewModel<>).Namespace,
        //                    typeof(CancellationToken).Namespace,
        //                    typeof(IUnitOfWorkVM<>).Namespace,
        //                    typeof(ICoreUnitOfWork).Namespace,
        //                    typeof(IMediator).Namespace,
        //                    typeof(IMapper).Namespace, // AutoMapper // MyProject.Services
        //                    typeof(FeedBackCode).Namespace,
        //                    typeof(Department).Namespace,
        //                    "static " + typeof(ActionStatus).Namespace + ".Type",
        //                },
        //                HasIncludesNames = false,
        //                IncludesNames = "",
        //                OprationType = OprationType.Query,
        //                QueryType = QueryType.List,
        //            },
        //            HasValidator = true,
        //            ValidatorVM = new ValidatorGeneratorViewModel
        //            {
        //                RuleProps = new List<string>
        //                {
        //                    "RuleFor(c => c.PageIndex)",
        //                    "RuleFor(c => c.PageSize)"
        //                },
        //                    UsingDirectives = new List<Assembly>
        //                {
        //                    typeof(AbstractValidator<>).Assembly, // MyProject.Data
        //                },
        //                    UsingDirectivesName = new List<string>
        //                {
        //                    typeof(AbstractValidator<>).Namespace, // MyProject.Data
        //                }
        //            },
        //            ClassVM = new ClassViewModel
        //            {
        //                IsList = true,
        //                ClassName = QueryType.List.ToString(),
        //                UsingDirectives = new List<Assembly>
        //                {
        //                },
        //                UsingDirectivesName = new List<string>
        //                {
        //                },
        //                PropsNames = new List<string>()
        //                {
        //                            "public int Id { get; set; }",
        //                            "public string? Code { get; set; }",
        //                            "public string? Name { get; set; }",
        //                            "public string? NameAr { get; set; }",

        //                            "public string? Description { get; set; }",
        //                            "public string? DescriptionAr { get; set; }",

        //                            "public string? Address { get; set; }",
        //                            "public string? AddressAr { get; set; }",


        //                            "public decimal? Longitude { get; set; }",
        //                            "public decimal? Latitude { get; set; }",

        //                            "public string? LogoUrl { get; set; }",

        //                            "public string? ShjSysCode { get; set; }",

        //                            "public string? PhoneNumber { get; set; }",
        //                            "public string? Email { get; set; }",

        //                            "public string? WebSite { get; set; }",

        //                            "public string? Facebook { get; set; }",

        //                            "public string? Twitter { get; set; }",

        //                            "public string? Instagram { get; set; }",

        //                            "public string? Linkedin { get; set; }",

        //                            "public DateTime CreatedDate { get; set; }",
        //                            "public DateTime UpdatedDate { get; set; }"
        //                }
        //            }
        //        },
        //        new GeneratorViewModel
        //        {
        //            //
        //            Assembly = Assembly.GetEntryAssembly().GetName().Name,
        //            Namespace = "Application.Features.Departments",
        //            //
        //            FileName = $"Department",
        //            FolderName = "Features\\Departments\\",
        //            //
        //            HasQuery = true,
        //            CommandOrQueryVM = new CommandOrQueryGeneratorViewModel
        //            {
        //                UsingDirectives = new List<Assembly>
        //                {
        //                   typeof(ResponseViewModel<>).Assembly,
        //                   typeof(IMediator).Assembly, // MediatR
        //                   typeof(IRequest).Assembly,
        //                   typeof(ActionStatus).Assembly,
        //                },
        //                UsingDirectivesNames = new List<string>
        //                {
        //                   typeof(ResponseViewModel<>).Namespace,
        //                   typeof(IMediator).Namespace,
        //                   "static " + typeof(ActionStatus).Namespace + ".Type",
        //                },
        //                PropsNames = new List<string>
        //                {
        //                    "public ActionStatus ActionStatusId { get; set; }",
        //                    "public int PageIndex { get; set; }",
        //                    "public int PageSize { get; set; }",
        //                }
        //            },
        //            HandlerVM = new HandlerGeneratorViewModel
        //            {
        //                ConstructorParamTypes = new List<Type>
        //                {
        //                   typeof(ICoreUnitOfWork),
        //                   typeof(IMapper)
        //                },
        //                UsingDirectives = new List<Assembly>
        //                {
        //                    typeof(ResponseViewModel<>).Assembly,
        //                    typeof(CancellationToken).Assembly,
        //                    typeof(IUnitOfWorkVM<>).Assembly,
        //                    typeof(ICoreUnitOfWork).Assembly,
        //                    typeof(IMediator).Assembly, // MediatR
        //                    typeof(IRequest).Assembly,
        //                    typeof(IMapper).Assembly, // AutoMapper // MyProject.Services
        //                    typeof(FeedBackCode).Assembly,
        //                    typeof(Department).Assembly,
        //                    typeof(ActionStatus).Assembly,
        //                },
        //                UsingDirectivesNames = new List<string>
        //                {
        //                    typeof(ResponseViewModel<>).Namespace,
        //                    typeof(CancellationToken).Namespace,
        //                    typeof(IUnitOfWorkVM<>).Namespace,
        //                    typeof(ICoreUnitOfWork).Namespace,
        //                    typeof(IMediator).Namespace,
        //                    typeof(IMapper).Namespace, // AutoMapper // MyProject.Services
        //                    typeof(FeedBackCode).Namespace,
        //                    typeof(Department).Namespace,
        //                    "static " + typeof(ActionStatus).Namespace + ".Type",
        //                },
        //                HasIncludesNames = false,
        //                IncludesNames = ", x => x.Department, x => x.DepartmentSector",
        //                OprationType = OprationType.Query,
        //                QueryType = QueryType.ActionStatusList,
        //            },
        //            HasValidator = true,
        //            ValidatorVM = new ValidatorGeneratorViewModel
        //            {
        //                RuleProps = new List<string>
        //                {
        //                    "RuleFor(c => c.ActionStatusId)",
        //                    "RuleFor(c => c.PageIndex)",
        //                    "RuleFor(c => c.PageSize)"
        //                },
        //                    UsingDirectives = new List<Assembly>
        //                {
        //                    typeof(AbstractValidator<>).Assembly, // MyProject.Data
        //                },
        //                    UsingDirectivesName = new List<string>
        //                {
        //                    typeof(AbstractValidator<>).Namespace, // MyProject.Data
        //                }
        //            },
        //            ClassVM = new ClassViewModel
        //            {
        //                IsList = true,
        //                ClassName = QueryType.ActionStatusList.ToString(),
        //                UsingDirectives = new List<Assembly>
        //                {
        //                },
        //                UsingDirectivesName = new List<string>
        //                {
        //                },
        //                PropsNames = new List<string>()
        //                {
        //                            "public int Id { get; set; }",
        //                            "public string? Code { get; set; }",
        //                            "public string? Name { get; set; }",
        //                            "public string? NameAr { get; set; }",

        //                            "public string? Description { get; set; }",
        //                            "public string? DescriptionAr { get; set; }",

        //                            "public string? Address { get; set; }",
        //                            "public string? AddressAr { get; set; }",


        //                            "public decimal? Longitude { get; set; }",
        //                            "public decimal? Latitude { get; set; }",

        //                            "public string? LogoUrl { get; set; }",

        //                            "public string? ShjSysCode { get; set; }",

        //                            "public string? PhoneNumber { get; set; }",
        //                            "public string? Email { get; set; }",

        //                            "public string? WebSite { get; set; }",

        //                            "public string? Facebook { get; set; }",

        //                            "public string? Twitter { get; set; }",

        //                            "public string? Instagram { get; set; }",

        //                            "public string? Linkedin { get; set; }",

        //                            "public DateTime CreatedDate { get; set; }",
        //                            "public DateTime UpdatedDate { get; set; }"
        //                }
        //            }
        //        }
        //    };
        //    foreach (var item in collection)
        //    {
        //        await GenerateAndWriteToQueryFeatureFile(item);
        //    }
        //}
    }
}
