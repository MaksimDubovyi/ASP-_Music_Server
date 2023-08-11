using System.ComponentModel.DataAnnotations;
namespace ASP__Music_Server.Models.UserModel
{
    public class UserModel
    {
        public string? ConnectionId { get; set; }
        public string? Login { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string PasswordHash { get; set; } = null!;

    }
}
