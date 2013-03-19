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
using GameConstructLibrary;

namespace MapEditor
{
    /// <summary>
    /// Creates a map editor menu
    /// </summary>
    public class ObjectEditorDialog : Dialog
    {

        private Dictionary<string, DummyObject> mTypes;

        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mMinimizeMaximize;
        private System.Collections.ObjectModel.Collection<Control> mControls;

        private Nuclex.UserInterface.Controls.LabelControl mObjectLabel;
        private Nuclex.UserInterface.Controls.Desktop.ListControl mObjectList;
        private Nuclex.UserInterface.Controls.LabelControl mScaleLabel;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mScaleXInput;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mScaleYInput;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mScaleZInput;
        private Nuclex.UserInterface.Controls.LabelControl mScaleRandomLabel;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mScaleRandomInput;
        private Nuclex.UserInterface.Controls.LabelControl mOrientationLabel;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mOrientationXInput;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mOrientationYInput;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mOrientationZInput;
        private Nuclex.UserInterface.Controls.LabelControl mOrientationRandomLabel;
        private Nuclex.UserInterface.Controls.Desktop.OptionControl mOrientationRandomOption;
        private Nuclex.UserInterface.Controls.LabelControl mHeightLabel;
        private Nuclex.UserInterface.Controls.Desktop.InputControl mHeightInput;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mPlaceButton;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mBackButton;

        public ObjectEditorDialog() :
            base()
        {
            mTypes = new Dictionary<string, DummyObject>();
            InitializeComponent();
            PopulateList();
        }

        #region Component Layout

        /// <summary>
        /// Adds items to dialog
        /// </summary>
        private void InitializeComponent()
        {

            mControls = new System.Collections.ObjectModel.Collection<Control>();

            // Declare all components
            mMinimizeMaximize = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();
            mObjectLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mObjectList = new Nuclex.UserInterface.Controls.Desktop.ListControl();
            mScaleLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mScaleXInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mScaleYInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mScaleZInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mScaleRandomLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mScaleRandomInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mOrientationLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mOrientationXInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mOrientationYInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mOrientationZInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mOrientationRandomLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mOrientationRandomOption = new Nuclex.UserInterface.Controls.Desktop.OptionControl();
            mHeightLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mHeightInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
            mPlaceButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();
            mBackButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();

            // Position components

            mMinimizeMaximize.Text = "Minimize";
            mMinimizeMaximize.Bounds = new UniRectangle(new UniScalar(0.0f, 10.0f), new UniScalar(1.0f, -40.0f), 80.0f, 24.0f);
            mMinimizeMaximize.Pressed += delegate(object sender, EventArgs arguments) { MinimizeMaximizeClicked(sender, arguments); };

            mObjectLabel.Text = "Object:";
            mObjectLabel.Bounds = new UniRectangle(20.0f, 40.0f, 100.0f, 30.0f);

            mObjectList.Bounds = new UniRectangle(20.0f, 70.0f, 360.0f, 200.0f);
            mObjectList.Slider.Bounds.Location.X.Offset -= 1.0f;
            mObjectList.Slider.Bounds.Location.Y.Offset += 1.0f;
            mObjectList.Slider.Bounds.Size.Y.Offset -= 2.0f;

            mObjectList.SelectionMode = Nuclex.UserInterface.Controls.Desktop.ListSelectionMode.Single;

            mScaleLabel.Text = "Scale:";
            mScaleLabel.Bounds = new UniRectangle(62.0f, 280.0f, 50.0f, 30.0f);
            mScaleXInput.Bounds = new UniRectangle(110.0f, 280.0f, 30.0f, 30.0f);
            mScaleXInput.Text = "1";
            mScaleYInput.Bounds = new UniRectangle(150.0f, 280.0f, 30.0f, 30.0f);
            mScaleYInput.Text = "1";
            mScaleZInput.Bounds = new UniRectangle(190.0f, 280.0f, 30.0f, 30.0f);
            mScaleZInput.Text = "1";

            mScaleRandomLabel.Text = "Random:";
            mScaleRandomLabel.Bounds = new UniRectangle(262.0f, 280.0f, 50.0f, 30.0f);
            mScaleRandomInput.Bounds = new UniRectangle(330.0f, 280.0f, 30.0f, 30.0f);
            mScaleRandomInput.Text = "0";

            mOrientationLabel.Text = "Orientation:";
            mOrientationLabel.Bounds = new UniRectangle(20.0f, 320.0f, 90.0f, 30.0f);
            mOrientationXInput.Bounds = new UniRectangle(110.0f, 320.0f, 30.0f, 30.0f);
            mOrientationXInput.Text = "0";
            mOrientationYInput.Bounds = new UniRectangle(150.0f, 320.0f, 30.0f, 30.0f);
            mOrientationYInput.Text = "0";
            mOrientationZInput.Bounds = new UniRectangle(190.0f, 320.0f, 30.0f, 30.0f);
            mOrientationZInput.Text = "0";

            mOrientationRandomLabel.Text = "Random:";
            mOrientationRandomLabel.Bounds = new UniRectangle(262.0f, 320.0f, 50.0f, 30.0f);
            mOrientationRandomOption.Bounds = new UniRectangle(330.0f, 320.0f, 30.0f, 30.0f);

            mHeightLabel.Text = "Height:";
            mHeightLabel.Bounds = new UniRectangle(52.0f, 360.0f, 54.0f, 30.0f);
            mHeightInput.Bounds = new UniRectangle(110.0f, 360.0f, 110.0f, 30.0f);
            mHeightInput.Text = "0";

            mPlaceButton.Text = "Place";
            mPlaceButton.Bounds = new UniRectangle(new UniScalar(1.0f, -180.0f), new UniScalar(1.0f, -40.0f), 80, 24);
            mPlaceButton.Pressed += delegate(object sender, EventArgs arguments) { PlaceClicked(sender, arguments); };

            mBackButton.Text = "Back";
            mBackButton.Bounds = new UniRectangle(new UniScalar(1.0f, -90.0f), new UniScalar(1.0f, -40.0f), 80, 24);
            mBackButton.Pressed += delegate(object sender, EventArgs arguments) { BackClicked(sender, arguments); };

            Bounds = new UniRectangle(10.0f, 10.0f, 400.0f, 460.0f);
            mBounds = new UniRectangle(10.0f, 10.0f, 100.0f, 80.0f);

            // Add components to GUI
            Children.Add(mMinimizeMaximize);
            Children.Add(mObjectLabel);
            Children.Add(mObjectList);
            Children.Add(mScaleLabel);
            Children.Add(mScaleXInput);
            Children.Add(mScaleYInput);
            Children.Add(mScaleZInput);
            Children.Add(mScaleRandomLabel);
            Children.Add(mScaleRandomInput);
            Children.Add(mOrientationLabel);
            Children.Add(mOrientationXInput);
            Children.Add(mOrientationYInput);
            Children.Add(mOrientationZInput);
            Children.Add(mOrientationRandomLabel);
            Children.Add(mOrientationRandomOption);
            Children.Add(mHeightLabel);
            Children.Add(mHeightInput);
            Children.Add(mPlaceButton);
            Children.Add(mBackButton);

            foreach (Control child in Children)
            {
                if (child != mMinimizeMaximize) mControls.Add(child);
            }

        }

