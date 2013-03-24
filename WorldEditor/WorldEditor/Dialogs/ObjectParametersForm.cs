using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WorldEditor.Dialogs
{
    public partial class ObjectParametersForm : Form
    {
        public ObjectParametersForm()
        {
            InitializeComponent();
        }

        private void Floating_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                PositionY.Enabled = false;
                Height.Enabled = true;
            }
            else
            {
                PositionY.Enabled = true;
                Height.Enabled = false;
            }
        }

    }
}
