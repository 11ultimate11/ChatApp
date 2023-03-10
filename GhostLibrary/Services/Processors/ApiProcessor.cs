using GhostLibrary.Models;
using GhostLibrary.Models.Abstract;
using GhostLibrary.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace GhostLibrary.Services.Processor
{
    public class ApiProcessor : IApiProcessor
    {
        const string _baseUrlAdress = "https://ghostappapiflorin.azurewebsites.net/api/";
        public ApiProcessor()
        {

        }
        public async Task<string> GetStringAsync(string route, string token)
        {
            var httpClient = new HttpClient();
            AddHeader(httpClient, token);
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
        public async Task<string> GetStringAsync(object obj, string route, string token)
        {
            var httpClient = new HttpClient();
            AddHeader(httpClient, token);
            HttpRequestMessage request = new()
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
        public async Task<string> LogInAsync(object obj, string route, string token)
        {
            var httpClient = new HttpClient();
            AddHeader(httpClient, token);
            var data = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            Uri uri = new Uri($"{_baseUrlAdress}/{route}");
            httpClient.BaseAddress = uri;
            var result = await httpClient.PostAsync(uri, data);
            var content = await result.Content.ReadAsStringAsync();
            httpClient.Dispose();
            if (result.IsSuccessStatusCode)
            {
                return content;
            }
            else
            {
                //Console.WriteLine("Get errror: " + content);
            }
            return string.Empty;
        }
        public async Task<bool> DeleteAsync(int id, string route, string token)
        {
            var httpClient = new HttpClient();
            AddHeader(httpClient, token);
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
        public async Task<bool> PostAsync(object model, string route, string token)
        {
            var httpClient = new HttpClient();
            AddHeader(httpClient, token);
            var data = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            Uri uri = new Uri($"{_baseUrlAdress}{route}");
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
        public async Task<bool> PutAsync(object model, string route, string token)
        {
            var httpClient = new HttpClient();
            AddHeader(httpClient, token);
            var data = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            Uri uri = new Uri($"{_baseUrlAdress}{route}");
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
        public async Task<string> GetToken(CredentialsModel model, string token)
        {
            var httpClient = new HttpClient();
            AddHeader(httpClient, token);
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
        private void AddHeader(HttpClient client, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
