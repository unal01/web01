using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace CoreBuilder.SignalR
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", user, message);
        }

        public async Task SendToTenant(Guid tenantId, string message)
        {
            await Clients.Group(tenantId.ToString()).SendAsync("ReceiveNotification", message);
        }

        public async Task JoinTenantGroup(Guid tenantId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, tenantId.ToString());
        }

        public async Task LeaveTenantGroup(Guid tenantId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, tenantId.ToString());
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
