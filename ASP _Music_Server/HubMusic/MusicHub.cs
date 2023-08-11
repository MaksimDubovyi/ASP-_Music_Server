using Microsoft.AspNetCore.SignalR;
using ASP__Music_Server.Models.UserModel;
using ASP__Music_Server.Repository;
using ASP__Music_Server.Models;
using Microsoft.AspNetCore.Mvc;
using ASP__Music_Server.Models.Data.Entity;
using Newtonsoft.Json;


namespace ASP__Music_Server.HubMusic
{
    /*
    Ключевой сущностью в SignalR, через которую клиенты обмениваются сообщеними 
    с сервером и между собой, является хаб (hub). 
    Хаб представляет некоторый класс, который унаследован от абстрактного класса 
    Microsoft.AspNetCore.SignalR.Hub.
    */
    public class MusicHub : Hub
    {
        static List<string> Users = new ();

        public async Task AddMusic(string login, string id)
        {
            // Вызов метода AddMusic на всех клиентах
            await Clients.All.SendAsync("AddMusicClient", login,  id);
        }

        // Подключение нового пользователя
        public async Task Connect()
        {
            var id = Context.ConnectionId;

            if (!Users.Contains(id))
            {
                 Users.Add(id);
                // Вызов метода Connected только на текущем клиенте, который обратился к серверу
                await Clients.Caller.SendAsync("ConnectedR", id);
            }
        }

        // OnDisconnectedAsync срабатывает при отключении клиента.
        // В качестве параметра передается сообщение об ошибке, которая описывает,
        // почему произошло отключение.
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var item = Users.FirstOrDefault(x => x == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                // Вызов метода UserDisconnected на всех клиентах
                await Clients.All.SendAsync("UserDisconnected", id);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
