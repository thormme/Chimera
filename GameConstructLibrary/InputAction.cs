using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    public class InputAction
    {
        private static List<InputAction> mInputActionList = new List<InputAction>();

        public enum ButtonAction { Pressed, Released, Down};
        public enum GamePadThumbStick { Left, Right };
        public enum GamePadThumbStickAxis { X, Y };
        public enum GamePadTrigger { Left, Right };
        public enum MouseButton { Left, Right, Middle };
        private enum InputType { /*/*/GamePadButton, /*/*/GamePadThumbStick, /*/*/GamePadTrigger, /*/*/Key, /**/MouseButton, MousePosition };

        protected PlayerIndex mPlayerIndex;
        protected ButtonAction mButtonAction;

        protected bool previousDown;

        public bool Active
        {
            get;
            protected set;
        }

        public double Degree
        {
            get;
            protected set;
        }

        protected virtual bool IsDown()
        {
            return false;
        }

        protected virtual double GetDegree()
        {
            return IsDown() ? 1.0 : 0;
        }

        void StepInputAction()
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
                Degree = 0.0;
            }
            else
            {
                Degree = GetDegree();
            }
        }

        public InputAction(PlayerIndex playerIndex, ButtonAction buttonAction) {
            mPlayerIndex = playerIndex;
            mButtonAction = buttonAction;
            previousDown = false;
            Active = false;
            Degree = 0.0;
            mInputActionList.Add(this);
        }

        public void Destroy ()
        {
            mInputActionList.Remove(this);
        }

        public static void Update()
        {
            foreach (InputAction inputAction in mInputActionList)
            {
                inputAction.StepInputAction();
            }
        }
    }
}