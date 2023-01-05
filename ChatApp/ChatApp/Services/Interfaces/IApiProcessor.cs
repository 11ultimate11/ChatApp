using ChatApp.Models;
using System.Threading.Tasks;

namespace ChatApp.Services.Interfaces
{
    public interface IApiProcessor
    {
        Task<bool> DeleteAsync(int id, string route);
        Task<string> GetStringAsync(string route);
        Task<string> GetStringAsync(object obj, string route);
        Task<string> LogInAsync(object obj, string route);
        Task<bool> PostAsync(object model, string route);
        Task<string> GetToken(CredentialsModel model);
        Task<bool> PutAsync(object model, string route);
    }
}