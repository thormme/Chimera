using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    public class KeyInputAction : InputAction
    {
        Keys mKey;

        /// <summary>
        /// Constructs an InputAction which tracks keyboard keys.
        /// </summary>
        /// <param name="playerIndex">The index of the player using this input.</param>
        /// <param name="buttonAction">What state the button should be in to activate.</param>
        /// <param name="key">The key to track.</param>
        public KeyInputAction(PlayerIndex playerIndex, ButtonAction buttonAction, Keys key)
            : base(playerIndex, buttonAction)
        {
            mKey = key;
        }

        protected override bool IsDown()
        {
            KeyboardState gamePad = Microsoft.Xna.Framework.Input.Keyboard.GetState(mPlayerIndex);
            return gamePad.IsKeyDown(mKey);
        }
    }
}