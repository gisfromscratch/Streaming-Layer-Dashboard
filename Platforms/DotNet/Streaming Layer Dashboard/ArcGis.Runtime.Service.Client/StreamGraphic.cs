using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Geometry;

using Newtonsoft.Json;

namespace ArcGis.Runtime.Service.Client
{
    /// <summary>
    /// Graphic delivered from a Streaming Service.
    /// </summary>
    internal class StreamGraphic
    {
        [JsonProperty(PropertyName = @"geometry")]
        internal MapPoint Geometry { get; set; }

        [JsonProperty(PropertyName = @"attributes")]
        internal Dictionary<string, object> Attributes { get; set; }
    }
}
