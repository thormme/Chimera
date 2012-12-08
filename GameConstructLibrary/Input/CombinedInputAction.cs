using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    public class CombinedInputAction : InputAction
    {
        InputAction[] mInputs;

        /// <summary>
        /// Constructs an InputAction which will track multiple InputActions.
        /// </summary>
        /// <param name="inputs">The inputs to track.</param>
        /// <param name="buttonAction">What state the InputActions should be in to activate.</param>
        public CombinedInputAction(InputAction[] inputs, ButtonAction buttonAction)
            : base(PlayerIndex.One, buttonAction)
        {
            mInputs = inputs;
        }

        protected override bool IsDown()
        {
            foreach (InputAction input in mInputs)
            {
                if (input.Active)
                {
                    return true;
                }
            }
            return false;
        }

        protected override float GetDegree()
        {
            float degree = 0f;

            foreach (InputAction input in mInputs)
            {
                degree = Math.Abs(degree) > Math.Abs(input.Degree) ? degree : input.Degree;
            }

            return degree;
        }
    }
}