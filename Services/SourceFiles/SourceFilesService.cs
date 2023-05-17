using DataAccessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Services.SourceFiles.Dto;
using System.Configuration;
using System.Linq.Expressions;

namespace Services.SourceFiles
{
    public class SourceFilesService: DisposableService
    {
        private readonly ISourceFileRepository _sourceFileRepository;

        public SourceFilesService(ISourceFileRepository sourceFileRepository)
        {
            _sourceFileRepository = sourceFileRepository;
        }

        public SourceFilesService(string connectionString)
        {
            throw new NotImplementedException();
        }

        public List<SourceFileDto> GetFiles()
        {
            var entities = _sourceFileRepository.GetMany(s => true);
            
            var result = entities
                .Select(MapEntityToDto)
                .ToList();

            return result;
        }

        public SourceFileDto Add(string path)
        {
            var entity = new SourceFile
            {
                Path = path
            };
            _sourceFileRepository.Add(entity);
            _sourceFileRepository.SaveChanges();

            return MapEntityToDto(entity);
        }

        public void Remove(IEnumerable<int> ids)
        {
            _sourceFileRepository.RemoveRange(
                ids
                .Select(
                    id => new SourceFile
                    {
                        Id = id
                    }
                )
                .ToList()
            );

            _sourceFileRepository.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                _sourceFileRepository.Dispose();
            }                

            base.Dispose(disposing);
        }

        private SourceFileDto MapEntityToDto(SourceFile entity)
        {
            return new SourceFileDto
            {
                Id = entity.Id,
                Path = entity.Path
            };
        }
    }
}
