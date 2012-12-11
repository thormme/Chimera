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

        string mTip = String.Empty;
        bool mDisplay = false;

        public TipTrigger(String modelName, Vector3 translation, Quaternion orientation, Vector3 scale, string[] parameters) :
            base(translation, Convert.ToSingle(parameters[0]))
        {
            mTip = Convert.ToString(parameters[1]);
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
                Game1.AddTip(new GameTip(new string[] {mTip}, 5.0f));
            }
        }

    }
}
