using ChatAppAPI.Helpers;
using GhostLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml;

namespace ChatAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Jobs : ControllerBase
    {
        private readonly IJwtService _jwt;
        private readonly ApplicationDbContext _db;
        public Jobs(IJwtService _jwt, ApplicationDbContext _db)
        {
            this._jwt = _jwt;
            this._db = _db;
        }
        [AllowAnonymous, HttpGet("GetJobs")]
        public async Task<IActionResult> GetJobs()
        {
            var result = await _db.JobModels.ToListAsync();
            return Ok(result);
        }
        [Authorize,HttpPost("AddJob")]
        public async Task<IActionResult> AddJob([FromBody] JobModel model)
        {
            if (HttpContext.User.Identity is not ClaimsIdentity claims ||
                _jwt.Credentials(claims) is not CredentialsModel credentials) return BadRequest();
            model.CreatedDate= DateTime.Now;
            await _db.JobModels.AddAsync(model);
            await _db.SaveChangesAsync();
            return model.ID > 0 ? Ok(model.ID) : BadRequest();
        }
        [Authorize,HttpPut("UpdateJob")]
        public async Task<IActionResult> UpdateJob([FromBody] JobModel model)
        {
            var job = await _db.JobModels.FindAsync(model.ID);
            if(job == null) return BadRequest();
            var properties = model.GetType().GetProperties();
            foreach (var property in properties)
            {
                property.SetValue(job, property.GetValue(model));
            }
            await _db.SaveChangesAsync();
            return Ok(true);
        }
        [Authorize,HttpDelete("DeleteJob/{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _db.JobModels.FindAsync(id);
            if(job == null) return BadRequest();
            _db.JobModels.Remove(job);
            await _db.SaveChangesAsync();
            return Ok(true);
        }
    }
}
