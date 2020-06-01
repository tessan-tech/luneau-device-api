using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DeviceApi
{
    public class Item
    {
        public int price { get; set; }
        public string name { get; set; }
    }

    public class ItemAddResult
    {
        public int result { get; set; }
        public int totalPrice { get; set; }
    }

    internal class DeviceHub : Hub
    {
        private readonly HubService HubService;
        public DeviceHub(HubService hubService)
        {
            HubService = hubService;
        }

        public ItemAddResult AddItem(Item item, int quantity)
        {
            return HubService.AddItem(item, quantity);
        }

        public override Task OnConnectedAsync()
        {
            HubService.RequestVideo();
            return base.OnConnectedAsync();
        }
    }
}
