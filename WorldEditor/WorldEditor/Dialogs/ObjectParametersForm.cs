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
            // Background default color.
            System.Drawing.Color defaultBGColor = System.Drawing.Color.White;
            // Background when there are different values for the same parameter.
            System.Drawing.Color differentValuesBGColor = System.Drawing.Color.LightYellow;

            // Set controls to default state.
            PositionX.BackColor = defaultBGColor;
            PositionY.BackColor = defaultBGColor;
            PositionZ.BackColor = defaultBGColor;
            Yaw.BackColor = defaultBGColor;
            Pitch.BackColor = defaultBGColor;
            Roll.BackColor = defaultBGColor;
            ScaleX.BackColor = defaultBGColor;
            ScaleY.BackColor = defaultBGColor;
            ScaleZ.BackColor = defaultBGColor;
            FloatingHeight.BackColor = defaultBGColor;
            Floating.Tag = CheckState.Unchecked;
            Physical.Tag = CheckState.Unchecked;
            Physical.Enabled = true;

            // Remove context sensitive parameters.
            AdditionalParametersPanel.Controls.Clear();

            List<TextBox> parameterTextBoxes = new List<TextBox>();
            if (SelectedObjects.Count > 0)
            {
                for (int i = 0; i < SelectedObjects[0].Parameters.Length; i++)
                {
                    string parameter = SelectedObjects[0].Parameters[i];

                    TextBox param = new TextBox();
                    param.Location = new System.Drawing.Point(0, AdditionalParametersPanel.Height);
                    param.BackColor = defaultBGColor;
                    param.Tag = i;
                    param.TextChanged += AdditionalParameterChanged;
                    AdditionalParametersPanel.Controls.Add(param);
                    parameterTextBoxes.Add(param);
                }
                AdditionalParametersPanel.Invalidate();
            }
            bool objectsOfSameType = true;
            
            for (int i = 0; i < SelectedObjects.Count; i++)
            {
                // Check that certain conditions are met for every selected object.
                if (!(SelectedObjects[i].Type.Contains("Chimera.PhysicsProp") || SelectedObjects[i].Type.Contains("Chimera.Prop")))
                {
                    Physical.Enabled = false;
                }
                
                // Check which parameters are not shared by all objects.
                if (i > 0)
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
                        FloatingHeight.BackColor = differentValuesBGColor;
                    }

                    if (SelectedObjects[i].Floating != SelectedObjects[i - 1].Floating)
                    {
                        Floating.Tag = CheckState.Indeterminate;
                    }

                    if (SelectedObjects[i].Type != SelectedObjects[i - 1].Type)
                    {
                        Physical.Tag = CheckState.Indeterminate;
                        objectsOfSameType = false;

                        parameterTextBoxes.Clear();
                        AdditionalParametersPanel.Controls.Clear();
                    }

                    if (objectsOfSameType)
                    {
                        for (int j = 0; j < SelectedObjects[i].Parameters.Length; j++)
                        {
                            if (SelectedObjects[i].Parameters[j] != SelectedObjects[i - 1].Parameters[j])
                            {
                                parameterTextBoxes[i].BackColor = differentValuesBGColor;
                            }
                        }
                    }
                }
            }

            if (SelectedObjects.Count > 0)
            {
                if (PositionX.BackColor == defaultBGColor)
                {
                    PositionX.Value = (decimal)SelectedObjects[0].Position.X;
                }
                if (PositionY.BackColor == defaultBGColor)
                {
                    PositionY.Value = (decimal)SelectedObjects[0].Position.Y;
                }
                if (PositionZ.BackColor == defaultBGColor)
                {
                    PositionZ.Value = (decimal)SelectedObjects[0].Position.Z;
                }

                if (Yaw.BackColor == defaultBGColor)
                {
                    Yaw.Value = (decimal)SelectedObjects[0].YawPitchRoll.X / (decimal)Math.PI * (decimal)180.0;
                }
                if (Pitch.BackColor == defaultBGColor)
                {
                    Pitch.Value = (decimal)SelectedObjects[0].YawPitchRoll.Y / (decimal)Math.PI * (decimal)180.0;
                }
                if (Roll.BackColor == defaultBGColor)
                {
                    Roll.Value = (decimal)SelectedObjects[0].YawPitchRoll.Z / (decimal)Math.PI * (decimal)180.0;
                }

                if (ScaleX.BackColor == defaultBGColor)
                {
                    ScaleX.Value = (decimal)SelectedObjects[0].Scale.X;
                }
                if (ScaleY.BackColor == defaultBGColor)
                {
                    ScaleY.Value = (decimal)SelectedObjects[0].Scale.Y;
                }
                if (ScaleZ.BackColor == defaultBGColor)
                {
                    ScaleZ.Value = (decimal)SelectedObjects[0].Scale.Z;
                }

                if (FloatingHeight.BackColor == defaultBGColor)
                {
                    FloatingHeight.Value = (decimal)SelectedObjects[0].Height;
                }

                if ((CheckState)Floating.Tag != CheckState.Indeterminate)
                {
                    Floating.CheckState = SelectedObjects[0].Floating ? CheckState.Checked : CheckState.Unchecked;
                }
                else
                {
                    Floating.CheckState = CheckState.Indeterminate;
                }

                if ((CheckState)Physical.Tag != CheckState.Indeterminate && Physical.Enabled)
                {
                    Physical.CheckState = SelectedObjects[0].Type.Contains("Chimera.PhysicsProp") ? CheckState.Checked : CheckState.Unchecked;
                }
                else
                {
                    Physical.CheckState = CheckState.Indeterminate;
                }

                for (int i = 0; i < parameterTextBoxes.Count; i++)
                {
                    parameterTextBoxes[i].Text = SelectedObjects[0].Parameters[i];
                }
            }

            // Resize form to fit contents.
            int height = 0;
            foreach (Control control in Controls)
            {
                height += control.Height;
            }
            Height = height;
        }

        private void Floating_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).CheckState == CheckState.Checked)
            {
                foreach (DummyObject dummyObject in SelectedObjects)
                {
                    dummyObject.Floating = true;
                }
                PositionY.Enabled = false;
                FloatingHeight.Enabled = true;
            }
            else if ((sender as CheckBox).CheckState == CheckState.Unchecked)
            {
                foreach (DummyObject dummyObject in SelectedObjects)
                {
                    dummyObject.Floating = false;
                }
                PositionY.Enabled = true;
                FloatingHeight.Enabled = false;
            }
            else if ((sender as CheckBox).CheckState == CheckState.Indeterminate)
            {
                PositionY.Enabled = false;
                FloatingHeight.Enabled = false;
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

        private void Physical_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).CheckState != CheckState.Indeterminate)
            {
                foreach (DummyObject dummyObject in SelectedObjects)
                {
                    dummyObject.Type = ((sender as CheckBox).Checked) ? "Chimera.PhysicsProp" : "Chimera.Prop";
                }
            }
        }

        private void AdditionalParameterChanged(object sender, EventArgs e)
        {
            foreach (DummyObject dummyObject in SelectedObjects)
            {
                dummyObject.Parameters[(int)(sender as TextBox).Tag] = (sender as TextBox).Text;
            }
        }
    }
}
