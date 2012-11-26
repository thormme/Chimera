using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SaveMapDialog : Dialog
    {

        private Nuclex.UserInterface.Controls.LabelControl mSaveLevelLabel;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mNameInput;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mSaveButton;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mCancelButton;
        

        public SaveMapDialog() :
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
            mSaveLevelLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mNameInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mSaveButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();
            mCancelButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();

            // Position components
            mSaveLevelLabel.Text = "Map Name:";
            mSaveLevelLabel.Bounds = new UniRectangle(20.0f, 50.0f, 100.0f, 30.0f);

            mNameInput.Bounds = new UniRectangle(120.0f, 50.0f, 240.0f, 30.0f);

            mSaveButton.Text = "Save";
            mSaveButton.Bounds = new UniRectangle(new UniScalar(1.0f, -180.0f), new UniScalar(1.0f, -40.0f), 80, 24);
            mSaveButton.Pressed += delegate(object sender, EventArgs arguments) { SaveClicked(sender, arguments); };

            mCancelButton.Text = "Cancel";
            mCancelButton.Bounds = new UniRectangle(new UniScalar(1.0f, -90.0f), new UniScalar(1.0f, -40.0f), 80, 24);
            mCancelButton.Pressed += delegate(object sender, EventArgs arguments) { CancelClicked(sender, arguments); };

            Bounds = new UniRectangle(10.0f, 10.0f, 400.0f, 140.0f);
            mBounds = Bounds;

            // Add components to GUI
            Children.Add(mSaveLevelLabel);
            Children.Add(mNameInput);
            Children.Add(mSaveButton);
            Children.Add(mCancelButton);

        }

        #endregion

        private void SaveClicked(object sender, EventArgs arguments)
        {
            GameMapEditor.Map.Save(mNameInput.Text.ToString());
            GameMapEditor.ToggleState(States.None);
        }

        private void CancelClicked(object sender, EventArgs arguments)
        {
            GameMapEditor.ToggleState(States.None);
        }

    }
}
