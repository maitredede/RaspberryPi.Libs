using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaspberryPi.LibNFC.WebReader
{
    public interface INfcReaderHubClient
    {
        void setVersion(string version);
        void cardOn();
        void cardOff();
    }

    public interface INfcReaderHubServer
    {
    }

    public class NfcReaderHub : Hub<INfcReaderHubClient>, INfcReaderHubServer
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            this.Clients.Client(this.Context.ConnectionId).setVersion(NfcContext.Version);
        }
    }
}
