using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

using ArcGis.Runtime.Service.Client;

namespace ArcGis.Runtime.StreamingLayerDashboard.Model
{
    /// <summary>
    /// Collection of Streaming Layers used for data binding.
    /// </summary>
    internal class StreamingLayerCollection : ObservableCollection<StreamingLayer>
    {
    }
}
