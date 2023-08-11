using ASP__Music_Server.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using ASP__Music_Server.Models.UserModel;
using ASP__Music_Server.Repository;
using System.Net.Mail;
using System.Net;

namespace ASP__Music_Server.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository_User _repository_user;
        IWebHostEnvironment _appEnvironment;
        private readonly IConfiguration _configuration;
        public UserController(IRepository_User repository_user, IWebHostEnvironment appEnvironment, IConfiguration configuration)
        {
            _repository_user = repository_user;
            _appEnvironment = appEnvironment;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Create([FromForm] IFormFile? uploadedFile, [FromForm] UserModel user)
        {
            
            User user_ = await _repository_user.CheckLogin(user.Login);
            if (user_ != null)
                return BadRequest("Логін вже використовується");
            user_ = await _repository_user.CheckEmail(user.Email);
            if (user_ != null)
                return BadRequest("Email вже використовується");

            await _repository_user.AddUser(uploadedFile, user);
            return Ok("Вітаємо з успішною реєстрацією!");
        }

        [HttpPost("login")]
        public async Task <IActionResult> Login([FromQuery]string login, string password)
        {
         
            User user = await _repository_user.CheckLogin(login);
            
            if (user == null)
            {
                return BadRequest("Невірний логін або пароль!");
            }
            String derivedKey = _repository_user.CheckPassword(password, user.Id.ToString());

            if (derivedKey == user.PasswordHash)
                return Ok(user);

            return BadRequest("Невірний логін або пароль!");
        }

        [HttpPost("updateavatar")]
        public async Task<IActionResult> UpdateAvatar([FromForm] IFormFile? uploadedFile, [FromQuery] string id)
        {
            User user_ = await _repository_user.GetUser(id);
            if (user_ == null)
                return BadRequest("505");
           string path= await _repository_user.UpdateAvatar(uploadedFile, user_);
            return Ok(path);
        }

        [HttpPost("sms")]
        public async Task<IActionResult> SenPassword([FromQuery] string to)
        {
            User user = await _repository_user.CheckEmail(to);
            if (user == null)
            {
                return BadRequest("Невірна Email!");
            }

            string newPassword = await _repository_user.GeneratePassword(user);

            var gmailConfig = _configuration.GetSection("Gmail");
            String key = gmailConfig.GetValue<String>("Key")!;
            String host = gmailConfig.GetValue<String>("Host")!;
            String box = gmailConfig.GetValue<String>("Box")!;
            int port = gmailConfig.GetValue<int>("Port");
            bool ssl = gmailConfig.GetValue<bool>("Ssl");

            ///
            using SmtpClient smtpClient = new(host)
            {
                Port = port,
                EnableSsl = ssl,
                Credentials = new NetworkCredential(box, key)

            };

            String message;
            try
            {

                MailMessage mailMessage = new()
                {
                    IsBodyHtml = true,
                    From = new MailAddress(box),
                    Subject = "Відновлення паролю!",
                    Body = $"<h2>Ваш пароль</h2>" +
                    $"<p><i style='color:green'>{newPassword}</i></p><hr/>" +
                    $"<a href='#unsubscribe' style='color:red'>Відмовитись від розсилки</a>"
                };
                mailMessage.To.Add(new MailAddress(to));
                mailMessage.Attachments.Add(new Attachment("wwwroot/avatar/37VA.png"));
                smtpClient.Send(mailMessage);
                return Ok("Надіслано успішно!");
            }
            catch (Exception ex) { message = $"Помилка {ex.Message}"; }
            return BadRequest("Невірний Email!");
        }

        [HttpPost("updatepassword")]
        public async Task<IActionResult> UpdatePassword([FromQuery] string Password, [FromQuery] string idUser)
        {
            User user = await _repository_user.GetUser(idUser);
            if (user == null)
            {
                return BadRequest("Невірна Email!");
            }
            try
            {
                await _repository_user.UpdatePassword(Password, user);
                return Ok("Змінено!");
            }
            catch (Exception ex) {}
            return BadRequest("Невірний Email!");
        }

        [HttpGet("mymusic")]
        public async Task<IActionResult> MyMusic([FromQuery] string id ,[FromQuery] int _perPage, [FromQuery] int currentPage = 0)
        {
            return new ObjectResult(await _repository_user.GetMyMusicList(id, _perPage, currentPage));
        }

        [HttpGet("addmymusic")]
        public async Task AddMyMusic([FromQuery] string idUser, [FromQuery] string idMusic)
        {
            await _repository_user.AddMyMusic(idUser, idMusic);
        }

        [HttpGet("deletemymusic")]
        public async Task<IActionResult> DeleteMyMusic([FromQuery] string idUser, [FromQuery] string idMusic, [FromQuery] int _perPage, [FromQuery] int currentPage = 0)
        {
            return new ObjectResult(await _repository_user.DeleteMyMusic(idUser, idMusic,  _perPage,  currentPage));
        }
        
    }
}
