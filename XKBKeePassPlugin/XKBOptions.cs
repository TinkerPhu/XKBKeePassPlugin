using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace XKBKeePassPlugin
{
    public partial class XKBOptions : Form
    {
        private List<Dictionary<string, object>> _propsCollection;
        private Func<Dictionary<string, object>, string> FormatComboboxDisplay = p => $"{p[DeviceIdString]} / {p[PnpDeviceIdString]}";
        private const string DeviceIdString = "DeviceID";
        private Label label1;
        private ComboBox _deviceIdComboBox;
        private Timer _deviceCheckTimer;
        private System.ComponentModel.IContainer components;
        private Label label2;
        private TextBox _cyrcuitPytnonSource_textBox;
        private const string PnpDeviceIdString = "PNPDeviceID";

        public XKBOptions()
        {
            InitializeComponent();

            _propsCollection = ComPortFinder.CollectAllComPortProps().ToList();

            _deviceIdComboBox.Items.AddRange(_propsCollection.Select(FormatComboboxDisplay).ToArray());

            string expectedDeviceId = Properties.XKBOptionsSettings.Default.PNPDeviceId;

            var ind = _propsCollection.Select((p, index) => new { p, index })
                .FirstOrDefault(i => i.p[PnpDeviceIdString].Equals(expectedDeviceId))?.index;

            _deviceIdComboBox.SelectedIndex = ind??0;

            _deviceCheckTimer.Interval = 200;
            _deviceCheckTimer.Start();
        }

        private void _deviceCheckTimer_Tick(object sender, EventArgs e)
        {
            var propsCollection = ComPortFinder.CollectAllComPortProps();

            var newlyAdded = propsCollection.Where(p=> !_propsCollection.Any(o=> o[PnpDeviceIdString].Equals(p[PnpDeviceIdString]) )).ToArray();
            if (newlyAdded.Any())
            {
                _propsCollection.AddRange(newlyAdded);
                var strings = newlyAdded.Select(FormatComboboxDisplay).ToArray();
                _deviceIdComboBox.Items.AddRange(strings);
                //_deviceIdComboBox.SelectedIndex = _deviceIdComboBox.Items.Count - 1;
            }
        }

        private void _deviceIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ind = _deviceIdComboBox.SelectedIndex;

            var item = _propsCollection.Select((p, index) => new { p, index }).First(i => i.index == ind).p;

            var defaultPnpDeviceId = item[PnpDeviceIdString].ToString();
            Properties.XKBOptionsSettings.Default.PNPDeviceId = defaultPnpDeviceId;
            Properties.XKBOptionsSettings.Default.Save();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XKBOptions));
            this.label1 = new System.Windows.Forms.Label();
            this._deviceIdComboBox = new System.Windows.Forms.ComboBox();
            this._deviceCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this._cyrcuitPytnonSource_textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ComPort";
            // 
            // _deviceIdComboBox
            // 
            this._deviceIdComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._deviceIdComboBox.FormattingEnabled = true;
            this._deviceIdComboBox.Location = new System.Drawing.Point(96, 20);
            this._deviceIdComboBox.Name = "_deviceIdComboBox";
            this._deviceIdComboBox.Size = new System.Drawing.Size(288, 21);
            this._deviceIdComboBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Copy the following CircuitPython code:";
            // 
            // _cyrcuitPytnonSource_textBox
            // 
            this._cyrcuitPytnonSource_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cyrcuitPytnonSource_textBox.Location = new System.Drawing.Point(27, 104);
            this._cyrcuitPytnonSource_textBox.Multiline = true;
            this._cyrcuitPytnonSource_textBox.Name = "_cyrcuitPytnonSource_textBox";
            this._cyrcuitPytnonSource_textBox.ReadOnly = true;
            this._cyrcuitPytnonSource_textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._cyrcuitPytnonSource_textBox.Size = new System.Drawing.Size(357, 63);
            this._cyrcuitPytnonSource_textBox.TabIndex = 3;
            this._cyrcuitPytnonSource_textBox.Text = resources.GetString("_cyrcuitPytnonSource_textBox.Text");
            // 
            // XKBOptions
            // 
            this.ClientSize = new System.Drawing.Size(411, 192);
            this.Controls.Add(this._cyrcuitPytnonSource_textBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._deviceIdComboBox);
            this.Controls.Add(this.label1);
            this.Name = "XKBOptions";
            this.Text = "XKB Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
