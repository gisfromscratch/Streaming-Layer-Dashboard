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

            GraphicsThresold = 5;
        }

        /// <summary>
        /// The thresold for adding graphics delivered by stream messages to this layer.
        /// If this value is set to one, every new obtained graphic is added to this layer immediately.
        /// </summary>
        public int GraphicsThresold { get; set; }

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
            var bufferSegment = new ArraySegment<byte>(buffer);
            while (WebSocketState.Open == _socket.State)
            {
                var messageTask = await _socket.ReceiveAsync(bufferSegment, cancellationToken);
                switch (messageTask.MessageType)
                {
                    case WebSocketMessageType.Close:
                        break;
                }

                var messageSize = messageTask.Count;
                while (!messageTask.EndOfMessage)
                {
                    if (buffer.Length <= messageSize)
                    {
                        // TODO: Handle Invalid Payload
                        await _socket.CloseAsync(WebSocketCloseStatus.InvalidPayloadData, @"Buffer exceeded!", CancellationToken.None);
                        return;
                    }

                    bufferSegment = new ArraySegment<byte>(buffer, messageSize, buffer.Length - messageSize);
                    messageTask = await _socket.ReceiveAsync(bufferSegment, cancellationToken);
                    messageSize += messageTask.Count;
                }

                // TODO: Analyze message
                var receivedBytes = bufferSegment.Skip<byte>(bufferSegment.Offset).Take<byte>(messageSize).ToArray<byte>();
                graphicsBuilder.Visit(new StreamMessage(receivedBytes));
                if (GraphicsThresold <= graphicsBuilder.Graphics.Count)
                {
                    AddGraphicsFromBuilder(graphicsBuilder);
                }
            }

            AddGraphicsFromBuilder(graphicsBuilder);
        }

        /// <summary>
        /// Adds the obtained graphics to this layer
        /// </summary>
        /// <param name="graphicsBuilder">The builder for the graphics.</param>
        private void AddGraphicsFromBuilder(GraphicsFromStreamBuilder graphicsBuilder)
        {
            var newGraphics = graphicsBuilder.Graphics;
            Graphics.AddRange(newGraphics);
            newGraphics.Clear();
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
