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
        private readonly IWebSocket _socket;

        public StreamingLayer(String url)
        {
            _socket = new WebSocketFactory().CreateWebSocket();
            Url = new Uri(url);

            GraphicsThresold = 5;
        }

        /// <summary>
        /// The thresold for adding graphics delivered by stream messages to this layer.
        /// If this value is set to one, every new obtained graphic is added to this layer immediately.
        /// </summary>
        public int GraphicsThresold { get; set; }

        public Uri Url { get; private set; }

        /// <summary>
        /// Starts connecting to the Streaming Service,
        /// </summary>
        /// <param name="cancellationToken">The cancelation token for this request.</param>
        public async Task ConnectAsync(CancellationToken cancellationToken)
        {
            _socket.BytesReceived += BytesReceived;
            await _socket.ConnectAsync(Url, cancellationToken);
        }

        private void BytesReceived(object sender, ReceiveEventArgs e)
        {
            var graphicsBuilder = new GraphicsFromStreamBuilder();
            graphicsBuilder.Visit(new StreamMessage(e.Data));
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