        #endregion // Not component designer generated code

        private void PopulateList()
        {

            DirectoryInfo di = new DirectoryInfo(DirectoryManager.GetRoot() + "Chimera/ChimeraContent/objects/");
            FileInfo[] objects = di.GetFiles("*");
            foreach (FileInfo obj in objects)
            {
                mObjectList.Items.Add(obj.Name);

                DummyObject temp = new DummyObject();

                try
                {

                    string[] data = System.IO.File.ReadAllLines(DirectoryManager.GetRoot() + "Chimera/ChimeraContent/objects/" + obj.Name);

                    temp.Type = data[0];
                    temp.Model = data[1];
                    temp.Scale = new Vector3(1.0f, 1.0f, 1.0f);
                    temp.YawPitchRoll = new Vector3(0.0f, 0.0f, 0.0f);
                    List<string> parameters = new List<string>();
                    if (data.Length - 2 > 0)
                    {
                        for (int count = 0; count < data.Length - 2; count++)
                        {
                            parameters.Add(data[count + 2]);
                        }
                    }
                    temp.Parameters = parameters.ToArray();

                }
                catch (SystemException)
                {
                    
                }

                mTypes.Add(obj.Name, temp);

            }

        }

        private void MinimizeMaximizeClicked(object sender, EventArgs arguments)
        {

            if (mMinimizeMaximize.Text == "Minimize")
            {
                mMinimizeMaximize.Text = "Maximize";
                foreach (Control child in mControls)
                {
                    Children.Remove(child);
                }
            }
            else
            {
                mMinimizeMaximize.Text = "Minimize";
                foreach (Control child in mControls)
                {
                    Children.Add(child);
                }
            }

            UniRectangle temp = Bounds;
            Bounds = mBounds;
            mBounds = temp;
        }

        private void PlaceClicked(object sender, EventArgs arguments)
        {
            try
            {
                DummyObject temp = new DummyObject();
                string selected = mObjectList.Items.ElementAt<string>(mObjectList.SelectedItems[0]);
                mTypes.TryGetValue(selected, out temp);

                temp.Scale = new Vector3(Convert.ToSingle(mScaleXInput.Text), Convert.ToSingle(mScaleYInput.Text), Convert.ToSingle(mScaleZInput.Text));
                temp.YawPitchRoll = new Vector3(Convert.ToSingle(mOrientationXInput.Text), Convert.ToSingle(mOrientationYInput.Text), Convert.ToSingle(mOrientationZInput.Text));
                temp.Height = Convert.ToSingle(mHeightInput.Text) / GameMapEditor.MapScale.Y;

                GameMapEditor.Dummy = temp;

                GameMapEditor.RandomOrientation = mOrientationRandomOption.Selected;

                Single randomScale = Convert.ToSingle(mScaleRandomInput.Text);
                if (randomScale > 1.0f) randomScale = 1.0f;
                else if (randomScale < 0.0f) randomScale = 0.0f;
                GameMapEditor.RandomScale = randomScale;

                GameMapEditor.Entity.SetModel(temp);
                GameMapEditor.EditMode = Edit.Object;
                GameMapEditor.ToggleState(States.Parameters);
            }
            catch (SystemException)
            {

            }
            
        }

        private void BackClicked(object sender, EventArgs arguments)
        {
            GameMapEditor.ToggleState(States.None);
        }
        
    }
}