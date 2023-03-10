


using Microsoft.AspNetCore.SignalR;
using System.Security.Cryptography.X509Certificates;

namespace WebServer.SRhub;

public class ChatHub : Hub
{
    public async Task BroadcastMessage(string json , string channel)
    {
        await Clients.Group(channel).SendAsync("Broadcast" , json);
        await Clients.Caller.SendAsync("Debug", $"{Context.ConnectionId} call on group {channel}");
    }
    public async Task Register(string channel)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, channel);
        await Clients.Caller.SendAsync("Debug", $"{Context.ConnectionId} register to {channel}");
    }
}
