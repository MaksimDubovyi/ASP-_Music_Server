using ASP__Music_Server.Models;
using ASP__Music_Server.Models.Data.Entity;

namespace ASP__Music_Server.Repository
{
    public interface IRepository_Music
    {
        Task<Music> GetMusic(Guid id);
        Task<ListMusicAndCountPages> GetMusicList(int _perPage, int currentPage);
        Task ClearingAllMusic();
        Task<Music> AddMusic(MusicModel music);
        Task<ListMusicAndCountPages> GetSongsByGenre(string genre_, int _perPage, int currentPage);
        Task<ListMusicAndCountPages> AddRatingMusic(string idUser, string idMusic, int _perPage, int currentPage);
        Task<ListMusicAndCountPages> DeleteRatingMusic(string idUser, string idMusic, int _perPage, int currentPage);
    }
    
}
