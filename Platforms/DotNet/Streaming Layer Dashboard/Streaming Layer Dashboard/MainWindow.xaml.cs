using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ArcGis.Runtime.Service.Client;
using ArcGis.Runtime.StreamingLayerDashboard.Model;

namespace ArcGis.Runtime.StreamingLayerDashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Connect(object sender, RoutedEventArgs e)
        {
            ConnectToStreamingService(@"ws://geoeventsample1.esri.com:8080/exactearthservice");
        }

        private void ConnectToStreamingService(string url)
        {
            var streamingLayer = new StreamingLayer(url);
            MapView.Map.Layers.Add(streamingLayer);
         
            var connectTask = streamingLayer.ConnectAsync(CancellationToken.None);
            var layerCollection = (StreamingLayerCollection) StreamingLayerGrid.ItemsSource;
            layerCollection.Add(streamingLayer);
        }
    }
}
