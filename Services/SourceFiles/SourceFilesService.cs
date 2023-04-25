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

        private void EXAMPLE_ExtensionMethod()
        {
            /////////////////////////////////////////
            // Where(), Take(), Select() are static separate methods, but we can use them as if they are a part of some class
            // In this specific case they look like part of IQueryable

            var query1 = _db.SourceFiles.Where(s => s.Id > 5).Take(5).Select(s =>
                new SourceFileDto
                {
                    Id = s.Id,
                    Path = s.Path
                }
            );

            /////////////////////////////////////////
            // Before we have extensions methods in C# we could use these methods like this

            var whereClause = Queryable.Where(_db.SourceFiles, s => s.Id > 5);
            var take5Clause = Queryable.Take(whereClause, 5);
            var selectClause = Queryable.Select(take5Clause, s => new SourceFileDto
            {
                Id = s.Id,
                Path = s.Path
            });
            var query2 = selectClause;

            /////////////////////////////////////////
            // If we wanted to use this methods in one line without creation of additional variables

            var query3 =
                Queryable.Select(
                    Queryable.Take(
                        Queryable.Where(_db.SourceFiles, s => s.Id > 5),
                        5
                    ),
                s => new SourceFileDto
                {
                    Id = s.Id,
                    Path = s.Path
                }
            );
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
