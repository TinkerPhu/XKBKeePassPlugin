using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using XKBKeePassPlugin;

namespace XPWKeePassPlugin.Tests
{
    public class OptionsTest
    {
        //[Ignore("Blocking Demo")]
        [Test]
        public void OptionsDemo()
        {
            new XKBOptions().ShowDialog();
        }
    }
}
