using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;
using GameConstructLibrary;

namespace MapEditor
{
    /// <summary>
    /// Creates a map editor menu
    /// </summary>
    public class HeightEditorDialog : Dialog
    {

        private ParametersDialog mParametersDialog;

        private Nuclex.UserInterface.Controls.LabelControl mSizeLabel;
        private Nuclex.UserInterface.Controls.LabelControl mIntensityLabel;
        private Nuclex.UserInterface.Controls.LabelControl mFeatherLabel;
        private Nuclex.UserInterface.Controls.LabelControl mSetLabel;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mSizeInput;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mIntensityInput;
        private Nuclex.UserInterface.Controls.Desktop.OptionControl mFeatherOption;
        private Nuclex.UserInterface.Controls.Desktop.OptionControl mSetOption;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mEditButton;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mBackButton;

        public HeightEditorDialog() :
            base()
        {
            InitializeComponent();
        }

        #region Component Layout

        /// <summary>
        /// Adds items to dialog
        /// </summary>
        private void InitializeComponent()
        {

            BringToFront();

            // Declare all components
            mSizeLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mIntensityLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mFeatherLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mSetLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mSizeInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mIntensityInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mFeatherOption = new Nuclex.UserInterface.Controls.Desktop.OptionControl();
            mSetOption = new Nuclex.UserInterface.Controls.Desktop.OptionControl();
            mEditButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();
            mBackButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();

            // Position components
            mSizeLabel.Text = "Size:";
            mSizeLabel.Bounds = new UniRectangle(20.0f, 45.0f, 100.0f, 30.0f);
            mSizeInput.Text = "0";
            mSizeInput.Bounds = new UniRectangle(120.0f, 40.0f, 140.0f, 30.0f);

            mIntensityLabel.Text = "Intensity:";
            mIntensityLabel.Bounds = new UniRectangle(20.0f, 85.0f, 100.0f, 30.0f);
            mIntensityInput.Text = "0";
            mIntensityInput.Bounds = new UniRectangle(120.0f, 80.0f, 140.0f, 30.0f);

            mFeatherLabel.Text = "Feather:";
            mFeatherLabel.Bounds = new UniRectangle(20.0f, 125.0f, 100.0f, 30.0f);
            mFeatherOption.Bounds = new UniRectangle(120.0f, 120.0f, 30.0f, 30.0f);

            mSetLabel.Text = "Set:";
            mSetLabel.Bounds = new UniRectangle(20.0f, 165.0f, 100.0f, 30.0f);
            mSetOption.Bounds = new UniRectangle(120.0f, 160.0f, 30.0f, 30.0f);

            mEditButton.Text = "Edit";
            mEditButton.Bounds = new UniRectangle(new UniScalar(1.0f, -180.0f), new UniScalar(1.0f, -45.0f), 80, 24);
            mEditButton.Pressed += delegate(object sender, EventArgs arguments) { EditClicked(sender, arguments); };

            mBackButton.Text = "Back";
            mBackButton.Bounds = new UniRectangle(new UniScalar(1.0f, -90.0f), new UniScalar(1.0f, -45.0f), 80, 24);
            mBackButton.Pressed += delegate(object sender, EventArgs arguments) { BackClicked(sender, arguments); };

            Bounds = new UniRectangle(10.0f, 10.0f, 280.0f, 260.0f);
            mBounds = Bounds;

            // Add components to GUI
            Children.Add(mSizeLabel);
            Children.Add(mSizeInput);
            Children.Add(mIntensityLabel);
            Children.Add(mIntensityInput);
            Children.Add(mFeatherLabel);
            Children.Add(mFeatherOption);
            Children.Add(mSetLabel);
            Children.Add(mSetOption);
            Children.Add(mEditButton);
            Children.Add(mBackButton);

        }

        #endregion // Not component designer generated code

        public Single GetScale()
        {
            Single scale = 0.0f;
            try
            {
                scale = Convert.ToSingle(mSizeInput.Text);
            }
            catch (SystemException)
            {
                // Invalid Input
            }
            return scale;
        }

        public Single GetIntensity()
        {
            Single intensity = 0.0f;
            try
            {
                intensity = Convert.ToSingle(mSizeInput.Text);
            }
            catch (SystemException)
            {
                // Invalid Input
            }
            return intensity;
        }

        public Boolean GetFeather()
        {
            return mFeatherOption.Selected;
        }

        public Boolean GetSet()
        {
            return mSetOption.Selected;
        }

        public bool GetHeight(out int size, out int intensity, out bool feather, out bool set)
        {

            size = 0;
            intensity = 0;
            feather = false;
            set = false;

            try
            {
                size = Convert.ToInt32(mSizeInput.Text);
                intensity = Convert.ToInt32(mIntensityInput.Text);
                feather = mFeatherOption.Selected;
                set = mSetOption.Selected;
            }
            catch (FormatException)
            {
                return false;
            }

            return true;

        }

        private void EditClicked(object sender, EventArgs arguments)
        {

            try
            {
                DummyObject temp = new DummyObject();
                temp.Model = "editor";
                temp.Position = Vector3.Zero;
                temp.YawPitchRoll = Vector3.Zero;
                temp.Scale = new Vector3(Convert.ToInt32(mSizeInput.Text) * GameMapEditor.MapScale.X, Convert.ToInt32(mSizeInput.Text) * GameMapEditor.MapScale.Y /*(GameMapEditor.MapScale.X + GameMapEditor.MapScale.Z) / 2*/, Convert.ToInt32(mSizeInput.Text) * GameMapEditor.MapScale.Z);
                GameMapEditor.Entity.SetModel(temp);

                GameMapEditor.Size = Convert.ToInt32(mSizeInput.Text);
                GameMapEditor.Intensity = Convert.ToInt32(mIntensityInput.Text);
                GameMapEditor.Feather = mFeatherOption.Selected;
                GameMapEditor.Set = mSetOption.Selected;

                GameMapEditor.EditMode = Edit.Height;
                GameMapEditor.ToggleState(States.Parameters);
            }
            catch (FormatException)
            {

            }
        }

        private void BackClicked(object sender, EventArgs arguments)
        {
            GameMapEditor.ToggleState(States.None);
        }

    }
}