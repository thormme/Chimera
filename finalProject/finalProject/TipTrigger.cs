using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace finalProject
{
    class TipTrigger : Trigger
    {

        string mTip;
        bool mDisplay;

        public TipTrigger(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale, string[] parameters) :
            base(translation, Convert.ToSingle(parameters[0]))
        {
            mTip = Convert.ToString(parameters[1]);
            Console.WriteLine(mTip);
        }

        public override void OnEnter()
        {
            mDisplay = true;
        }

        public override void OnExit()
        {
            mDisplay = false;
        }

        public override void Render()
        {
            if (mDisplay)
            {
                Game1.tips.Enqueue(mTip);
            }
        }

    }
}
