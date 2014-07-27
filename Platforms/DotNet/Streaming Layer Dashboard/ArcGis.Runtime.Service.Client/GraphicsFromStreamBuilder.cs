using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Layers;

using Newtonsoft.Json;

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
            _serializer = new DataContractJsonSerializer(typeof(List<List<StreamGraphic>>));
            _graphics = new List<Graphic>();
        }
        
        public bool Visit(StreamMessage message)
        {
            try
            {
                var messageAsJson = Encoding.UTF8.GetString(message.RawBytes);
                messageAsJson = messageAsJson.Replace("\\\"", "\"");
                File.AppendAllText(@"C:\data\stream.json", messageAsJson);
                var streamGraphics = JsonConvert.DeserializeObject<List<StreamGraphic>>(messageAsJson);
                foreach (var streamGraphic in streamGraphics)
                {
                    _graphics.Add(new Graphic(streamGraphic.Geometry, streamGraphic.Attributes));
                }
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return false;
            }
        }

        public ICollection<Graphic> Graphics
        {
            get
            {
                return _graphics;
            }
        }
    }
}
