using ASP__Music_Server.Models.Data.Entity;

namespace ASP__Music_Server.Data.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Login { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Avatar { get; set; }
        public string PasswordHash { get; set; } = null!;
        public DateTime RegsterDt { get; set; }
        public DateTime? DeleteDt { get; set; }

    }
}
