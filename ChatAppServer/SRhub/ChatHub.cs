

using ChatAppServer.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace ChatAppServer.SRhub;

public class ChatHub : Hub
{
    public async Task BroadcastMessage(string json , string channel)
    {
        await Clients.Group(channel).SendAsync("Broadcast" , json);
        //await Clients.Caller.SendAsync("Debug", $"{Context.ConnectionId} call on group {channel}");
    }
    public async Task Register(string channel)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, channel);
        await Clients.Caller.SendAsync("Debug", $"{Context.ConnectionId} register to {channel}");
    }
}
