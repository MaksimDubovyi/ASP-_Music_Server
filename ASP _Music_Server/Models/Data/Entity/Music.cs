using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace ASP__Music_Server.Models.Data.Entity
{
    public class Music
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public Guid id_User { get; set; }
        public string Name { get; set; }
        public string? Path { get; set; }
        public string? Executor { get; set; }
        public string Size { get; set; }
        public float? GPA { get; set; }
        public string? Genre { get; set; }
    }
}
