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
using GameConstructLibrary;
using GraphicsLibrary;

namespace MapEditor
{
    /// <summary>
    /// Creates a map editor menu
    /// </summary>
    public class HotkeyDialog : Dialog
    {

        private Nuclex.UserInterface.Controls.LabelControl mHotkeyLabel;

        public HotkeyDialog() :
            base()
        {
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
            mHotkeyLabel = new Nuclex.UserInterface.Controls.LabelControl();

            // Position components
            mHotkeyLabel.Text = "Undo: Ctrl + Z\nRedo: Ctrl + Shift + Z\n\nMove Selected: (+, -) and Arrows\nScale Selected: Ctrl + (+, -)\nOrient Selected: Shift + (+, -)\n\nHeight Inverse: 1\nHeight Smooth: 2\nHeight Flatten: 3\n\n(~ to toggle this window)";
            mHotkeyLabel.Bounds = new UniRectangle(20.0f, 40.0f, 100.0f, 30.0f);

            Bounds = new UniRectangle(new UniScalar(1.0f, -300.0f), 10.0f, 280.0f, 300.0f);
            mBounds = Bounds;

            // Add components to GUI
            Children.Add(mHotkeyLabel);

        }

        #endregion // Not component designer generated code

    }
}
