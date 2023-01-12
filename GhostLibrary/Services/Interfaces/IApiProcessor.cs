using GhostLibrary.Models;

namespace GhostLibrary.Services.Interfaces
{
    public interface IApiProcessor
    {
        Task<bool> DeleteAsync(int id, string route, string token);
        Task<string> GetStringAsync(string route, string token);
        Task<string> GetStringAsync(object obj, string route, string token);
        Task<string> LogInAsync(object obj, string route, string token);
        Task<bool> PostAsync(object model, string route, string token);
        Task<string> GetToken(CredentialsModel model, string token);
        Task<bool> PutAsync(object model, string route, string token);
    }
}