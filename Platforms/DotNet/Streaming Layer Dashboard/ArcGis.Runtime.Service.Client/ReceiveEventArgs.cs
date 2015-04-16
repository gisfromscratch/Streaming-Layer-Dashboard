using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcGis.Runtime.Service.Client
{
    /// <summary>
    /// Event argument which is raised when data was received over a WebSocket connection.
    /// </summary>
    internal class ReceiveEventArgs : EventArgs
    {
        private readonly byte[] _data;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="data">The data which was received over a WebSocket connection.</param>
        internal ReceiveEventArgs(byte[] data)
        {
            _data = data;
        }

        /// <summary>
        /// The received bytes.
        /// </summary>
        internal byte[] Data
        {
            get
            {
                return _data;
            }
        }
    }
}
