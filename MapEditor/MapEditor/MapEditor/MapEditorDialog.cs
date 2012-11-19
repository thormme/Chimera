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
    public class MapEditorDialog : WindowControl
    {

        private MapEditor mMapEditor;
        public MapEditor MapEditor { get { return mMapEditor; } set { mMapEditor = value; } }

        private Screen mMainScreen;

        private HeightMapEditorDialog mHeightMapEditorDialog;
        private ObjectEditorDialog mObjectEditorDialog;

        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mEditHeightsButton;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mEditObjectsButton;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mNewButton;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mSaveButton;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mLoadButton;

        // Creates a new level that is 100x100 by default
        public MapEditorDialog(MapEditor mapEditor, Screen mainScreen) :
            base()
        {
            mMainScreen = mainScreen;
            mMapEditor = mapEditor;

            mHeightMapEditorDialog = new HeightMapEditorDialog(mMapEditor);
            mMainScreen.Desktop.Children.Add(mHeightMapEditorDialog);

            mObjectEditorDialog = new ObjectEditorDialog(mMapEditor);
            mMainScreen.Desktop.Children.Add(mObjectEditorDialog);

            InitializeComponent();
        }

        #region Not component designer generated code

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

            Done();

            // Add components to GUI
            Children.Add(mEditHeightsButton);
            Children.Add(mEditObjectsButton);
            Children.Add(mNewButton);
            Children.Add(mSaveButton);
            Children.Add(mLoadButton);

        }

        #endregion // Not component designer generated code

        public void Done()
        {
            Bounds = new UniRectangle(10.0f, 10.0f, 300.0f, 160.0f);
            mHeightMapEditorDialog.Bounds = new UniRectangle(-1000.0f, -1000.0f, 0.0f, 0.0f);
            mObjectEditorDialog.Bounds = new UniRectangle(-1000.0f, -1000.0f, 0.0f, 0.0f);
            mMapEditor.State = States.None;
        }

        public bool GetHeightEditorInput(out int size, out int intensity, out bool feather, out bool set)
        {
            return mHeightMapEditorDialog.GetHeightEditorInput(out size, out intensity, out feather, out set);
        }

        public bool GetObjectEditorInput(out string objectType, out string objectModel, out Vector3 objectScale, out Vector3 objectOrientation, out string[] parameterModel)
        {
            return mObjectEditorDialog.GetObjectEditorInput(out objectType, out objectModel, out objectScale, out objectOrientation, out parameterModel);
        }

        public void Disable()
        {
            Bounds = new UniRectangle(-1000.0f, -1000.0f, 0.0f, 0.0f);
            mHeightMapEditorDialog.Bounds = new UniRectangle(-1000.0f, -1000.0f, 0.0f, 0.0f);
            mObjectEditorDialog.Bounds = new UniRectangle(-1000.0f, -1000.0f, 0.0f, 0.0f);
            mMainScreen.FocusedControl = this;
        }

        public void Enable()
        {
            if (mMapEditor.State == States.None)
            {
                Bounds = new UniRectangle(10.0f, 10.0f, 300.0f, 160.0f);
            }
            else if (mMapEditor.State == States.Height)
            {
                mHeightMapEditorDialog.HeightsEnable();
            }
            else if (mMapEditor.State == States.Object)
            {
                mObjectEditorDialog.ObjectsEnable();
            }
        }

        private void HeightClicked(object sender, EventArgs arguments)
        {
            mMapEditor.State = States.Height;
            Bounds = new UniRectangle(-1000.0f, -1000.0f, 0.0f, 0.0f);
            mHeightMapEditorDialog.HeightsEnable();
        }

        private void ObjectClicked(object sender, EventArgs arguments)
        {
            mMapEditor.State = States.Object;
            Bounds = new UniRectangle(-1000.0f, -1000.0f, 0.0f, 0.0f);
            mObjectEditorDialog.ObjectsEnable();
        }

        private void NewClicked(object sender, EventArgs arguments)
        {
            mMainScreen.Desktop.Children.Add(new NewLevelDialog(mMapEditor));
        }

        private void SaveClicked(object sender, EventArgs arguments)
        {
            mMainScreen.Desktop.Children.Add(new SaveLevelDialog(mMapEditor));
        }

        private void LoadClicked(object sender, EventArgs arguments)
        {
            mMainScreen.Desktop.Children.Add(new LoadLevelDialog(mMapEditor));
        }

    }
}
