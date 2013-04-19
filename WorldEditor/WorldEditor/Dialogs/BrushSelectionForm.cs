using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace WorldEditor.Dialogs
{
    public partial class BrushSelectionForm : UserControl
    {
        private readonly ReadOnlyCollection<ToolStripButton> mBrushButtons;

        public BrushSelectionForm()
        {
            InitializeComponent();

            CircleBrushButton.Checked = true;

            mBrushButtons = new List<ToolStripButton>
            {
                CircleBrushButton,
                CircleFeatherBrushButton,
                BlockBrushButton,
                BlockFeatherBrushButton
            }.AsReadOnly();

            CircleBrushButton.Click        += BrushButton_Click;
            CircleFeatherBrushButton.Click += BrushButton_Click;
            BlockBrushButton.Click         += BrushButton_Click;
            BlockFeatherBrushButton.Click  += BrushButton_Click;
            
            //InitializeButtonImages();
        }

        private void InitializeButtonImages()
        {
            this.CircleBrushButton.Image        = UILibrary.CircleBrushIcon;
            this.CircleFeatherBrushButton.Image = UILibrary.CircleFeatheredBrushIcon;
            this.BlockBrushButton.Image         = UILibrary.BlockBrushIcon;
            this.BlockFeatherBrushButton.Image  = UILibrary.BlockFeatheredBrushIcon;
        }

        private void BrushButton_Click(object sender, EventArgs e)
        {
            foreach (ToolStripButton button in mBrushButtons)
            {
                button.Checked = button == sender;
            }
        }
    }
}
