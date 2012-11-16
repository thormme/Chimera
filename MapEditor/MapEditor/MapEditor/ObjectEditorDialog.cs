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
using GraphicsLibrary;
using System.IO;

namespace MapEditor
{
    /// <summary>
    /// Creates a map editor menu
    /// </summary>
    public class ObjectEditorDialog : WindowControl
    {
        private MapEditor mMapEditor;
        private Nuclex.UserInterface.Controls.LabelControl mObjectLabel;
        private Nuclex.UserInterface.Controls.Desktop.ListControl mObjectList;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mDoneButton;

        public ObjectEditorDialog(MapEditor mapEditor) :
            base()
        {
            mMapEditor = mapEditor;
            InitializeComponent();
            PopulateList();
        }

        #region Not component designer generated code

        /// <summary>
        /// Adds items to dialog
        /// </summary>
        private void InitializeComponent()
        {

            BringToFront();

            // Declare all components
            mObjectLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mObjectList = new Nuclex.UserInterface.Controls.Desktop.ListControl();
            mDoneButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();

            // Position components
            mObjectLabel.Text = "Object:";
            mObjectLabel.Bounds = new UniRectangle(20.0f, 40.0f, 100.0f, 30.0f);

            mObjectList.Bounds = new UniRectangle(20.0f, 70.0f, 240.0f, 200.0f);
            mObjectList.Slider.Bounds.Location.X.Offset -= 1.0f;
            mObjectList.Slider.Bounds.Location.Y.Offset += 1.0f;
            mObjectList.Slider.Bounds.Size.Y.Offset -= 2.0f;

            mObjectList.SelectionMode = Nuclex.UserInterface.Controls.Desktop.ListSelectionMode.Single;

            mDoneButton.Text = "Done";
            mDoneButton.Bounds = new UniRectangle(new UniScalar(1.0f, -90.0f), new UniScalar(1.0f, -40.0f), 80, 24);
            mDoneButton.Pressed += delegate(object sender, EventArgs arguments) { DoneClicked(sender, arguments); };

            // Add components to GUI
            Children.Add(mObjectLabel);
            Children.Add(mObjectList);
            Children.Add(mDoneButton);

        }

        #endregion // Not component designer generated code

        private void PopulateList()
        {

            DirectoryInfo di = new DirectoryInfo(DirectoryManager.GetRoot() + "finalProject/finalProjectContent/objects/");
            FileInfo[] objects = di.GetFiles("*");
            foreach (FileInfo obj in objects)
            {
                mObjectList.Items.Add(obj.Name);
            }

        }

        public bool GetObjectEditorInput(out string objectType, out string objectModel, out string[] objectParameters)
        {

            objectType = "None";
            objectModel = "dude";
            objectParameters = new string[0];

            try
            {

                string selected = mObjectList.Items.ElementAt<string>(mObjectList.SelectedItems[0]);
                string[] data = System.IO.File.ReadAllLines(DirectoryManager.GetRoot() + "finalProject/finalProjectContent/objects/" + selected);

                objectType = data[0];
                objectModel = data[1];
                List<string> parameters = new List<string>();
                if (data.Length - 2 > 0)
                {
                    for (int count = 0; count < data.Length - 2; count++)
                    {
                        parameters.Add(data[count + 2]);
                    }
                }
                objectParameters = parameters.ToArray();
            }
            catch (SystemException)
            {
                return false;
            }

            return true;

        }

        public void ObjectsEnable()
        {
            Bounds = new UniRectangle(10.0f, 10.0f, 280.0f, 340.0f);
        }

        private void DoneClicked(object sender, EventArgs arguments)
        {
            Bounds = new UniRectangle(-1000.0f, -1000.0f, 0.0f, 0.0f);
            mMapEditor.MapEditorDialog.Done();
        }

    }
}