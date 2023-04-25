using Microsoft.AspNetCore.Mvc;
using Services.SourceFiles;
using Services.SourceFiles.Dto;

namespace WebBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        [HttpGet]
        public List<SourceFileDto> GetFiles()
        {
            using var service = new SourceFilesService(@"Data Source=C:\Users\Volodymyr\FileMonitor\FMDB.sqlite");

            var files = service.GetFiles();

            return files;
        }
    }
}
