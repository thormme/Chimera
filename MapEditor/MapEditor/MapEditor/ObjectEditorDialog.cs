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

namespace MapEditor
{
    /// <summary>
    /// Creates a map editor menu
    /// </summary>
    public class ObjectEditorDialog : WindowControl
    {
        private MapEditorDialog mMapEditor;
        private Nuclex.UserInterface.Controls.LabelControl mSizeLabel;
        private Nuclex.UserInterface.Controls.LabelControl mIntensityLabel;
        private Nuclex.UserInterface.Controls.LabelControl mSetLabel;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mSizeInput;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mIntensityInput;
        private Nuclex.UserInterface.Controls.Desktop.OptionControl mSetOption;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mDoneButton;

        public ObjectEditorDialog(MapEditorDialog mapEditor) :
            base()
        {
            mMapEditor = mapEditor;
            InitializeComponent();
        }

        #region Not component designer generated code

        /// <summary>
        /// Adds items to dialog
        /// </summary>
        private void InitializeComponent()
        {

            BringToFront();

            // Declare all components
            mSizeLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mIntensityLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mSetLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mSizeInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mIntensityInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mSetOption = new Nuclex.UserInterface.Controls.Desktop.OptionControl();
            mDoneButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();

            // Position components
            mSizeLabel.Text = "Size:";
            mSizeLabel.Bounds = new UniRectangle(20.0f, 45.0f, 100.0f, 30.0f);
            mSizeInput.Bounds = new UniRectangle(120.0f, 40.0f, 140.0f, 30.0f);

            mIntensityLabel.Text = "Intensity:";
            mIntensityLabel.Bounds = new UniRectangle(20.0f, 85.0f, 100.0f, 30.0f);
            mIntensityInput.Bounds = new UniRectangle(120.0f, 80.0f, 140.0f, 30.0f);

            mSetLabel.Text = "Set:";
            mSetLabel.Bounds = new UniRectangle(20.0f, 125.0f, 100.0f, 30.0f);
            mSetOption.Bounds = new UniRectangle(120.0f, 120.0f, 30.0f, 30.0f);

            mDoneButton.Text = "Done";
            mDoneButton.Bounds = new UniRectangle(new UniScalar(1.0f, -100.0f), new UniScalar(1.0f, -45.0f), 80, 24);
            mDoneButton.Pressed += delegate(object sender, EventArgs arguments) { DoneClicked(sender, arguments); };

            // Add components to GUI
            Children.Add(mSizeLabel);
            Children.Add(mSizeInput);
            Children.Add(mIntensityLabel);
            Children.Add(mIntensityInput);
            Children.Add(mSetLabel);
            Children.Add(mSetOption);
            Children.Add(mDoneButton);

        }

        #endregion // Not component designer generated code

        public bool GetInputs(out int size, out int intensity, out bool set)
        {

            size = 0;
            intensity = 0;
            set = false;

            try
            {
                size = Convert.ToInt32(mSizeInput.Text);
                intensity = Convert.ToInt32(mIntensityInput.Text);
                set = mSetOption.Selected;
            }
            catch (FormatException)
            {
                return false;
            }

            return true;

        }

        public void ObjectsEnable()
        {
            Bounds = new UniRectangle(10.0f, 10.0f, 280.0f, 170.0f);
        }

        private void DoneClicked(object sender, EventArgs arguments)
        {
            Bounds = new UniRectangle(-1000.0f, -1000.0f, 0.0f, 0.0f);
            mMapEditor.Done();
        }

    }
}