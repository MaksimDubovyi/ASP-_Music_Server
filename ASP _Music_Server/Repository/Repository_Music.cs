using Microsoft.EntityFrameworkCore;
using ASP__Music_Server.Models;
using ASP__Music_Server.Data;
using ASP__Music_Server.Models.Data.Entity;
using ASP__Music_Server.Data.Entity;


namespace ASP__Music_Server.Repository
{
    public class Repository_Music : IRepository_Music
    {
        private readonly DataContext _context;
        readonly IWebHostEnvironment _appEnvironment;
        public Repository_Music(DataContext context, IWebHostEnvironment appEnvironment = null)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }


        public async Task<ListMusicAndCountPages> GetMusicList(int _perPage, int currentPage)
        {

            if (currentPage == 1|| currentPage == 0)
            {
                currentPage = 0;
                currentPage *= _perPage;
            }
            else
            {
                currentPage *= _perPage;
                currentPage -= _perPage;
            }
            int count = _context.Musics.Count();

            int perPage = (int)Math.Ceiling((double)count / _perPage);
            var temp = await _context.Musics.Skip(currentPage).Take(_perPage).ToListAsync();
            ListMusicAndCountPages result = new ();
            result.totalPage = perPage;
            result.Musics= temp;
            return result;  
        }
  
    
        public async Task<Music> GetMusic(Guid id)
        {
            return await _context.Musics.FirstOrDefaultAsync(m => m.id == id);

        }


        public async Task<ListMusicAndCountPages> GetSongsByGenre(string genre_, int _perPage, int currentPage)
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
            int count = _context.Musics.Count();

            int perPage = (int)Math.Ceiling((double)count / _perPage);
            var temp = await _context.Musics.Where(music => music.Genre == genre_).Skip(currentPage).Take(_perPage).ToListAsync();
            ListMusicAndCountPages result = new();
            result.totalPage = perPage;
            result.Musics = temp;
            return result;
        }

        public async Task<Music> AddMusic(MusicModel requestData)
        {
            try
            {
                
                IFormFile uploadedFile = requestData.uploadedFile;
                // Создаем папку для сохранения файлов, если она не существует
                string folderPath = Path.Combine(_appEnvironment.WebRootPath, "mp3");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string path = "/mp3/" + uploadedFile.FileName; // имя файла
       
                string mp3_path = DataContext.WebServerAPI+"/mp3/" + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream); // копируем файл в поток  
                }

                Music music = new Music();


                music.Size = (uploadedFile.Length / 1000000).ToString() + "Mb";
                music.Path = mp3_path;           // Сохраняем полный путь в базе данных
                music.Executor = requestData.executor;
                music.Name = requestData.titel;
                music.GPA = 0;
                music.Genre=requestData.genre;
                music.id_User = requestData.id_User;

                _context.Add(music);
                await _context.SaveChangesAsync();
                return music;
            }
            catch { }
            return null;
        }


        public async Task ClearingAllMusic()
        {
            // Получаем все записи из таблицы Musics
            var allMusics = _context.Musics.ToList();

            // Удаляем каждую запись
            foreach (var music in allMusics)
            {
                string fileName = Path.GetFileName(music.Path);                                  // Парсим URL - адресу, щоб отримати тільки ім'я файлу (останній сегмент URL-адреси)
                string filePath = Path.Combine(_appEnvironment.WebRootPath, "mp3", fileName);  // Отримуємо шлях до файлу на сервері

                if (File.Exists(filePath))  // Проверяем, существует ли файл
                {
                    File.Delete(filePath);  // Видаляємо файл
                    _context.Musics.Remove(music);
                    _context.SaveChanges();
                }
            }

        }

        public async Task<ListMusicAndCountPages> AddRatingMusic(string _idUser, string _idMusic, int _perPage, int currentPage)
        {
            Guid idUser = Guid.Parse(_idUser);
            Guid idMusic = Guid.Parse(_idMusic);
            try
            {
                User us = await _context.Users.FindAsync(idUser);
                Music mc = await _context.Musics.FirstOrDefaultAsync(m => m.id == idMusic);

                float? _gpa = mc.GPA;
                RatingMusics _rm = new RatingMusics();
                if (us != null && mc != null)
                {
                    RatingMusics rm = await _context.RatingMusic.FirstOrDefaultAsync(p => p.UserId == us.Id && p.MusicId == mc.id);
                    if(rm == null)
                    {
                        _gpa++;
                        _rm.UserId = us.Id;
                        _rm.MusicId = mc.id;
                        _context.Add(_rm);
                        mc.GPA= _gpa;
                        _context.Update(mc);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return await GetMusicList(_perPage, currentPage);
        }

        public async Task<ListMusicAndCountPages> DeleteRatingMusic(string _idUser, string _idMusic, int _perPage, int currentPage)
        {
            Guid idUser = Guid.Parse(_idUser);
            Guid idMusic = Guid.Parse(_idMusic);
            try
            {
                User us = await _context.Users.FindAsync(idUser);
                Music mc = await _context.Musics.FirstOrDefaultAsync(m => m.id == idMusic);

                float? _gpa = mc.GPA;
                RatingMusics _rm = new RatingMusics();
                if (us != null && mc != null)
                {
                    RatingMusics rm = await _context.RatingMusic.FirstOrDefaultAsync(p => p.UserId == us.Id && p.MusicId == mc.id);
                    if (rm != null)
                    {
                        _gpa--;
                        _context.RatingMusic.Remove(rm);
                        mc.GPA = _gpa;
                        _context.Update(mc);
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return await GetMusicList(_perPage, currentPage);
        }

        public async Task<List<Music>> GetMyMusicList(string id)
        {
            Guid userId = Guid.Parse(id);

            return await (
                from userMusic in _context.UserMusics
                join music in _context.Musics on userMusic.MusicId equals music.id
                where userMusic.UserId == userId
                select music
            ).ToListAsync();

        }

    }

}
