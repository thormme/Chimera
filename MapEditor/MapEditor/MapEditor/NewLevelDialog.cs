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
    public class NewLevelDialog : WindowControl
    {
        private MapEditorDialog mMapEditor;
        private Nuclex.UserInterface.Controls.LabelControl mNewLevelLabel;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mHeightInput;
        private Nuclex.UserInterface.Controls.LabelControl mDimensionX;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mWidthInput;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mCreateButton;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mCancelButton;


        public NewLevelDialog(MapEditorDialog mapEditor) :
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
            mNewLevelLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mHeightInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mDimensionX = new Nuclex.UserInterface.Controls.LabelControl();
            mWidthInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mCreateButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();
            mCancelButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();

            // Position components
            mNewLevelLabel.Text = "Dimensions:";
            mNewLevelLabel.Bounds = new UniRectangle(20.0f, 50.0f, 100.0f, 30.0f);

            mHeightInput.Bounds = new UniRectangle(120.0f, 50.0f, 100.0f, 30.0f);

            mDimensionX.Text = "x";
            mDimensionX.Bounds = new UniRectangle(235.0f, 50.0f, 10.0f, 30.0f);

            mWidthInput.Bounds = new UniRectangle(260.0f, 50.0f, 100.0f, 30.0f);

            mCreateButton.Text = "Create";
            mCreateButton.Bounds = new UniRectangle(new UniScalar(1.0f, -180.0f), new UniScalar(1.0f, -40.0f), 80, 24);
            mCreateButton.Pressed += delegate(object sender, EventArgs arguments) { CreateClicked(sender, arguments); };

            mCancelButton.Text = "Cancel";
            mCancelButton.Bounds = new UniRectangle(new UniScalar(1.0f, -90.0f), new UniScalar(1.0f, -40.0f), 80, 24);
            mCancelButton.Pressed += delegate(object sender, EventArgs arguments) { CancelClicked(sender, arguments); };

            Bounds = new UniRectangle(10.0f, 10.0f, 400.0f, 140.0f);

            // Add components to GUI
            Children.Add(mNewLevelLabel);
            Children.Add(mHeightInput);
            Children.Add(mDimensionX);
            Children.Add(mWidthInput);
            Children.Add(mCreateButton);
            Children.Add(mCancelButton);

        }

        #endregion // Not component designer generated code

        private void CreateClicked(object sender, EventArgs arguments)
        {
            try
            {
                mMapEditor.DummyLevel = new DummyLevel(Convert.ToInt32(mHeightInput.Text.ToString()), Convert.ToInt32(mWidthInput.Text.ToString()));
            }
            catch (SystemException)
            {
                // Error in input
            }
            Close();
        }

        private void CancelClicked(object sender, EventArgs arguments)
        {
            Close();
        }

    }
}
