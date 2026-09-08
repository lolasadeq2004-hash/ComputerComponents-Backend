using Microsoft.AspNetCore.SignalR;

namespace ComputerComponents.API.Hubs
{
    public class ComponentHub : Hub
    {
        public async Task BroadcastComponentAdded()
        {
            await Clients.All.SendAsync("ComponentAdded");
        }

        public async Task BroadcastComponentUpdated()
        {
            await Clients.All.SendAsync("ComponentUpdated");
        }

        public async Task BroadcastComponentDeleted()
        {
            await Clients.All.SendAsync("ComponentDeleted");
        }
    }
}
