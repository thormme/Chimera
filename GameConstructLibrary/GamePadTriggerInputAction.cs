using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameConstructLibrary
{
    public class GamePadTriggerInputAction : InputAction
    {
        GamePadTrigger mGamePadTrigger;
        GamePadDeadZone mGamePadDeadZone;
        float mDeadZoneMin;
        float mDeadZoneMax;

        public GamePadTriggerInputAction(
            PlayerIndex playerIndex,
            ButtonAction buttonAction,
            GamePadTrigger trigger,
            GamePadDeadZone deadZoneType,
            float deadZoneMin,
            float deadZoneMax)
            : base(playerIndex, buttonAction)
        {
            mGamePadTrigger = trigger;
            mGamePadDeadZone = deadZoneType;
            mDeadZoneMax = deadZoneMax;
            mDeadZoneMin = deadZoneMin;
        }

        protected override bool IsDown()
        {
            float degree = GetDegree();
            return (degree > mDeadZoneMin && degree < mDeadZoneMax);
        }

        protected override float GetDegree()
        {
            GamePadState gamePad = Microsoft.Xna.Framework.Input.GamePad.GetState(mPlayerIndex, mGamePadDeadZone);

            switch (mGamePadTrigger)
            {
                case GamePadTrigger.Left:
                    return gamePad.Triggers.Left;
                case GamePadTrigger.Right:
                default:
                    return gamePad.Triggers.Right;
            }
        }
    }
}
