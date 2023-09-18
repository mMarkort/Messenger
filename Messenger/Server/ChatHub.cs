using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Server
{
    class ChatHub
    {
        private readonly HubConnection _connecttion;
        public event Action<string> MessageReceived;
        public ChatHub(HubConnection connecttion)
        {
            _connecttion = connecttion;
            _connecttion.On<string>("Someone to send", (MessageString)=>MessageReceived?.Invoke(MessageString));
        }
        public async Task Connect()
        {
            await _connecttion.StartAsync();
        }
        public async Task SendMessage(string Message)
        {
            await _connecttion.SendAsync("SendMessage",Message);
        }
    }
}
