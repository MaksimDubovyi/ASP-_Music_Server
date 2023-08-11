using ASP__Music_Server.Models;
using Microsoft.AspNetCore.Mvc;
using ASP__Music_Server.Models.Data.Entity;
using ASP__Music_Server.Repository;

namespace ASP__Music_Server.Controllers
{
    [Route("api/Music")]
    [ApiController]
    public class MusicController : ControllerBase
    {
   
        readonly IRepository_Music Repository;

        public MusicController(IRepository_Music repository, IWebHostEnvironment appEnvironment)
        {
            Repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]  int _perPage , [FromQuery]  int currentPage = 0)
        { 
            return new ObjectResult(await Repository.GetMusicList(_perPage, currentPage));
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetMusic(Guid id)
        {
            Music ms = await Repository.GetMusic(id);
            return new ObjectResult(ms);
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromForm] MusicModel requestData)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Music ms= await Repository.AddMusic(requestData);
                    return Ok(ms.id);
                }
                else
                    return BadRequest("Error");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("clearingallmusic")]
        public async Task<IActionResult> ClearingAllMusic()
        {
            await Repository.ClearingAllMusic();
            return new ObjectResult(await Repository.GetMusicList(7,0));
        }

        [HttpGet("addratingmusic")]
        public async Task<IActionResult> AddRatingMusic([FromQuery] string idUser, [FromQuery] string idMusic, [FromQuery] int _perPage, [FromQuery] int currentPage = 0)
        {
            return new ObjectResult(await Repository.AddRatingMusic(idUser, idMusic, _perPage, currentPage));
        }

        [HttpGet("deleteratingmusic")]
        public async Task<IActionResult> DeleteRatingMusic([FromQuery] string idUser, [FromQuery] string idMusic, [FromQuery] int _perPage, [FromQuery] int currentPage = 0)
        {
            return new ObjectResult(await Repository.DeleteRatingMusic(idUser, idMusic, _perPage, currentPage));
        }

        [HttpGet("getsongsbygenre")]
        public async Task<IActionResult> GetSongsByGenre([FromQuery] string genre,  [FromQuery] int _perPage, [FromQuery] int currentPage = 0)
        {
            return new ObjectResult(await Repository.GetSongsByGenre(genre, _perPage, currentPage));
        }

    }
}

