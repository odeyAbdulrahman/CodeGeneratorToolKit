using System.Reflection;
using CodeGeneratorToolKit.FeaturesGenerator.Enumerations;

namespace CodeGeneratorToolKit.FeaturesGenerator.ViewModels
{
    public class GeneratorViewModel
    {
        //Shard Props
        public string? Assembly { get; set; }
        public string? Namespace { get; set; }
        public Type? ResponseType { get; set; }
        //File Props
        public string? FileName { get; set; }
        public string? FolderName { get; set; }
        //Command Prps
        public bool? HasCommand { get; set; }
        public bool? HasQuery { get; set; }
        public CommandOrQueryGeneratorViewModel? CommandOrQueryVM { get; set; }
        //Handler Prps
        public bool? HasHandler { get; set; }
        public HandlerGeneratorViewModel? HandlerVM { get; set; }
        //Validator Prps
        public bool? HasValidator { get; set; }
        public ValidatorGeneratorViewModel? ValidatorVM { get; set; }
        //Class Prps
        public bool? HasViewModel { get; set; }
        public ClassViewModel? ClassVM { get; set; }
    }

    public class CommandOrQueryGeneratorViewModel
    {
        public bool? HasActionVM { get; set; }
        public List<string>? PropsNames { get; set; }
        public List<Assembly>? UsingDirectives { get; set; }
        public List<string>? UsingDirectivesNames { get; set; }
    }
    public class HandlerGeneratorViewModel
    {
        public OprationType? OprationType { get; set; }
        public CommandType? CommandType { get; set; }
        public QueryType? QueryType { get; set; }
        public Type? RequestType { get; set; }
        public List<Type>? ConstructorParamTypes { get; set; }
        public List<Assembly>? UsingDirectives { get; set; }
        public List<string>? UsingDirectivesNames { get; set; }
        public bool HasIncludesNames { get; set; }
        public string? IncludesNames { get; set; }
    }
    public class ValidatorGeneratorViewModel
    {
        public Type? CommandOrQueryType { get; set; }
        public List<string>? RuleProps { get; set; }
        public List<Assembly>? UsingDirectives { get; set; }
        public List<string>? UsingDirectivesName { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ClassViewModel
    {
        public bool? IsList { get; set; }
        public string? ClassName { get; set; }
        public List<string>? PropsNames { get; set; }
        public List<Assembly>? UsingDirectives { get; set; }
        public List<string>? UsingDirectivesName { get; set; }
    }
}
