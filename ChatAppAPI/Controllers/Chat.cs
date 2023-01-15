using ChatApp.Services;
using ChatAppAPI.Helpers;
using GhostLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Security.Claims;
using System.Text.Json.Nodes;

namespace ChatAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Chat : ControllerBase
    {
        private readonly IJwtService _jwt;
        private readonly ApplicationDbContext _db;
        public Chat(IJwtService _jwt, ApplicationDbContext _db)
        {
            this._jwt = _jwt;
            this._db = _db;
        }
        [Authorize, HttpGet("Getchats/{count}")]
        public async Task<IActionResult> GetChats(int count)
        {
            if (HttpContext.User.Identity is not ClaimsIdentity claims ||
                _jwt.Credentials(claims) is not CredentialsModel credentials) return BadRequest();
            var models = await _db.ChatModels.Where(x=> x.OwnerID == credentials.PersonID || x.TargetID == credentials.PersonID).OrderByDescending(x=> x.ID).ToListAsync();
            return models.Count >= count ? Ok(models.Skip(count)) : Ok(Enumerable.Empty<ChatModel>());
        }
        [Authorize, HttpPost]
        public async Task<IActionResult> AddNewChat([FromBody] ChatModel model)
        {
            if (HttpContext.User.Identity is not ClaimsIdentity claims ||
                _jwt.Credentials(claims) is not CredentialsModel credentials) return BadRequest();
            var chat = await _db.ChatModels.Where(x => (x.OwnerID == model.ID && x.TargetID == credentials.PersonID) || (x.TargetID == model.ID && x.OwnerID == credentials.PersonID)).FirstOrDefaultAsync();
            if (chat is not null) return BadRequest("already exists");
            else
            {
                await _db.ChatModels.AddAsync(model);

                await _db.SaveChangesAsync();
                return Ok(model.ID);

            }
        }
        [Authorize, HttpPost("AddChatMsj")]
        public async Task<IActionResult> AddNewMessage([FromBody] ChatMessageModel model)
        {
            if (HttpContext.User.Identity is not ClaimsIdentity claims ||
                _jwt.Credentials(claims) is not CredentialsModel credentials
                || credentials.PersonID != model.PersonID) return BadRequest();
            var chat = await _db.ChatModels.FindAsync(model.ReferenceID);
            if (chat!=null)
            {
                chat.MsjCount++;
                chat.LastMessage = model.Content;
            }
            await _db.ChatMsjModels.AddAsync(model);
            await _db.SaveChangesAsync();
            return Ok(model.ID);
        }
        [Authorize, HttpGet("GetMessages/{id}/{count}")]
        public async Task<IActionResult> GetMessages(int id , int count)
        {
            if (HttpContext.User.Identity is not ClaimsIdentity claims ||
                _jwt.Credentials(claims) is not CredentialsModel credentials) return BadRequest();
            var models = await _db.ChatMsjModels.Where(x=> x.ReferenceID == id).OrderByDescending(x=> x.ID).ToListAsync();
            if (models is null) return BadRequest("no messages");
            return models.Count >= count ? Ok(models.Skip(count).Take(20)) : BadRequest("no more");
        }
        [Authorize, HttpGet("GetCounts")]
        public async Task<IActionResult> GetCounts()
        {
            if (HttpContext.User.Identity is not ClaimsIdentity claims ||
                _jwt.Credentials(claims) is not CredentialsModel credentials) return BadRequest();
            var chats = await _db.ChatModels.Where(x => x.OwnerID == credentials.PersonID || x.TargetID == credentials.PersonID).ToListAsync();
            var msjs = await _db.ChatMsjModels.Where(x => x.PersonID == credentials.PersonID).ToListAsync();
            JsonObject json = new()
            {
                {"chats" , chats.Count },
                {"msjs" , msjs.Count },
            };
            return Ok(json);
        }
    }
}
