using Microsoft.AspNetCore.Mvc;

namespace ASP__Music_Server.Models
{
    public class MusicModel
    {
        [FromForm]
        public Guid id_User { get; set; }
        [FromForm]
        public string titel { get; set; }
        [FromForm]
        public string executor { get; set; }
        [FromForm]
        public string genre { get; set; }
        [FromForm]
        public IFormFile uploadedFile { get; set; }

        [FromForm]
        public int perPage { get; set; }
        [FromForm]
        public int currentPage { get; set; } = 0;
    }

}
