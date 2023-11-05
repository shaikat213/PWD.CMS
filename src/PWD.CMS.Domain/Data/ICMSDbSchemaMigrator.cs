using System.Threading.Tasks;

namespace PWD.CMS.Data;

public interface ICMSDbSchemaMigrator
{
    Task MigrateAsync();
}
