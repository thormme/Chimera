using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameConstructLibrary
{
    public class GamePadThumbStickInputAction : InputAction
    {
        GamePadThumbStick mGamePadThumbStick;
        GamePadThumbStickAxis mGamePadThumbStickAxis;
        GamePadDeadZone mGamePadDeadZone;
        float mDeadZoneMin;
        float mDeadZoneMax;

        /// <summary>
        /// Constructs an InputAction which tracks GamePad thumb sticks.
        /// </summary>
        /// <param name="playerIndex">The index of the player using this input.</param>
        /// <param name="buttonAction">What state the button should be in to activate.</param>
        /// <param name="thumbStick">Which thumb stick to track.</param>
        /// <param name="thumbStickAxis">The axis of movement to be tracked.</param>
        /// <param name="deadZoneType">The type of dead zone.</param>
        /// <param name="deadZoneMin">The minimum Degree which will not be sensed.</param>
        /// <param name="deadZoneMax">The maximum Degree which will not be sensed.</param>
        public GamePadThumbStickInputAction(
            PlayerIndex playerIndex,
            ButtonAction buttonAction,
            GamePadThumbStick thumbStick,
            GamePadThumbStickAxis thumbStickAxis,
            GamePadDeadZone deadZoneType,
            float deadZoneMin,
            float deadZoneMax)
            : base(playerIndex, buttonAction)
        {
            mGamePadThumbStick = thumbStick;
            mGamePadThumbStickAxis = thumbStickAxis;
            mGamePadDeadZone = deadZoneType;
            mDeadZoneMax = deadZoneMax;
            mDeadZoneMin = deadZoneMin;
        }

        protected override bool IsDown()
        {
            float degree = GetDegree();
            return (degree < mDeadZoneMin || degree > mDeadZoneMax);
        }

        protected override float GetDegree()
        {
            GamePadState gamePad = Microsoft.Xna.Framework.Input.GamePad.GetState(mPlayerIndex, mGamePadDeadZone);
            GamePadThumbSticks sticks = gamePad.ThumbSticks;
            Vector2 stick;
            switch (mGamePadThumbStick)
            {
                case GamePadThumbStick.Left:
                    stick = sticks.Left;
                    break;
                case GamePadThumbStick.Right:
                default:
                    stick = sticks.Right;
                    break;
            }

            switch (mGamePadThumbStickAxis)
            {
                case GamePadThumbStickAxis.X:
                    return stick.X;
                case GamePadThumbStickAxis.Y:
                default:
                    return stick.Y;
            }
        }
    }
}
