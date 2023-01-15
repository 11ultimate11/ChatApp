using ChatAppAPI.Helpers;
using GhostLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChatAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Extra : ControllerBase
    {
        private readonly IJwtService _jwt;
        private readonly ApplicationDbContext _db;
        public Extra(IJwtService _jwt, ApplicationDbContext _db)
        {
            this._jwt = _jwt;
            this._db = _db;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetLastZettel()
        {
            try
            {
                var zettel = await _db.ZettelModels.FirstOrDefaultAsync();

                return zettel is not null ? Ok(zettel) : BadRequest();
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }
            return BadRequest();
        }
        [Authorize, HttpPut("UpdateZettel")]
        public async Task<IActionResult> UpdateZettel([FromBody] ZettelModel model)
        {
            if (HttpContext.User.Identity is not ClaimsIdentity claims ||
                _jwt.Credentials(claims) is not CredentialsModel credentials) return BadRequest("bad credentials/token");
            var zettel = _db.ZettelModels.FirstOrDefault();
            if (zettel is not null)
            {
                zettel.Title = model.Title;
                zettel.Content = model.Content;
                await _db.SaveChangesAsync();
                return Ok(true);
            }
            else
            {
                await _db.ZettelModels.AddAsync(model);
                await _db.SaveChangesAsync();
                return Ok(true);
            }
        }
        [Authorize, HttpDelete("DeleteZettel/{id}")]
        public async Task<IActionResult> DeleteZettel()
        {
            if (HttpContext.User.Identity is not ClaimsIdentity claims ||
                _jwt.Credentials(claims) is not CredentialsModel credentials) return BadRequest();
            var model = _db.ZettelModels.FirstOrDefault();
            if (model != null)
            {
                _db.ZettelModels.Remove(model);
            }

            await _db.SaveChangesAsync();
            return Ok(true);
        }
        [AllowAnonymous]
        [HttpGet("GetAllNews")]
        public async Task<IActionResult> GetAllNews()
        {
            var result = await _db.NewsModels.Include(x=> x.Media).ToListAsync();
            return Ok(result);
        }
        [Authorize, HttpPost("AddNews")]
        public IActionResult AddNews([FromBody] NewsModel model)
        {
            if (HttpContext.User.Identity is not ClaimsIdentity claims ||
                _jwt.Credentials(claims) is not CredentialsModel credentials) return BadRequest("bad credentials/token");
            try
            {
                _db.NewsModels.Add(model);
                _db.SaveChanges();
                return Ok(model.ID);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Authorize, HttpDelete("DeleteNews/{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            if (HttpContext.User.Identity is not ClaimsIdentity claims ||
                _jwt.Credentials(claims) is not CredentialsModel credentials) return BadRequest("bad credentials/token");
            var result = await _db.NewsModels.FindAsync(id);
            if (result is not null)
            {
                _db.NewsModels.Remove(result);
                await _db.SaveChangesAsync();
                return Ok(true);
            }
            return BadRequest(false);
        }
    }
}
