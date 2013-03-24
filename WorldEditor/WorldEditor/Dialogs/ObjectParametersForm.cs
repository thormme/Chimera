using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GameConstructLibrary;
using Microsoft.Xna.Framework;

namespace WorldEditor.Dialogs
{
    public partial class ObjectParametersForm : Form
    {
        public ObjectParametersForm()
        {
            InitializeComponent();
        }

        public List<DummyObject> SelectedObjects = new List<DummyObject>();

        public void UpdateParameterFields()
        {
            // Background when there are different values for the same parameter.
            System.Drawing.Color differentValuesBGColor = System.Drawing.Color.LightYellow;

            // Set controls to default state.
            PositionX.BackColor = DefaultBackColor;
            PositionY.BackColor = DefaultBackColor;
            PositionZ.BackColor = DefaultBackColor;
            Yaw.BackColor = DefaultBackColor;
            Pitch.BackColor = DefaultBackColor;
            Roll.BackColor = DefaultBackColor;
            ScaleX.BackColor = DefaultBackColor;
            ScaleY.BackColor = DefaultBackColor;
            ScaleZ.BackColor = DefaultBackColor;
            Height.BackColor = DefaultBackColor;
            Floating.CheckState = CheckState.Unchecked;

            // Check which parameters are not shared by all objects.
            for (int i = 1; i < SelectedObjects.Count; i++)
            {
                if (SelectedObjects[i].Position.X != SelectedObjects[i - 1].Position.X)
                {
                    PositionX.BackColor = differentValuesBGColor;
                }
                if (SelectedObjects[i].Position.Y != SelectedObjects[i - 1].Position.Y)
                {
                    PositionY.BackColor = differentValuesBGColor;
                }
                if (SelectedObjects[i].Position.Z != SelectedObjects[i - 1].Position.Z)
                {
                    PositionZ.BackColor = differentValuesBGColor;
                }

                if (SelectedObjects[i].YawPitchRoll.X != SelectedObjects[i - 1].YawPitchRoll.X)
                {
                    Yaw.BackColor = differentValuesBGColor;
                }
                if (SelectedObjects[i].YawPitchRoll.Y != SelectedObjects[i - 1].YawPitchRoll.Y)
                {
                    Pitch.BackColor = differentValuesBGColor;
                }
                if (SelectedObjects[i].YawPitchRoll.Z != SelectedObjects[i - 1].YawPitchRoll.Z)
                {
                    Roll.BackColor = differentValuesBGColor;
                }

                if (SelectedObjects[i].Scale.X != SelectedObjects[i - 1].Scale.X)
                {
                    ScaleX.BackColor = differentValuesBGColor;
                }
                if (SelectedObjects[i].Scale.Y != SelectedObjects[i - 1].Scale.Y)
                {
                    ScaleY.BackColor = differentValuesBGColor;
                }
                if (SelectedObjects[i].Scale.Z != SelectedObjects[i - 1].Scale.Z)
                {
                    ScaleZ.BackColor = differentValuesBGColor;
                }

                if (SelectedObjects[i].Height != SelectedObjects[i - 1].Height)
                {
                    Height.BackColor = differentValuesBGColor;
                }

                if (SelectedObjects[i].Floating != SelectedObjects[i - 1].Floating)
                {
                    Floating.CheckState = CheckState.Indeterminate;
                }
            }

            if (SelectedObjects.Count > 0)
            {
                if (PositionX.BackColor == DefaultBackColor)
                {
                    PositionX.Value = (decimal)SelectedObjects[0].Position.X;
                }
                if (PositionY.BackColor == DefaultBackColor)
                {
                    PositionY.Value = (decimal)SelectedObjects[0].Position.Y;
                }
                if (PositionZ.BackColor == DefaultBackColor)
                {
                    PositionZ.Value = (decimal)SelectedObjects[0].Position.Z;
                }

                if (Yaw.BackColor == DefaultBackColor)
                {
                    Yaw.Value = (decimal)SelectedObjects[0].YawPitchRoll.X;
                }
                if (Pitch.BackColor == DefaultBackColor)
                {
                    Pitch.Value = (decimal)SelectedObjects[0].YawPitchRoll.Y;
                }
                if (Roll.BackColor == DefaultBackColor)
                {
                    Roll.Value = (decimal)SelectedObjects[0].YawPitchRoll.Z;
                }

                if (ScaleX.BackColor == DefaultBackColor)
                {
                    ScaleX.Value = (decimal)SelectedObjects[0].Scale.X;
                }
                if (ScaleY.BackColor == DefaultBackColor)
                {
                    ScaleY.Value = (decimal)SelectedObjects[0].Scale.Y;
                }
                if (ScaleZ.BackColor == DefaultBackColor)
                {
                    ScaleZ.Value = (decimal)SelectedObjects[0].Scale.Z;
                }

                if (Height.BackColor == DefaultBackColor)
                {
                    Height.Value = (decimal)SelectedObjects[0].Height;
                }

                if (Floating.CheckState != CheckState.Indeterminate)
                {
                    Floating.Checked = SelectedObjects[0].Floating;
                }
            }
        }

