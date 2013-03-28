using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace WorldEditor
{
    public class Controls
    {

        public MouseState MouseState = Mouse.GetState();

        public KeyInputAction Forward = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.W);
        public KeyInputAction Backward = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.S);
        public KeyInputAction Left = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.A);
        public KeyInputAction Right = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.D);

        public KeyInputAction Inverse = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.D1);
        public KeyInputAction Smooth = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.D2);
        public KeyInputAction Flatten = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.D2);

        public KeyInputAction Control = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.LeftControl);
        public KeyInputAction Alt = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.LeftAlt);
        public KeyInputAction Shift = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.LeftShift);

        public KeyInputAction Undo = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.Z);
        public KeyInputAction Redo = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.Y);
        public KeyInputAction Delete = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.Delete);
        public KeyInputAction Increase = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.OemPlus);
        public KeyInputAction Decrease = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.OemMinus);

        public MouseButtonInputAction LeftPressed = new MouseButtonInputAction(0, InputAction.ButtonAction.Pressed, InputAction.MouseButton.Left);
        public MouseButtonInputAction LeftHold = new MouseButtonInputAction(0, InputAction.ButtonAction.Down, InputAction.MouseButton.Left);
        public MouseButtonInputAction LeftReleased = new MouseButtonInputAction(0, InputAction.ButtonAction.Released, InputAction.MouseButton.Left);

        public MouseButtonInputAction RightPressed = new MouseButtonInputAction(0, InputAction.ButtonAction.Pressed, InputAction.MouseButton.Right);
        public MouseButtonInputAction RightHold = new MouseButtonInputAction(0, InputAction.ButtonAction.Down, InputAction.MouseButton.Right);
        public MouseButtonInputAction RightReleased = new MouseButtonInputAction(0, InputAction.ButtonAction.Released, InputAction.MouseButton.Right);

        public Controls()
        {

        }

        public void Update(GameTime gameTime)
        {
            
            InputAction.Update();
            MouseState = Mouse.GetState();

        }

    }

}
