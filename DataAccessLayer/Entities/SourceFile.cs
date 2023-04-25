using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    [Table("source_file")]
    public class SourceFile
    {
        public int Id { get; set; }
        public string Path { get; set; }
    }
}
