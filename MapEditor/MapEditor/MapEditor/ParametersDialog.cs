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
    public class ParametersDialog : Dialog
    {

        private List<string> mParameters;

        private Nuclex.UserInterface.Controls.LabelControl mEditModeLabel;
        private List<Nuclex.UserInterface.Controls.LabelControl> mParameterLabel;
        private List<Nuclex.UserInterface.Controls.Desktop.InputControl> mParameterInput;

        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mDummyButton;
        private Nuclex.UserInterface.Controls.Desktop.ButtonControl mBackButton;

        public ParametersDialog(List<string> parameters) :
            base()
        {
            mParameters = parameters;
            InitializeComponent();
        }

        #region Component Layout

        /// <summary>
        /// Adds items to dialog
        /// </summary>
        private void InitializeComponent()
        {

            mEditModeLabel = new Nuclex.UserInterface.Controls.LabelControl();
            mEditModeLabel.Text = "Edit Mode";
            mEditModeLabel.Bounds = new UniRectangle(20.0f, 40.0f, 100.0f, 30.0f);

            Children.Add(mEditModeLabel);

            // Declare all components
            mParameterLabel = new List<Nuclex.UserInterface.Controls.LabelControl>();
            mParameterInput = new List<Nuclex.UserInterface.Controls.Desktop.InputControl>();

            float location = 90.0f;

            foreach (string parameter in mParameters)
            {
                Nuclex.UserInterface.Controls.LabelControl tempLabel = new Nuclex.UserInterface.Controls.LabelControl();
                tempLabel.Text = parameter;
                tempLabel.Bounds = new UniRectangle(20.0f, location, 140.0f, 30.0f);

                Nuclex.UserInterface.Controls.Desktop.InputControl tempInput = new Nuclex.UserInterface.Controls.Desktop.InputControl();
                tempInput.Bounds = new UniRectangle(180.0f, location, 220.0f, 30.0f);
                
                mParameterLabel.Add(tempLabel);
                mParameterInput.Add(tempInput);
                Children.Add(tempLabel);
                Children.Add(tempInput);
                location += 50.0f;
            }

            mDummyButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();

            mDummyButton.Text = "Dummy";
            mDummyButton.Bounds = new UniRectangle(new UniScalar(1.0f, -180.0f), location, 80, 24);

            Children.Add(mDummyButton);

            mBackButton = new Nuclex.UserInterface.Controls.Desktop.ButtonControl();

            mBackButton.Text = "Back";
            mBackButton.Bounds = new UniRectangle(new UniScalar(1.0f, -90.0f), location, 80, 24);
            mBackButton.Pressed += delegate(object sender, EventArgs arguments) { BackClicked(sender, arguments); };

            Children.Add(mBackButton);

            location += 34.0f;

            Bounds = new UniRectangle(10.0f, 10.0f, 420.0f, location);
            mBounds = Bounds;

        }

        #endregion

        public List<string> GetParameters()
        {
            List<string> parameters = new List<string>();
            for (int count = 0; count < mParameters.Count(); count++)
            {
                string parameter = mParameterInput[count].Text;
                Console.WriteLine(parameter);
                parameters.Add(parameter);
            }
            return parameters;
        }

        private void BackClicked(object sender, EventArgs arguments)
        {
            GameMapEditor.EditMode = Edit.None;
            GameMapEditor.ReturnState();
        }

    }
}