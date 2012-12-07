using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace finalProject.AI
{
    class FrogAI : PassiveAI
    {
        protected const double TimerLength = 2.0f;

        protected const double TongueTime = TimerLength / 2.0f;
        protected double mTimer = TimerLength;

        protected override void UsePartUpdate(GameTime time)
        { 
        }

        protected override void DurdleMoveUpdate(GameTime time)
        {
            if (mTimer > 0.0f)
            {
                if (mTimer > TongueTime)
                {
                    mTimer -= time.ElapsedGameTime.TotalSeconds;
                    if (mTimer < TongueTime)
                    {
                        mCreature.UsePart(ChoosePartSlot(), mCreature.Forward);
                        mUsingPart = true;
                    }
                }
                else
                {
                    mTimer -= time.ElapsedGameTime.TotalSeconds;
                    if (mTimer < 0.0f)
                    {
                        mTimer = TimerLength;
                        FinishUsePart();
                    }
                }
            }
        }
    }
}
