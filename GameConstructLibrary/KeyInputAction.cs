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
        ButtonAction mButtonAction;
        Keys mKey;

        bool previousDown;

        public KeyInputAction(PlayerIndex playerIndex, ButtonAction buttonAction, Keys key)
            : base(playerIndex, buttonAction)
        {
            mKey = key;
            mButtonAction = buttonAction;
        }

        protected override bool IsDown()
        {
            KeyboardState gamePad = Microsoft.Xna.Framework.Input.Keyboard.GetState(mPlayerIndex);
            return gamePad.IsKeyDown(mKey);
        }
    }
}