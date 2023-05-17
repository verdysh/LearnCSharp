using DataAccessLayer;
using DataAccessLayer.Repositories;
using System.Configuration;

namespace Services.Helpers
{
    public class DbHelper
    {
        public static FilesMonitorDbContext CreateInstance()
        {
            return new FilesMonitorDbContext(ConfigurationManager.ConnectionStrings[nameof(FilesMonitorDbContext)].ConnectionString);
        }

        public static ISourceFileRepository CreateSourceFileRepositoryInstance()
        {
            return new SourceFileRepository(CreateInstance());
        }
    }
}
