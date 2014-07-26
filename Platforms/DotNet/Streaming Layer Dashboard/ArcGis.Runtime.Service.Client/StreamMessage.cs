using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcGis.Runtime.Service.Client
{
    /// <summary>
    /// Message from a Streaming Service.
    /// </summary>
    public class StreamMessage
    {
        private readonly byte[] _buffer;

        internal StreamMessage(byte[] buffer)
        {
            _buffer = buffer;
        }

        /// <summary>
        /// Raw bytes containing the serialized features.
        /// </summary>
        public byte[] RawBytes
        {
            get
            {
                return _buffer;
            }
        }
    }
}
