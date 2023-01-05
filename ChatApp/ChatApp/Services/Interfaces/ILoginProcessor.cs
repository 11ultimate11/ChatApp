using ChatApp.Models;
using System.Threading.Tasks;

namespace ChatApp.Services.Interfaces
{
    public interface ILoginProcessor
    {
        /// <summary>
        /// Diese Methode nimmt die parameters <paramref name="username"/> und <paramref name="password"/> und überpruft ob die credentials valid sind
        /// <br/>
        /// Diese Methode hat als Rückgabe true wenn erfolgreich und false falls nicht erfolgreich. Falls true JWT wird in Appsettings gespeichert
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> Login(string username, string password);
        /// <summary>
        /// Try to register async , true if success , false if its fails
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> RegisterAsync(PersonModel model);
    }
}