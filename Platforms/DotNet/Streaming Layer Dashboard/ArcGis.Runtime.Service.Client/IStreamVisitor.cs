using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcGis.Runtime.Service.Client
{
    /// <summary>
    /// Visitor for messages delivered by a Streaming Service.
    /// </summary>
    public interface IStreamVisitor
    {
        /// <summary>
        /// Visits a stream message delivered by a Streaming Service.
        /// </summary>
        /// <param name="message">The message containing the serialized features.</param>
        /// <returns><c>false</c> if the next stream message should not be delegated to this visitor instance.</returns>
        bool visit(StreamMessage message);
    }
}
