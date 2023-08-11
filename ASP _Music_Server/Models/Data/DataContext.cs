using ASP__Music_Server.Data.Entity;
using ASP__Music_Server.Models.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace ASP__Music_Server.Data
{
    public class DataContext: DbContext
    {
        //public static readonly String WebServerAPI = "https://localhost:7166";
        public static readonly String WebServerAPI = "https://musicserv.azurewebsites.net";
      
        //public static readonly String WebClientAPI = "http://localhost:8081";
        public static readonly String WebClientAPI = "https://musicsua.azurewebsites.net/";


        public static readonly String SchemaName = "MusicServer";


        public DbSet<User> Users { get; set; }
        public DbSet<Music> Musics { get; set; }
        public DbSet<UserMusic> UserMusics { get; set; }
        public DbSet<RatingMusics> RatingMusic { get; set; }
        
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //схема - як спосіб розділення проєктів в одній БД
            modelBuilder.HasDefaultSchema(SchemaName);

          //_____  Додавання usera в базу даних 
            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = Guid.Parse("fa2c02a0-a639-4c4f-8b96-b81946117d54"),
                    Name = "Root Administrator",
                    PasswordHash = "A826F31FEA8407572E533E55E9247B2BC348D372",
                    Avatar = null,
                    Login = "Admin",
                    Email = "maksim24du@gmail.com",
                    RegsterDt = DateTime.Now,
                });
        }
    }
}
