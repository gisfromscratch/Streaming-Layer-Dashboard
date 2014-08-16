using System;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ArcGis.Runtime.Service.Client;

namespace Client.Testing
{
    [TestClass]
    public class StreamingLayerTests
    {
        [TestMethod]
        public void TestConnection()
        {
            var streamingLayer = new StreamingLayer(@"ws://geoeventsample1.esri.com:8080/exactearthservice");
            var task = streamingLayer.ConnectAsync(CancellationToken.None);
            task.Wait();
        }
    }
}
