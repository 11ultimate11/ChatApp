
using ChatApp.Models;
using ChatApp.Services.Global;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ChatApp.Services.Processor
{
    class ServerProcessor : IServerProcessor
    {
        private HubConnection hubConnection;
        public ServerProcessor()
        {
        }
        const string hubConnectionString = "https://testsignalrchat12311.azurewebsites.net/chatHub";
        public HubConnectionState HubConnectionState => hubConnection.State;
        public async Task InitHubConnection()
        {
            hubConnection = new HubConnectionBuilder().WithUrl(hubConnectionString).Build();
            await hubConnection.StartAsync();
            if (hubConnection.State != HubConnectionState.Connected)
            {
                App.Instance.DisplayToast("failed to connect to the server");
                return;
            }
            else
            {
                App.Instance.DisplayToast("Connected successfully to the server");
            }
            hubConnection.On<string>("Broadcast", json =>
            {
                var model = JsonConvert.DeserializeObject<ChatMessageModel>(json);
                InternalDelegates.CallOnBroadcast(model);
            });
            hubConnection.On<string>("Debug", msj =>
            {
                System.Console.WriteLine($"SIgnalr : {msj}");
            });
        }
        public async Task Broadcast(ChatMessageModel model, string channel)
        {
            await hubConnection.InvokeAsync("BroadcastMessage", JsonConvert.SerializeObject(model), channel);
        }
        public async Task RegisterToBroadcast()
        {
            await hubConnection.InvokeAsync("Register", $"Person{AppSettings.Person.ID}");
            //App.Instance.DisplayToast("Registered to broadcast");
        }
    }
}