        private void Floating_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).CheckState == CheckState.Checked)
            {
                PositionY.Enabled = false;
                Height.Enabled = true;
            }
            else if ((sender as CheckBox).CheckState == CheckState.Unchecked)
            {
                PositionY.Enabled = true;
                Height.Enabled = false;
            }
            else if ((sender as CheckBox).CheckState == CheckState.Indeterminate)
            {
                PositionY.Enabled = false;
                Height.Enabled = false;
            }
        }

        private void PositionX_ValueChanged(object sender, EventArgs e)
        {
            foreach (DummyObject dummyObject in SelectedObjects)
            {
                dummyObject.Position = new Vector3((float)((sender as NumericUpDown).Value), dummyObject.Position.Y, dummyObject.Position.Z);
            }
        }

        private void PositionY_ValueChanged(object sender, EventArgs e)
        {
            foreach (DummyObject dummyObject in SelectedObjects)
            {
                dummyObject.Position = new Vector3(dummyObject.Position.X, (float)((sender as NumericUpDown).Value), dummyObject.Position.Z);
            }
        }

        private void PositionZ_ValueChanged(object sender, EventArgs e)
        {
            foreach (DummyObject dummyObject in SelectedObjects)
            {
                dummyObject.Position = new Vector3(dummyObject.Position.X, dummyObject.Position.Y, (float)((sender as NumericUpDown).Value));
            }
        }

        private void Yaw_ValueChanged(object sender, EventArgs e)
        {
            foreach (DummyObject dummyObject in SelectedObjects)
            {
                dummyObject.YawPitchRoll = new Vector3((float)((sender as NumericUpDown).Value) / 180.0f * (float)Math.PI, dummyObject.YawPitchRoll.Y, dummyObject.YawPitchRoll.Z);
            }
        }

        private void Pitch_ValueChanged(object sender, EventArgs e)
        {
            foreach (DummyObject dummyObject in SelectedObjects)
            {
                dummyObject.YawPitchRoll = new Vector3(dummyObject.YawPitchRoll.X, (float)((sender as NumericUpDown).Value) / 180.0f * (float)Math.PI, dummyObject.YawPitchRoll.Z);
            }
        }

        private void Roll_ValueChanged(object sender, EventArgs e)
        {
            foreach (DummyObject dummyObject in SelectedObjects)
            {
                dummyObject.YawPitchRoll = new Vector3(dummyObject.YawPitchRoll.X, dummyObject.YawPitchRoll.Y, (float)((sender as NumericUpDown).Value) / 180.0f * (float)Math.PI);
            }
        }

        private void ScaleX_ValueChanged(object sender, EventArgs e)
        {
            foreach (DummyObject dummyObject in SelectedObjects)
            {
                dummyObject.Scale = new Vector3((float)((sender as NumericUpDown).Value), dummyObject.Scale.Y, dummyObject.Scale.Z);
            }
        }

        private void ScaleY_ValueChanged(object sender, EventArgs e)
        {
            foreach (DummyObject dummyObject in SelectedObjects)
            {
                dummyObject.Scale = new Vector3(dummyObject.Scale.X, (float)((sender as NumericUpDown).Value), dummyObject.Scale.Z);
            }
        }

        private void ScaleZ_ValueChanged(object sender, EventArgs e)
        {
            foreach (DummyObject dummyObject in SelectedObjects)
            {
                dummyObject.Scale = new Vector3(dummyObject.Scale.X, dummyObject.Scale.Y, (float)((sender as NumericUpDown).Value));
            }
        }

        private void Height_ValueChanged(object sender, EventArgs e)
        {
            foreach (DummyObject dummyObject in SelectedObjects)
            {
                dummyObject.Height = (float)((sender as NumericUpDown).Value);
            }
        }

    }
}
