using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Chimera.AI
{
    class EagleAI : PassiveAI
    {
        protected const double FlapInterval = 1.0f;
        protected double mFlapTimer = -1.0f;

        protected override void DurdleMoveUpdate(GameTime time)
        {
            base.DurdleMoveUpdate(time);
            UsePart(mCreature.Forward);
        }

        protected override void UsePartUpdate(GameTime time)
        {
            if (mUsingPart)
            {
                mFlapTimer -= time.ElapsedGameTime.TotalSeconds;
                if (mFlapTimer < 0.0f)
                {
                    int part = ChoosePartSlot();
                    mCreature.UsePart(part, mUsePartDirection);
                    mFlapTimer = FlapInterval;
                }
            }
        }
    }
}
