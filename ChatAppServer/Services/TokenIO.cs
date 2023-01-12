using Microsoft.JSInterop;

namespace ChatAppServer.Services
{
    public static class TokenIO
    {
       public static async Task SaveToken(string key , string value , IJSRuntime JS)
        {
            await JS.InvokeAsync<object>("WriteCookie.WriteCookie", key, value, DateTime.Now.AddDays(1));
        }
       public static async Task<string> ReadToken(string key , IJSRuntime JS)
        {
            var myCookieValue = await JS.InvokeAsync<string>("ReadCookie.ReadCookie", key);
            return myCookieValue;
        }
    }
}
