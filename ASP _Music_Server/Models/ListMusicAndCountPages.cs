using ASP__Music_Server.Models.Data.Entity;

namespace ASP__Music_Server.Models
{
    public class ListMusicAndCountPages
    {
        private List<Music> musics = new List<Music>();
        private int totalзage;

        public int totalPage { get => totalзage; set => totalзage = value; }

        public List<Music> Musics { get => musics; set => musics = value; }

    }
}
