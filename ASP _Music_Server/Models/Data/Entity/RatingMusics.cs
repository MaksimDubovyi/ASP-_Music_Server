namespace ASP__Music_Server.Models.Data.Entity
{
    public class RatingMusics
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid MusicId { get; set; }
    }
}
