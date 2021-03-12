using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using XKBKeePassPlugin;

namespace XPWKeePassPlugin.Tests
{
    public class ComPortFinderTest
    {
        [Test]
        public void ComPortFinderDemo()
        {
            var propsCollection = ComPortFinder.CollectAllComPortProps();
            var allComPortsToString = ComPortFinder.CollectAllComPortsToString(propsCollection);
            Console.Write(allComPortsToString);

            Assert.True(propsCollection.Any(e=>e["PNPDeviceID"].ToString().StartsWith(@"USB\VID_239A&PID_8052&MI_00\6&")));
            Assert.NotNull(ComPortFinder.FindComPortWithPNPDeviceID(@"USB\VID_239A&PID_8052&MI_00\6&"));
        }
    }
}
