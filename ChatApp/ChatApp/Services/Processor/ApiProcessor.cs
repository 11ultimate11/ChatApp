using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ChatApp.Models;
using ChatApp.Models.Abstract;
using ChatApp.Services.Global;
using ChatApp.Services.Interfaces;
using Newtonsoft.Json;

namespace ChatApp.Services.Processor
{
    public class ApiProcessor : IApiProcessor
    {
        const string _baseUrlAdress = "https://chatappapiflorin.azurewebsites.net/api/";
        public ApiProcessor()
        {
            
        }
        public async Task<string> GetStringAsync(string route)
        {
            var httpClient = new HttpClient();
            if (!CheckToken(httpClient)) return string.Empty;
            var result = await httpClient.GetAsync(new Uri($"{_baseUrlAdress}/{route}"));
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                httpClient.Dispose();
                return content;
            }
            httpClient.Dispose();
            return string.Empty;
        }
        public async Task<string> GetStringAsync(object obj , string route)
        {
            var httpClient = new HttpClient();
            if (!CheckToken(httpClient)) return string.Empty;
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                Content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"),
                RequestUri = new Uri($"{_baseUrlAdress}/{route}")
            };
            var result = await httpClient.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                httpClient.Dispose();
                return content;
            }
            httpClient.Dispose();
            return string.Empty;
        }
        public async Task<string> LogInAsync(object obj, string route)
        {
            var httpClient = new HttpClient();
            if (!CheckToken(httpClient)) return string.Empty;
            var data = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            Uri uri = new Uri($"{_baseUrlAdress}/{route}");
            httpClient.BaseAddress = uri;
            var result = await httpClient.PostAsync(uri , data);
            var content = await result.Content.ReadAsStringAsync();
            httpClient.Dispose();
            if (result.IsSuccessStatusCode)
            {
                return content;
            }
            else
            {
                Console.WriteLine("Get errror: " + content);
            }
            return string.Empty;
        }
        public async Task<bool> DeleteAsync(int id, string route)
        {
            var httpClient = new HttpClient();
            if (!CheckToken(httpClient)) return false;
            var result = await httpClient.DeleteAsync(new Uri($"{_baseUrlAdress}/{route}/{id}"));
            httpClient.Dispose();
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var parse = bool.TryParse(content, out bool parseResult);
                return parse && parseResult;
            }
            return false;
        }
        public async Task<bool> PostAsync(object model, string route)
        {
            var httpClient = new HttpClient();
            if (!CheckToken(httpClient)) return false;
            var data = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            Uri uri = new Uri($"{_baseUrlAdress}/{route}");
            httpClient.BaseAddress= uri;
            var result = await httpClient.PostAsync(uri, data);
            httpClient.Dispose();
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var parse = int.TryParse(content, out int id);
                if (parse && id > 0)
                {
                    var idProp = model.GetType().GetProperty(nameof(AbstractSqlModel.ID));
                    idProp?.SetValue(model, id);
                }
                return parse && id > 0;
            }
            else
            {
                var content = await result.Content.ReadAsStringAsync();
                Console.WriteLine("Post error: " + content);
            }
            return false;
        }
        public async Task<bool> PutAsync(object model, string route)
        {
            var httpClient = new HttpClient();
            if (!CheckToken(httpClient)) return false;
            var data = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            Uri uri = new Uri($"{_baseUrlAdress}/{route}");
            httpClient.BaseAddress= uri;
            var result = await httpClient.PutAsync(uri, data);
            httpClient.Dispose();
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var parse = bool.TryParse(content, out bool parseResult);
                return parse && parseResult;
            }
            else
            {
                var content = await result.Content.ReadAsStringAsync();
                Console.WriteLine("Post error: " + content);
            }
            return false;
        }
        public async Task<string> GetToken(CredentialsModel model)
        {
            var httpClient = new HttpClient();
            var data = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            Uri uri = new Uri($"{_baseUrlAdress}/Account/GetToken");
            httpClient.BaseAddress= uri;
            var result = await httpClient.PostAsync(uri, data);
            httpClient.Dispose();
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                var content = await result.Content.ReadAsStringAsync();
                Console.WriteLine("Post error: " + content);
            }
            return string.Empty;
        }
        private bool CheckToken(HttpClient httpClient)
        {
            if (AppSettings.Person is null) return true;
            if (httpClient.DefaultRequestHeaders.Authorization != null) return true;
            if (AppSettings.APItoken != null && httpClient.DefaultRequestHeaders.Authorization is null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AppSettings.APItoken);
                return true;
            }
            httpClient.Dispose();
            return false;
        }
    }
}
