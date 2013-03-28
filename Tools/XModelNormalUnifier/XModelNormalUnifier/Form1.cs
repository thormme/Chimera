using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XModelNormalUnifier
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.AddExtension = true;
            openDialog.Filter = "X Model Files | *.X";
            openDialog.DefaultExt = ".X";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileInfo = new FileInfo(openDialog.FileName);
                this.fileNameLabel.Text = fileInfo.Name;
                XModelManager.LoadModel(openDialog.FileName);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.AddExtension = true;
            saveDialog.Filter = "X Model Files | *.X";
            saveDialog.DefaultExt = ".X";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                if (XModelManager.SaveModel(saveDialog.FileName) == false)
                {
                    MessageBox.Show("You must Open a .X model file before you can Save.");
                }
            }
        }
    }
}
