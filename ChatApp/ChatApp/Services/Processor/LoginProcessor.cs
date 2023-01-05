using ChatApp.Models;
using ChatApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using ChatApp.Services.Global;

namespace ChatApp.Services.Processor
{
    public class LoginProcessor : ILoginProcessor
    {
        private readonly IApiProcessor api;
        const string _ApiRouteReg = "Account/Register";
        public LoginProcessor()
        {
            api = DependencyService.Get<IApiProcessor>(DependencyFetchTarget.GlobalInstance);
        }
        /// <summary>
        /// Diese Methode nimmt die parameters <paramref name="username"/> und <paramref name="password"/> und überpruft ob die credentials valid sind
        /// <br/>
        /// Diese Methode hat als Rückgabe true wenn erfolgreich und false falls nicht erfolgreich. Falls true JWT wird in Appsettings gespeichert
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> Login(string username, string password)
        {
            var result = await api.LogInAsync(new CredentialsModel() { Username = username, Password = password }, "Account/LogIn");
            if (!string.IsNullOrEmpty(result))
            {
                JSONNode node = JSON.Parse(result);
                if (node is null || node["person"].Value is null || node["token"].Value is null) return false;
                AppSettings.Person = JsonConvert.DeserializeObject<PersonModel>(node["person"].Value);
                AppSettings.APItoken = node["token"].Value;
                Task.Run(async ()=> await DependencyService.Get<IServerProcessor>(DependencyFetchTarget.GlobalInstance).RegisterToBroadcast());
                return true;
            }
            return false;
        }
        /// <summary>
        /// Try to register async , true if success , false if its fails
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> RegisterAsync(PersonModel model)
        {
            
            var result = await api.PostAsync(model, _ApiRouteReg);
            if (result)
            {
                var token = await api.GetToken(model.Credentials);
                AppSettings.Person = model;
                if (!string.IsNullOrEmpty(token))
                {
                    AppSettings.APItoken= token;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
