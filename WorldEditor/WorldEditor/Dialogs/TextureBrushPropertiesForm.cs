using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WorldEditor.Dialogs
{
    public partial class TextureBrushPropertiesForm : UserControl
    {
        public TextureBrushPropertiesForm()
        {
            InitializeComponent();

            this.BrushSizeTrackBar.ValueChanged += UpdateSizeUpDown;
            this.BrushSizeUpDown.ValueChanged += UpdateSizeTrackBar;

            this.BrushMagnitudeTrackBar.ValueChanged += UpdateMagnitudeUpDown;
            this.BrushMagnitudeUpDown.ValueChanged += UpdateMagnitudeTrackBar;
        }

        private void UpdateSizeUpDown(object sender, EventArgs e)
        {
            this.BrushSizeUpDown.Value = BrushSizeTrackBar.Value;
        }

        private void UpdateMagnitudeUpDown(object sender, EventArgs e)
        {
            this.BrushMagnitudeUpDown.Value = BrushMagnitudeTrackBar.Value;
        }

        private void UpdateSizeTrackBar(object sender, EventArgs e)
        {
            this.BrushSizeTrackBar.Value = (int)BrushSizeUpDown.Value;
        }

        private void UpdateMagnitudeTrackBar(object sender, EventArgs e)
        {
            this.BrushMagnitudeTrackBar.Value = (int)BrushMagnitudeUpDown.Value;
        }
    }
}
