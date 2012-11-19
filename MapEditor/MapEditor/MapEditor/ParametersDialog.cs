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
    public class ParametersDialog : WindowControl
    {

        private List<string> mParameters;

        private List<Nuclex.UserInterface.Controls.LabelControl> mParameterLabel;
        private List<Nuclex.UserInterface.Controls.Desktop.InputControl> mParameterInput;

        public ParametersDialog(List<string> parameters) :
            base()
        {
            mParameters = parameters;
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
            mParameterLabel = new List<Nuclex.UserInterface.Controls.LabelControl>();
            mParameterInput = new List<Nuclex.UserInterface.Controls.Desktop.InputControl>();

            float location = 40.0f;

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

            Bounds = new UniRectangle(10.0f, 10.0f, 420.0f, location);

        }

        #endregion // Not component designer generated code

        public List<string> GetParameters()
        {
            List<string> parameters = new List<string>();
            for (int count = 0; count < mParameters.Count(); count++)
            {
                string parameter = mParameters[count].Split(' ')[0] + " " + mParameterInput[count].Text;
                Console.WriteLine(parameter);
                parameters.Add(parameter);
            }
            return parameters;
        }

    }
}