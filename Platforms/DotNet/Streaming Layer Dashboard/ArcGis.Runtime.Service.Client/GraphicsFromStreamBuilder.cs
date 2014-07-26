using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Layers;

namespace ArcGis.Runtime.Service.Client
{
    /// <summary>
    /// Builds graphics from a <see cref="StreamMessage"/>.
    /// </summary>
    internal class GraphicsFromStreamBuilder : IStreamVisitor
    {
        private readonly DataContractJsonSerializer _serializer;

        private readonly ICollection<Graphic> _graphics;

        internal GraphicsFromStreamBuilder()
        {
            _serializer = new DataContractJsonSerializer(typeof(List<Graphic>));
            _graphics = new List<Graphic>();
        }
        
        public bool Visit(StreamMessage message)
        {
            using (var memoryStream = new MemoryStream(message.RawBytes))
            {
                var graphics = _serializer.ReadObject(memoryStream);
            }
            return true;    
        }
    }
}
