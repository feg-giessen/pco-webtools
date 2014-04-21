using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace PcoWeb.Hubs
{
    public class LiveHub : Hub
    {
        public void Offset(int value)
        {
            this.Clients.AllExcept(this.Context.ConnectionId).offset(value);
        }
    }
}