using DataAccessLayer;
using DataAccessLayer.Entities;
using Services.SourceFiles.Dto;
using System.Configuration;

namespace Services.SourceFiles
{
    public class SourceFilesService: DisposableService
    {
        private FilesMonitorDbContext _db;

        public SourceFilesService()
        {
            _db = new FilesMonitorDbContext(ConfigurationManager.ConnectionStrings[nameof(FilesMonitorDbContext)].ConnectionString);
        }

        public SourceFilesService(string connectionString)
        {
            _db = new FilesMonitorDbContext(connectionString);
        }

        public List<SourceFileDto> _GetFiles()
        {
            var query = _db.SourceFiles.Select(s => new SourceFileDto
            {
                Id = s.Id,
                Path = s.Path
            });

            return query.ToList();
        }

        public List<SourceFileDto> GetFiles()
        {
            var entities = _db.SourceFiles.ToList();
            var result = new List<SourceFileDto>();

            foreach (var entity in entities)
            {
                var dto = new SourceFileDto
                {
                    Id = entity.Id,
                    Path = entity.Path
                };
                result.Add(dto);
            }
            return result;
        }

        public void Add(string path)
        {
            _db.SourceFiles.Add(new SourceFile
            {
                Path = path
            });
            _db.SaveChanges();
        }

        public void Remove(IEnumerable<int> ids)
        {
            foreach(var id in ids)
            {
                _db.SourceFiles.Remove(new SourceFile
                {
                    Id = id
                });
            }
            _db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                _db.Dispose();
            }                

            base.Dispose(disposing);
        }
    }
}
