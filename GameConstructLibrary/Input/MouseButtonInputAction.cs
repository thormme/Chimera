using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameConstructLibrary
{
    public class MouseButtonInputAction : InputAction
    {
        MouseButton mButton;

        /// <summary>
        /// Constructs an InputAction which tracks mouse buttons.
        /// </summary>
        /// <param name="playerIndex">The index of the player using this input.</param>
        /// <param name="buttonAction">What state the button should be in to activate.</param>
        /// <param name="button">The button to track</param>
        public MouseButtonInputAction(PlayerIndex playerIndex, ButtonAction buttonAction, MouseButton button)
            : base(playerIndex, buttonAction)
        {
            mButton = button;
        }

        protected override bool IsDown()
        {
            MouseState mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
            switch (mButton) {
                case MouseButton.Left:
                    return mouse.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return mouse.RightButton == ButtonState.Pressed;
                case MouseButton.Middle:
                default:
                    return mouse.MiddleButton == ButtonState.Pressed;
            }
        }
    }
}
