using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Layers;
using Esri.ArcGISRuntime.Symbology;

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
                //messageAsJson = messageAsJson.Replace("\\\"", "\"");
                var streamGraphics = JsonConvert.DeserializeObject<List<StreamGraphic>>(messageAsJson);
                foreach (var streamGraphic in streamGraphics)
                {
                    var newGraphic = new Graphic(streamGraphic.Geometry, streamGraphic.Attributes);
                    
                    // TODO: Create symbology
                    switch (newGraphic.Geometry.GeometryType)
                    {
                        case GeometryType.Point:
                            var pointSymbol = new SimpleMarkerSymbol();
                            pointSymbol.Color = Colors.White;
                            newGraphic.Symbol = pointSymbol;
                            break;
                    }
                    _graphics.Add(newGraphic);
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
