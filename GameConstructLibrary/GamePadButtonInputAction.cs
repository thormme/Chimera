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
        ButtonAction mButtonAction;
        Buttons mButton;

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
