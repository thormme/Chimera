using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace finalProject.AI
{
    class RhinoAI : AggressiveAI
    {
        protected const double StopLength = 1.0f;
        protected double mStopTimer = -1.0f;
        protected override void UsePartUpdate(GameTime time)
        {
            if (mUsingPart)
            {
                if (mStopTimer > 0.0f)
                {
                    mStopTimer -= time.ElapsedGameTime.TotalSeconds;
                    mCreature.Forward = mUsePartDirection;
                    StopMoving();

                    if (mStopTimer <= 0.0f)
                    {
                        mCreature.UsePart(ChoosePartSlot(), mUsePartDirection);
                        FinishUsePart();
                    }
                }
                else
                {
                    mStopTimer = StopLength;
                }
            }
        }

        protected override void FinishUsePart()
        {
            if (mCreature.PartAttachments[0] != null)
            {
                base.FinishUsePart();
                mStopTimer = -1.0f;
            }
        }
    }
}
