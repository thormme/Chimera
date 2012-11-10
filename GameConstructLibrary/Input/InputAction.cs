using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    /// <summary>
    /// InputAction can track changes in inputs.
    /// Generalize the interface for all input methods.
    /// </summary>
    public class InputAction
    {
        private static List<InputAction> mInputActionList = new List<InputAction>();

        public enum ButtonAction { Pressed, Released, Down};
        public enum GamePadThumbStick { Left, Right };
        public enum GamePadThumbStickAxis { X, Y };
        public enum GamePadTrigger { Left, Right };
        public enum MouseButton { Left, Right, Middle };
        public enum MouseAxis { X, Y };
        private enum InputType { /*/*/GamePadButton, /*/*/GamePadThumbStick, /*/*/GamePadTrigger, /*/*/Key, /**/MouseButton, MousePosition };

        protected PlayerIndex mPlayerIndex;
        protected ButtonAction mButtonAction;

        protected bool previousDown;

        /// <summary>
        /// Whether the input is active. Analog outputs are considered to be down when outside the dead zone.
        /// </summary>
        public bool Active
        {
            get;
            protected set;
        }

        /// <summary>
        /// The degree to which the input is activated. 0 or 1 for binary inputs, -1.0 to 1.0 for analog.
        /// </summary>
        public float Degree
        {
            get;
            protected set;
        }

        /// <summary>
        /// Whether the input is active.
        /// </summary>
        /// <returns>True if active.</returns>
        protected virtual bool IsDown()
        {
            return false;
        }

        /// <summary>
        /// Get the degree to which the input is activated.
        /// </summary>
        /// <returns>The degree, generally between -1.0 and 1.0.</returns>
        protected virtual float GetDegree()
        {
            return IsDown() ? 1.0f : 0.0f;
        }

        /// <summary>
        /// Step the input. Stores current input state and changes since the last update step.
        /// </summary>
        protected virtual void StepInputAction()
        {
            bool down = IsDown();
            bool released = false, pressed = false;
            if (previousDown == true && down == false)
            {
                released = true;
            } 
            else if (previousDown == false && down == true)
            {
                pressed = true;
            }

            previousDown = down;

            switch (mButtonAction)
            {
                case ButtonAction.Pressed:
                    Active = pressed;
                    break;
                case ButtonAction.Released:
                    Active = released;
                    break;
                case ButtonAction.Down:
                default:
                    Active = down;
                    break;
            }

            if (!down)
            {
                Degree = 0.0f;
            }
            else
            {
                Degree = GetDegree();
            }
        }

        /// <summary>
        /// Construct a basic Input Action.
        /// </summary>
        /// <param name="playerIndex">The index of the player using this input.</param>
        /// <param name="buttonAction">What state the button should be in to activate.</param>
        public InputAction(PlayerIndex playerIndex, ButtonAction buttonAction) {
            mPlayerIndex = playerIndex;
            mButtonAction = buttonAction;
            previousDown = false;
            Active = false;
            Degree = 0.0f;
            mInputActionList.Add(this);
        }

        /// <summary>
        /// This must be called when tracking the input is no longer desired.
        /// </summary>
        public void Destroy ()
        {
            mInputActionList.Remove(this);
        }

        /// <summary>
        /// This will update all InputAction objects with new data.
        /// Call this on each game Update.
        /// </summary>
        public static void Update()
        {
            foreach (InputAction inputAction in mInputActionList)
            {
                inputAction.StepInputAction();
            }
        }
    }
}