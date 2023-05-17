using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public interface ISourceFileRepository: IRepository<SourceFile>
    { }

    public class SourceFileRepository : RepositoryBase<SourceFile>, ISourceFileRepository
    {
        public SourceFileRepository(FilesMonitorDbContext db) : base(db)
        { }
    }
}
