using Microsoft.EntityFrameworkCore;
using ASP__Music_Server.Models;
using ASP__Music_Server.Data.Entity;
using ASP__Music_Server.Data;
using ASP__Music_Server.Models.UserModel;
using ASP__Music_Server.Services.Kdf;
using ASP__Music_Server.Models.Data.Entity;


namespace ASP__Music_Server.Repository
{
    public class Repository_User : IRepository_User
    {
        private readonly IKdfServise _kdfService;
        private readonly DataContext _context;
        readonly IWebHostEnvironment _appEnvironment;
        public Repository_User(DataContext context, IWebHostEnvironment appEnvironment, IKdfServise kdfService)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _kdfService = kdfService;
        }

  
        public async Task<User> GetUser(string id)
        {
            Guid userId = Guid.Parse(id);
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User> CheckLogin(string login)
        {
            return await  _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        }

        public string CheckPassword(string password, string userId)
        {
            return _kdfService.GetDerivedKey(password, userId);
           
        }

        public async Task<User> CheckEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.Email == email);
        }
        
        public async Task<string> UpdateAvatar(IFormFile? uploadedFile, User user)
        {

            if (user.Avatar != null)
            {
                string fileName = Path.GetFileName(user.Avatar);                                  // Парсим URL - адресу, щоб отримати тільки ім'я файлу (останній сегмент URL-адреси)
                string filePath = Path.Combine(_appEnvironment.WebRootPath, "avatar", fileName);  // Отримуємо шлях до файлу на сервері

                if (File.Exists(filePath))  // Проверяем, существует ли файл
                {
                    File.Delete(filePath);  // Видаляємо файл
                }
            }
            string Avatar_path=null;
            if (uploadedFile != null)
                if (uploadedFile.ContentType.StartsWith("image/"))                                               //Перевіряємо чи користувач вибрав картинку
                {
                    string path = "/avatar/" + uploadedFile.FileName;                                            // имя файла                               
                    Avatar_path = DataContext.WebServerAPI + "/avatar/" + uploadedFile.FileName;
                    using var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create);
                    await uploadedFile.CopyToAsync(fileStream);                                                  // копіюємо файл у потік                      
                    user.Avatar = Avatar_path;                                                                  
                }

            _context.Update(user);
            await _context.SaveChangesAsync();
            return Avatar_path;
        }

        public async Task AddMyMusic(string _idUser, string _idMusic)
        {
            Guid idUser = Guid.Parse(_idUser);
            Guid idMusic = Guid.Parse(_idMusic);
            try
            {
                User us = await _context.Users.FindAsync(idUser);
                Music mList = await _context.Musics.FirstOrDefaultAsync(m => m.id == idMusic);

                UserMusic um = new ();
                if (us != null && mList != null)
                {
                    um.UserId = us.Id;
                    um.MusicId = mList.id;
                    _context.Add(um);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<ListMusicAndCountPages> GetMyMusicList(string id, int _perPage, int currentPage)
        {
            if (currentPage == 1 || currentPage == 0)
            {
                currentPage = 0;
                currentPage *= _perPage;
            }
            else
            {
                currentPage *= _perPage;
                currentPage -= _perPage;
            }
            Guid userId = Guid.Parse(id);

            List<Music> lm = new();
            lm= await (
                from userMusic in _context.UserMusics
                join music in _context.Musics on userMusic.MusicId equals music.id
                where userMusic.UserId == userId
                select music
            ).ToListAsync();

 
            int _totalPage = (int)Math.Ceiling((double)lm.Count / _perPage);
 

            ListMusicAndCountPages result = new();
            result.totalPage = _totalPage;
            result.Musics = lm.Skip(currentPage).Take(_perPage).ToList(); 

            return result;

        }
        
        public async Task AddUser(IFormFile? uploadedFile, UserModel user)
        {
            User user1 = new();
            if (uploadedFile!=null)
            if (uploadedFile.ContentType.StartsWith("image/"))                                               //Перевіряємо чи користувач вибрав картинку
            {
                string path = "/avatar/" + uploadedFile.FileName; // имя файла                               
                string Avatar_path = DataContext.WebServerAPI + "/avatar/" + uploadedFile.FileName;
                using var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create);
                await uploadedFile.CopyToAsync(fileStream);                                                  // копируем файл в поток                       
                user1.Avatar = Avatar_path;                                                                  //
            }                                                                                                
                                                                                                             
            user1.Id = Guid.NewGuid();                                                                       //
            user1.Name = user.Name;                                                                          //
            user1.Email = user.Email;                                                                        //
            user1.Login = user.Login;                                                                        //Створюємо нового користувача
            user1.PasswordHash = _kdfService.GetDerivedKey(user.PasswordHash, user1.Id.ToString());           //


            _context.Add(user1);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GeneratePassword( User user)
        {                                                                                           //
            Random random = new Random();                                                              // Створюємо новий пароль із 6 символів
            int randomNumber = random.Next(100000, 999999);                                         //
            user.PasswordHash = randomNumber.ToString();                                            //
            user.PasswordHash = _kdfService.GetDerivedKey(user.PasswordHash, user.Id.ToString());   // Хешуємо пароль

            _context.Update(user);                      // Оновлюємо в БД
            await _context.SaveChangesAsync();          // 
            return randomNumber.ToString();             // Повертаємо новий пароль для відправки на пошту користовачу 
        }

        public async Task UpdatePassword(string Password, User user)
        {
            user.PasswordHash = _kdfService.GetDerivedKey(Password, user.Id.ToString());           
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<ListMusicAndCountPages> DeleteMyMusic(string _idUser, string _idMusic, int _perPage, int currentPage)
        {
            Guid idUser = Guid.Parse(_idUser);
            Guid idMusic = Guid.Parse(_idMusic);
            try
            {
                User us = await _context.Users.FindAsync(idUser);
                Music mc = await _context.Musics.FirstOrDefaultAsync(m => m.id == idMusic);

                if (mc != null && us != null)
                {
                    UserMusic um = await _context.UserMusics.FirstOrDefaultAsync(p => p.UserId == us.Id && p.MusicId == mc.id);
                    _context.UserMusics.Remove(um);
                    await _context.SaveChangesAsync();
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return await GetMyMusicList(_idUser,  _perPage, currentPage);
        }
        
    }

}
