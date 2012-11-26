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
using GameConstructLibrary;

namespace MapEditor
{

    /// <summary>
    /// Creates a map editor menu
    /// </summary>
    public class MapEditorDialog : Dialog
    {

        private MapEditor mMapEditor;
        public MapEditor MapEditor { get { return mMapEditor; } set { mMapEditor = value; } }

        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mEditHeightsButton;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mEditObjectsButton;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mNewButton;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mSaveButton;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mLoadButton;

        // Creates a new level that is 100x100 by default
        public MapEditorDialog() :
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
            
            // Declare all components
            mEditHeightsButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();
            mEditObjectsButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();
            mNewButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();
            mSaveButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();
            mLoadButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();

            // Position components

            mEditHeightsButton.Text = "Edit Heights";
            mEditHeightsButton.Bounds = new UniRectangle(new UniScalar(0.0f, 20.0f), new UniScalar(0.0f, 40.0f), 120, 24);
            mEditHeightsButton.Pressed += delegate(object sender, EventArgs arguments) { HeightClicked(sender, arguments); };

            mEditObjectsButton.Text = "Edit Objects";
            mEditObjectsButton.Bounds = new UniRectangle(new UniScalar(0.0f, 20.0f), new UniScalar(0.0f, 80.0f), 120, 24);
            mEditObjectsButton.Pressed += delegate(object sender, EventArgs arguments) { ObjectClicked(sender, arguments); };

            mNewButton.Text = "New";
            mNewButton.Bounds = new UniRectangle(new UniScalar(0.0f, 20.0f), new UniScalar(1.0f, -40.0f), 80, 24);
            mNewButton.Pressed += delegate(object sender, EventArgs arguments) { NewClicked(sender, arguments); };

            mSaveButton.Text = "Save";
            mSaveButton.Bounds = new UniRectangle(new UniScalar(0.0f, 110.0f), new UniScalar(1.0f, -40.0f), 80, 24);
            mSaveButton.Pressed += delegate(object sender, EventArgs arguments) { SaveClicked(sender, arguments); };

            mLoadButton.Text = "Load";
            mLoadButton.Bounds = new UniRectangle(new UniScalar(0.0f, 200.0f), new UniScalar(1.0f, -40.0f), 80, 24);
            mLoadButton.Pressed += delegate(object sender, EventArgs arguments) { LoadClicked(sender, arguments); };

            Bounds = new UniRectangle(10.0f, 10.0f, 300.0f, 160.0f);
            mBounds = Bounds;

            // Add components to GUI
            Children.Add(mEditHeightsButton);
            Children.Add(mEditObjectsButton);
            Children.Add(mNewButton);
            Children.Add(mSaveButton);
            Children.Add(mLoadButton);

        }

        #endregion

        private void HeightClicked(object sender, EventArgs arguments)
        {
            GameMapEditor.ToggleState(States.Height);
        }

        private void ObjectClicked(object sender, EventArgs arguments)
        {
            GameMapEditor.ToggleState(States.Object);
        }

        private void NewClicked(object sender, EventArgs arguments)
        {
            GameMapEditor.ToggleState(States.New);
        }

        private void SaveClicked(object sender, EventArgs arguments)
        {
            GameMapEditor.ToggleState(States.Save);
        }

        private void LoadClicked(object sender, EventArgs arguments)
        {
            GameMapEditor.ToggleState(States.Load);
        }

    }
}
