namespace CodeGeneratorToolKit.FeaturesGenerator.Enumerations
{
    public enum OprationType
    {
        Command,
        Query
    }
    public enum CommandType
    {
        Active,
        Archive,
        Create,
        Delete,
        Recover,
        Restore,
        Update,
        Download,
        Upload,
        Location
    }
    public enum QueryType
    {
        Detail,
        List,
        ActiveList,
        ActionStatusList,
    }
}