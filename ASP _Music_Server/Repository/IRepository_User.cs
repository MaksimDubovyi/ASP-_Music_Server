using ASP__Music_Server.Data.Entity;
using ASP__Music_Server.Models;
using ASP__Music_Server.Models.Data.Entity;
using ASP__Music_Server.Models.UserModel;
using Microsoft.AspNetCore.Mvc;

namespace ASP__Music_Server.Repository
{
    public interface IRepository_User
    {
        Task<User> GetUser(string id);
        Task<User> CheckLogin(string login);
        string CheckPassword(string password, string userId);
        Task<User> CheckEmail(string email);
        Task AddUser(IFormFile? uploadedFile, UserModel user);
        Task<string> UpdateAvatar(IFormFile? uploadedFile,User user);
        Task<string> GeneratePassword(User user);
        Task UpdatePassword(string Password, User user);
        Task AddMyMusic(string idUser,  string idMusic);
        Task<ListMusicAndCountPages> DeleteMyMusic(string idUser, string idMusic, int _perPage, int currentPage);
        Task<ListMusicAndCountPages> GetMyMusicList(string id, int _perPage, int currentPage);
        
    }
    

}

