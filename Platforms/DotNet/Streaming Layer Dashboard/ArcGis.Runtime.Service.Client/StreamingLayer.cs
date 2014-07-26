using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Layers;

namespace ArcGis.Runtime.Service.Client
{
    /// <summary>
    /// Streaming Layer using WebSocket to connect with GeoEvent Processor.
    /// </summary>
    public class StreamingLayer : GraphicsLayer
    {
        private readonly ClientWebSocket _socket;

        private readonly Uri _url;

        public StreamingLayer(String url)
        {
            _socket = new ClientWebSocket();
            _url = new Uri(url);
        }

        /// <summary>
        /// Starts connecting to the Streaming Service,
        /// </summary>
        /// <param name="cancellationToken">The cancelation token for this request.</param>
        public async Task ConnectAsync(CancellationToken cancellationToken)
        {
            await _socket.ConnectAsync(_url, cancellationToken);

            var graphicsBuilder = new GraphicsFromStreamBuilder();

            const int ChunkSize = 1024;
            var buffer = new byte[ChunkSize];
            var ready = true;
            while (ready)
            {
                var bufferSegment = new ArraySegment<byte>(buffer);
                var messageTask = await _socket.ReceiveAsync(bufferSegment, cancellationToken);
                switch (messageTask.MessageType)
                {
                    case WebSocketMessageType.Close:
                        ready = false;
                        break;
                }

                var messageSize = messageTask.Count;
                while (!messageTask.EndOfMessage)
                {
                    if (buffer.Length <= messageSize)
                    {
                        // TODO: Handle Invalid Payload
                        await _socket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, "Buffer exceeded!", CancellationToken.None);
                        return;
                    }

                    bufferSegment = new ArraySegment<byte>(buffer, messageSize, buffer.Length - messageSize);
                    messageTask = await _socket.ReceiveAsync(bufferSegment, cancellationToken);
                    messageSize += messageTask.Count;
                }

                // TODO: Analyze message
                graphicsBuilder.Visit(new StreamMessage(buffer));
                //var messageAsUtf8 = Encoding.UTF8.GetString(buffer);
                //Trace.WriteLine(messageAsUtf8);
            }
        }

        /// <summary>
        /// Starts closing the connection to the Streaming Service.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token for this request.</param>
        public async void CloseAsync(CancellationToken cancellationToken)
        {
            // TODO: Status Description should be changed
            await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, @"OK", cancellationToken);
        }
    }
}
