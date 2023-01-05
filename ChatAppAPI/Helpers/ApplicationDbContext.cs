using ChatAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAppAPI.Helpers
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<MediaModel> MediaModels { get; set; }
        public DbSet<PersonModel> PersonModels { get; set; }
        public DbSet<CredentialsModel> CredentialsModel { get; set; }
        public DbSet<ChatModel> ChatModels { get; set; }
        public DbSet<ChatMessageModel> ChatMsjModels { get; set; }
        public DbSet<ChatKeyModel> ChatKeyModels { get; set; }
    }
}
