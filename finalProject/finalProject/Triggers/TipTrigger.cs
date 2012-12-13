using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;

namespace finalProject
{
    class TipTrigger : Trigger
    {

        GameTip mTip = null;
        bool mDisplay = false;

        public TipTrigger(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale, string[] parameters) :
            base(translation, orientation, scale)
        {
            mTip = new GameTip(Convert.ToString(parameters[0]).Split('?'), 10.0f);
        }

        public override void OnEnter()
        {
            mDisplay = true;
        }

        public override void OnExit()
        {
            
        }

        public override void Render()
        {
            if (mDisplay)
            {
                mDisplay = false;
                if (!mTip.Displayed)
                {
                    mTip.Displayed = true;
                    Game1.AddTip(mTip);
                }
            }
        }

    }
}
