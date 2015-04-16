using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArcGis.Runtime.Service.Client
{
    /// <summary>
    /// Represents a WebSocket abstracting the Windows 8.x platform dependent .NET implementation.
    /// </summary>
    internal interface IWebSocket
    {
        Task ConnectAsync(Uri uri, CancellationToken cancellationToken);

        WebSocketState State { get; }

        //Task ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken);

        event EventHandler<ReceiveEventArgs> BytesReceived;

        Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken);
    }
}
