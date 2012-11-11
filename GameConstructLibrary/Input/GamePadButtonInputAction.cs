using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    public class GamePadButtonInputAction : InputAction
    {
        Buttons mButton;

        /// <summary>
        /// Constructs an InputAction which tracks GamePad buttons.
        /// </summary>
        /// <param name="playerIndex">The index of the player using this input.</param>
        /// <param name="buttonAction">What state the button should be in to activate.</param>
        /// <param name="button">The button to track.</param>
        public GamePadButtonInputAction(PlayerIndex playerIndex, ButtonAction buttonAction, Buttons button)
            : base(playerIndex, buttonAction)
        {
            mPlayerIndex = playerIndex;
            mButton = button;
        }

        protected override bool IsDown()
        {
            GamePadState gamePad = Microsoft.Xna.Framework.Input.GamePad.GetState(mPlayerIndex);
            return gamePad.IsButtonDown(mButton);
        }
    }
}
