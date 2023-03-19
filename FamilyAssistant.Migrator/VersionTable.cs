using FluentMigrator.Runner.VersionTableInfo;

namespace FamilyAssistant.Migrator;

public class VersionTable : IVersionTableMetaData
{
    public object ApplicationContext { get; set; }

    public bool OwnsSchema => true;

    public string SchemaName => "public";

    public string TableName => "version_info";

    public string ColumnName => "version";

    public string DescriptionColumnName => "description";

    public string UniqueIndexName => "uc_version";

    public string AppliedOnColumnName => "applied_on";
}
