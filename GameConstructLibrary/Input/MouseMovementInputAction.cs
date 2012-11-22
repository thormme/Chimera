using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameConstructLibrary
{
    public class MouseMovementInputAction : InputAction
    {
        MouseAxis mMouseAxis;
        float mSensitivity;
        float mDeadZoneMin;
        float mDeadZoneMax;

        int previousX;
        int previousY;

        /// <summary>
        /// Constructs an InputAction which tracks relative mouse movement.
        /// </summary>
        /// <param name="playerIndex">The index of the player using this input.</param>
        /// <param name="buttonAction">What state the button should be in to activate.</param>
        /// <param name="axis">The axis of movement to be tracked.</param>
        /// <param name="sensitivity">
        /// How sensitive the mouse is.
        /// A sensitivity of 4.0 and movement of 2 pixels will have a Degree of 0.5.</param>
        /// <param name="deadZoneMin">The minimum Degree which will not be sensed.</param>
        /// <param name="deadZoneMax">The maximum Degree which will not be sensed.</param>
        public MouseMovementInputAction(
            PlayerIndex playerIndex,
            ButtonAction buttonAction,
            MouseAxis axis,
            float sensitivity,
            float deadZoneMin,
            float deadZoneMax)
            : base(playerIndex, buttonAction)
        {
            mMouseAxis = axis;
            mSensitivity = sensitivity;
            mDeadZoneMax = deadZoneMax;
            mDeadZoneMin = deadZoneMin;
            previousX = Microsoft.Xna.Framework.Input.Mouse.GetState().X;
            previousY = Microsoft.Xna.Framework.Input.Mouse.GetState().Y;
        }

        protected override void StepInputAction()
        {
            base.StepInputAction();
            MouseState mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
            if (IsMouseLocked)
            {
                previousX = (int) mMouseLockedPosition.X;
                previousY = (int) mMouseLockedPosition.Y;
            }
            else
            {
                previousX = mouse.X;
                previousY = mouse.Y;
            }
        }

        protected override bool IsDown()
        {
            float degree = GetDegree();
            return (degree < mDeadZoneMin || degree > mDeadZoneMax);
        }

        protected override float GetDegree()
        {
            MouseState mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();

            float degree = 0.0f;
            
            switch (mMouseAxis)
            {
                case MouseAxis.X:
                    degree = (float)(mouse.X - previousX) / mSensitivity;
                    break;
                case MouseAxis.Y:
                    degree = (float)(mouse.Y - previousY) / mSensitivity;
                    break;
                default:
                    break;
            }

            return degree;
        }
    }
}
