using System;
using System.IO.Ports;
using System.Windows.Forms;
using KeePass.Plugins;
using KeePassLib.Collections;

namespace XKBKeePassPlugin
{
    public sealed class XKBKeePassPluginExt : Plugin
    {
        private IPluginHost _host = null;

        public override bool Initialize(IPluginHost host)
        {
            if (host == null) return false;
            _host = host;

            var loginMenuItem = new ToolStripMenuItem("Load Login to XKB", null, onLoadLoginToXPW);
            loginMenuItem.ShortcutKeys = Keys.Control | Keys.B;

            _host.MainWindow.EntryContextMenu.Items.Add(loginMenuItem);

            var pwMenuItem = new ToolStripMenuItem("Load PW to XKB", null, onLoadPWToXPW);
            pwMenuItem.ShortcutKeys = Keys.Control | Keys.P;

            _host.MainWindow.EntryContextMenu.Items.Add(pwMenuItem);

            return true;
        }

        private void ActionOnPortAndSelectedEntry(Action<SerialPort, ProtectedStringDictionary> action)
        {
            string expectedDeviceId = Properties.XKBOptionsSettings.Default.PNPDeviceId;
            var comPortName = ComPortFinder.FindComPortWithPNPDeviceID(expectedDeviceId);
            //comPortName = "COM7";
            if (comPortName == null)
            {
                return;
            }

            try
            {
                using (var port = new SerialPort(comPortName))
                {
                    port.Open();
                    action(port, _host.MainWindow.GetSelectedEntry(false).Strings);
                    port.Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"ComPortName: {comPortName}\n\n{exception.Message}\n{exception.StackTrace}", "Exception");
            }
        }

        private void onLoadLoginToXPW(object sender, EventArgs e)
        {
            ActionOnPortAndSelectedEntry((port, dic) => port.Write($"{dic.GetSafe("UserName").ReadString()}\t{dic.GetSafe("Password").ReadString()}\n"));
        }

        private void onLoadPWToXPW(object sender, EventArgs e)
        {
            ActionOnPortAndSelectedEntry((port, dic) => port.Write($"{dic.GetSafe("Password").ReadString()}"));
        }


        public override void Terminate()
        {
        }

        
        public override ToolStripMenuItem GetMenuItem(PluginMenuType t)
        {
            // Provide a menu item for the main location(s)
            if (t == PluginMenuType.Main)
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                tsmi.Text = "XKB Options...";
                tsmi.Click += OnOptionsClicked;
                return tsmi;
            }

            return null; // No menu items in other locations
        }

        private void OnOptionsClicked(object sender, EventArgs e)
        {
            // Called when the menu item is clicked
            var options = new XKBOptions();
            options.ShowDialog();
        }
    }
}
