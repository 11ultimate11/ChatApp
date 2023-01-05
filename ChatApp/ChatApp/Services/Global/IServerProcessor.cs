using ChatApp.Models;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace ChatApp.Services.Processor
{
    internal interface IServerProcessor
    {
        Task Broadcast(ChatMessageModel model, string channel);
        Task InitHubConnection();
        Task RegisterToBroadcast();
        HubConnectionState HubConnectionState { get; }
    }
}