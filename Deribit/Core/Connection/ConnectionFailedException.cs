using System;
using System.Collections.Generic;
using System.Text;
using System.Net.WebSockets;

namespace Deribit.Core.Connection
{
    class ConnectionFailedException : Exception
    {
        public ConnectionFailedException(Uri server, WebSocketState state) : base($"Connection with {server.ToString()} failed. State: {state.ToString()}") 
        {
        
        }
    }
}
