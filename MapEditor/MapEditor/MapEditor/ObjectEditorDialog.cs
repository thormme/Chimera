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
        private Nuclex.UserInterface.Controls.LabelControl mScaleLabel;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mScaleXInput;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mScaleYInput;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mScaleZInput;
        private Nuclex.UserInterface.Controls.LabelControl mOrientationLabel;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mOrientationXInput;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mOrientationYInput;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mOrientationZInput;
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
            mScaleLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mScaleXInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mScaleYInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mScaleZInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mOrientationLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mOrientationXInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mOrientationYInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mOrientationZInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mDoneButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();

            // Position components
            mObjectLabel.Text = "Object:";
            mObjectLabel.Bounds = new UniRectangle(20.0f, 40.0f, 100.0f, 30.0f);

            mObjectList.Bounds = new UniRectangle(20.0f, 70.0f, 240.0f, 200.0f);
            mObjectList.Slider.Bounds.Location.X.Offset -= 1.0f;
            mObjectList.Slider.Bounds.Location.Y.Offset += 1.0f;
            mObjectList.Slider.Bounds.Size.Y.Offset -= 2.0f;

            mObjectList.SelectionMode = Nuclex.UserInterface.Controls.Desktop.ListSelectionMode.Single;

            mScaleLabel.Text = "Scale:";
            mScaleLabel.Bounds = new UniRectangle(20.0f, 280.0f, 50.0f, 30.0f);
            mScaleXInput.Bounds = new UniRectangle(70.0f, 280.0f, 30.0f, 30.0f);
            mScaleXInput.Text = "1";
            mScaleYInput.Bounds = new UniRectangle(110.0f, 280.0f, 30.0f, 30.0f);
            mScaleYInput.Text = "1";
            mScaleZInput.Bounds = new UniRectangle(150.0f, 280.0f, 30.0f, 30.0f);
            mScaleZInput.Text = "1";

            mOrientationLabel.Text = "Orientation:";
            mOrientationLabel.Bounds = new UniRectangle(20.0f, 320.0f, 90.0f, 30.0f);
            mOrientationXInput.Bounds = new UniRectangle(110.0f, 320.0f, 30.0f, 30.0f);
            mOrientationXInput.Text = "0";
            mOrientationYInput.Bounds = new UniRectangle(150.0f, 320.0f, 30.0f, 30.0f);
            mOrientationYInput.Text = "0";
            mOrientationZInput.Bounds = new UniRectangle(190.0f, 320.0f, 30.0f, 30.0f);
            mOrientationZInput.Text = "1";

            mDoneButton.Text = "Done";
            mDoneButton.Bounds = new UniRectangle(new UniScalar(1.0f, -90.0f), new UniScalar(1.0f, -40.0f), 80, 24);
            mDoneButton.Pressed += delegate(object sender, EventArgs arguments) { DoneClicked(sender, arguments); };

            // Add components to GUI
            Children.Add(mObjectLabel);
            Children.Add(mObjectList);
            Children.Add(mScaleLabel);
            Children.Add(mScaleXInput);
            Children.Add(mScaleYInput);
            Children.Add(mScaleZInput);
            Children.Add(mOrientationLabel);
            Children.Add(mOrientationXInput);
            Children.Add(mOrientationYInput);
            Children.Add(mOrientationZInput);
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

        public bool GetObjectEditorInput(out string objectType, out string objectModel, out Vector3 objectScale, out Vector3 objectOrientation, out string[] objectParameters)
        {

            objectType = "Root";
            objectModel = "sphere";
            objectScale = new Vector3(1.0f, 1.0f, 1.0f);
            objectOrientation = new Vector3(0.0f, 0.0f, 1.0f);
            
            objectParameters = new string[0];

            try
            {

                string selected = mObjectList.Items.ElementAt<string>(mObjectList.SelectedItems[0]);
                string[] data = System.IO.File.ReadAllLines(DirectoryManager.GetRoot() + "finalProject/finalProjectContent/objects/" + selected);

                objectType = data[0];
                objectModel = data[1];
                objectScale = new Vector3(Convert.ToSingle(mScaleXInput.Text), Convert.ToSingle(mScaleYInput.Text), Convert.ToSingle(mScaleZInput.Text));
                objectOrientation = new Vector3(Convert.ToSingle(mOrientationXInput.Text), Convert.ToSingle(mOrientationYInput.Text), Convert.ToSingle(mOrientationZInput.Text));
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
            Bounds = new UniRectangle(10.0f, 10.0f, 280.0f, 420.0f);
        }

        private void DoneClicked(object sender, EventArgs arguments)
        {
            Bounds = new UniRectangle(-1000.0f, -1000.0f, 0.0f, 0.0f);
            mMapEditor.MapEditorDialog.Done();
        }

    }
}