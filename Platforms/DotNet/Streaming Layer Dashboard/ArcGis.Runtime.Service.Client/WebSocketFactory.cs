using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcGis.Runtime.Service.Client
{
    /// <summary>
    /// Creates the WebSocket instances dependent on the current Windows platform.
    /// </summary>
    internal class WebSocketFactory
    {
        /// <summary>
        /// Creates a new WebSocket instance.
        /// </summary>
        /// <returns>A newly created WebSocket instance.</returns>
        internal IWebSocket CreateWebSocket()
        {
            //TODO: Analyze when the platform dependent implmentation should be used.
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32Windows:
                case PlatformID.Win32NT:
                    return new LazyWebSocket();

                default:
                    return new LazyWebSocket();
            }
        }
    }
}
