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
    public partial class TextureLayerContainerForm : UserControl
    {
        public TextureLayerContainerForm()
        {
            InitializeComponent();

            InitializeLayerNames();
        }

        private void InitializeLayerNames()
        {
            this.BackgroundLayer.InvisibleButton.Text = "           Background Layer";
            this.BackgroundLayer.InvisibleButton.TextAlign = ContentAlignment.MiddleLeft;
            this.Layer1.InvisibleButton.Text = "           Layer 1";
            this.Layer1.InvisibleButton.TextAlign = ContentAlignment.MiddleLeft;
            this.Layer2.InvisibleButton.Text = "           Layer 2";
            this.Layer2.InvisibleButton.TextAlign = ContentAlignment.MiddleLeft;
            this.Layer3.InvisibleButton.Text = "           Layer 3";
            this.Layer3.InvisibleButton.TextAlign = ContentAlignment.MiddleLeft;
            this.Layer4.InvisibleButton.Text = "           Layer 4";
            this.Layer4.InvisibleButton.TextAlign = ContentAlignment.MiddleLeft;
        }
    }
}
