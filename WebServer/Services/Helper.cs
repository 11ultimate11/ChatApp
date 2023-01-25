
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using GhostLibrary.Services.Interfaces;
using GhostLibrary.Models;

namespace WebServer.Services
{
    public static class Helper
    {

        public static string GetImgFromBytes(byte[] bytes)
        {
            if (bytes is null || bytes.Length <= 0) return "";
            return @"data:image/jpeg;base64,"+ Convert.ToBase64String(bytes);
        }
        public static async Task<PersonModel> DownloadPerson(int id, IApiProcessor _api, string _token)
        {
            var p = await _api.GetStringAsync($"Account/GetAccount/{id}", _token);
            var model = JsonConvert.DeserializeObject<PersonModel>(p);
            return model!;
        }
    }
    
}
