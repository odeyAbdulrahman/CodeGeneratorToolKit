using System.Text;

namespace CodeGeneratorToolKit.FeaturesGenerator.ViewModels
{
    public class TemplateViewModel
    {
        public StringBuilder? UsingDirectivesBuilder { get; set; }
        public StringBuilder? ConstructorPropsBuilder { get; set; }
        public StringBuilder? ConstructorParamsBuilder { get; set; }
        public StringBuilder? ConstructorInitsBuilder { get; set; }
        public string? RequestTypeName { get; set; }
        public string? ResponseTypeName { get; set; }
    }

    public class CommandTemplateViewModel
    {
        public StringBuilder? UsingDirectivesBuilder { get; set; }
        public StringBuilder? PropsBuilder { get; set; }
        public string? ResponseTypeName { get; set; }
    }

    public class ValidatorTemplateViewModel
    {
        public StringBuilder? UsingDirectivesBuilder { get; set; }
        public StringBuilder? RulePropsBuilder { get; set; }
        public string? CommandOrQueryTypeName { get; set; }
    }

    public class ClassTemplateViewModel
    {
        public StringBuilder? UsingDirectivesBuilder { get; set; }
        public StringBuilder? PropsBuilder { get; set; }
    }
}
