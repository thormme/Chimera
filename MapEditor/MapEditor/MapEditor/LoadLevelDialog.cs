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
using System.IO;

namespace MapEditor
{
    /// <summary>
    /// Creates a map editor menu
    /// </summary>
    public class LoadLevelDialog : WindowControl
    {
        private MapEditorDialog mMapEditor;
        private Nuclex.UserInterface.Controls.LabelControl mLoadLevelLabel;
        private Nuclex.UserInterface.Controls.Desktop.ListControl mLevelList;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mLoadButton;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mCancelButton;

        public LoadLevelDialog(MapEditorDialog mapEditor) :
            base()
        {
            mMapEditor = mapEditor;
            InitializeComponent();
            PopulateList();
        }

        #region Not component Designer generated code

        /// <summary>
        /// Adds items to dialog
        /// </summary>
        private void InitializeComponent()
        {

            BringToFront();

            // Declare all components
            mLoadLevelLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mLevelList = new Nuclex.UserInterface.Controls.Desktop.ListControl();
            mLoadButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();
            mCancelButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();

            // Position components
            mLoadLevelLabel.Text = "Levels:";
            mLoadLevelLabel.Bounds = new UniRectangle(20.0f, 40.0f, 100.0f, 30.0f);

            mLevelList.Bounds = new UniRectangle(20.0f, 70.0f, 240.0f, 200.0f);
            mLevelList.Slider.Bounds.Location.X.Offset -= 1.0f;
            mLevelList.Slider.Bounds.Location.Y.Offset += 1.0f;
            mLevelList.Slider.Bounds.Size.Y.Offset -= 2.0f;

            mLevelList.SelectionMode = Nuclex.UserInterface.Controls.Desktop.ListSelectionMode.Single;

            mLoadButton.Text = "Load";
            mLoadButton.Bounds = new UniRectangle(new UniScalar(1.0f, -180.0f), new UniScalar(1.0f, -40.0f), 80, 24);
            mLoadButton.Pressed += delegate(object sender, EventArgs arguments) { LoadClicked(sender, arguments); };

            mCancelButton.Text = "Cancel";
            mCancelButton.Bounds = new UniRectangle(new UniScalar(1.0f, -90.0f), new UniScalar(1.0f, -40.0f), 80, 24);
            mCancelButton.Pressed += delegate(object sender, EventArgs arguments) { CancelClicked(sender, arguments); };

            Bounds = new UniRectangle(10.0f, 10.0f, 280.0f, 340.0f);

            // Add components to GUI
            Children.Add(mLoadLevelLabel);
            Children.Add(mLevelList);
            Children.Add(mLoadButton);
            Children.Add(mCancelButton);

        }

        #endregion // Not component designer generated code

        private void PopulateList()
        {
            
            DirectoryInfo di = new DirectoryInfo(Globals.LevelPath);
            FileInfo[] levels = di.GetFiles("*");
            foreach (FileInfo level in levels)
            {
                mLevelList.Items.Add(level.Name);
            }

        }

        private void LoadClicked(object sender, EventArgs arguments)
        {
            string selected = mLevelList.Items.ElementAt<string>(mLevelList.SelectedItems[0]);
            mMapEditor.DummyLevel.Load(selected);
            Close();
        }

        private void CancelClicked(object sender, EventArgs arguments)
        {
            Close();
        }

    }
}
