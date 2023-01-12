
using ChatApp.Services;
using ChatAppAPI.Helpers;
using ChatAppAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text.Json.Nodes;

namespace ChatAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Account : ControllerBase
    {
        private readonly IJwtService _jwt;
        private readonly ApplicationDbContext _db;
        public Account(IJwtService _jwt , ApplicationDbContext _db)
        {
            this._jwt = _jwt;
            this._db = _db;
        }
        [AllowAnonymous]
        [HttpPost("LogIn")]
        public IActionResult LogIn([FromBody] CredentialsModel model)
        {
            if (model is null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password)) return BadRequest("credentials not ok");
            try
            {
                var cred = _db.CredentialsModel.FirstOrDefault(x => x.Username!.Equals(model.Username) && x.Password == model.Password);
                if (cred is null) return BadRequest("invalid pass or username");
                var person = _db.PersonModels.FirstOrDefault(x=> x.CredentialsId == cred.ID);
                if (person is null) { return BadRequest("credentials not found"); }
                person.Media = _db.MediaModels.Find(person.MediaId);
                person.Credentials!.PersonID = person.ID;
                var token = _jwt.GenerateToken(person.Credentials!);
                JsonObject obj = new ()
                {
                    { "person" , JsonConvert.SerializeObject(person) },
                    { "token" , token },
                };
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpGet("LogInWithToken/{token}")]
        public IActionResult LogInWithToken()
        {
            if (HttpContext.User.Identity is not ClaimsIdentity claims ||
                _jwt.Credentials(claims) is not CredentialsModel credentials) return BadRequest();
            try
            {
                var cred = _db.CredentialsModel.FirstOrDefault(x => x.Username!.Equals(credentials.Username) && x.Password == credentials.Password);
                if (cred is null) return BadRequest("invalid pass or username");
                var person = _db.PersonModels.FirstOrDefault(x => x.CredentialsId == cred.ID);
                if (person is null) { return BadRequest("credentials not found"); }
                person.Media = _db.MediaModels.Find(person.MediaId);
                person.Credentials!.PersonID = person.ID;
                var token = _jwt.GenerateToken(person.Credentials!);
                JsonObject obj = new()
                {
                    { "person" , JsonConvert.SerializeObject(person) },
                    { "token" , token },
                };
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] PersonModel model)
        {
            if (model is null || model.Credentials is null || string.IsNullOrEmpty(model.Credentials.Username) || string.IsNullOrEmpty(model.Credentials.Password)) return BadRequest();
            try
            {
                var cred = _db.CredentialsModel.FirstOrDefault(x=> x.Username!.Equals(model.Credentials.Username));
                if (cred is not null) { return BadRequest("credentials already exist"); }
                await _db.PersonModels.AddAsync(model);
                await _db.SaveChangesAsync();
                return Ok(model.ID);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        [AllowAnonymous]
        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] CredentialsModel model)
        {
            if (model is null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password)) return BadRequest();
            try
            {
                var cred = await _db.CredentialsModel.FirstOrDefaultAsync(x => x.Username!.Equals(model.Username) && x.Password!.Equals(model.Password));
                if (cred is null) { return BadRequest("credentials not found"); }
                var person = await _db.PersonModels.FirstOrDefaultAsync(x => x.CredentialsId == cred.ID);
                if (person is null) { return BadRequest("person not found"); }
                cred.PersonID = person.ID;
                var token = _jwt.GenerateToken(cred);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Authorize , HttpGet("GetAllAccount")]
        public async Task<IActionResult> GetAllAccounts()
        {
            if (HttpContext.User.Identity is not ClaimsIdentity claims ||
                _jwt.Credentials(claims) is not CredentialsModel credentials) return BadRequest();
            var accounts = await _db.PersonModels.Include(a=> a.Credentials).Include(a=> a.Media).Where(x=> x.Credentials!.Username != credentials.Username).ToListAsync();
            return Ok(accounts);
        }
        [Authorize, HttpGet("GetAccount/{id}")]
        public async Task<IActionResult> GetAccount(int id)
        {
            if (HttpContext.User.Identity is not ClaimsIdentity claims ||
                _jwt.Credentials(claims) is not CredentialsModel credentials) return BadRequest();
            var account = await _db.PersonModels.Include(a => a.Credentials).Include(a => a.Media).FirstOrDefaultAsync(x=> x.ID == id);
            return Ok(account);
        }
        [Authorize, HttpPut("UpdatePerson")]
        public async Task<IActionResult> UpdatePerson([FromBody] PersonModel model)
        {
            if (HttpContext.User.Identity is not ClaimsIdentity claims ||
                _jwt.Credentials(claims) is not CredentialsModel credentials
                || credentials.PersonID != model.ID) return BadRequest();
            var media = await _db.MediaModels.FindAsync(model.MediaId);
            media.Key = model.Media.Key;
            media.Data = model.Media.Data;
            await _db.SaveChangesAsync();
            return Ok(true);
        }
        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult GEt()
        {
            return Ok("ok");
        }
    }
}
