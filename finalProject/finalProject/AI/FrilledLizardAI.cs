using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using finalProject.Parts;

namespace finalProject.AI
{
    class FrilledLizardAI : IntimidationAI
    {
        private const float RunRatio = 0.5f;

        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            base.Update(time, collidingCreatures);

            int intimidation = mUsingPart ? mCreature.Intimidation - FrilledLizardHead.IntimidationIncrease : mCreature.Intimidation;
            if (mMostIntimidatingCreature != null && mMostIntimidatingCreature.Intimidation > intimidation)
            {
                FrilledLizardHead part = ChoosePart() as FrilledLizardHead;
                foreach (Creature creature in collidingCreatures)
                {
                    if (mCreature.CollideDistance(creature) < mCreature.Sensor.Radius * RunRatio && creature.Intimidation > intimidation)
                    {
                        FleeOrder(creature);
                        return;
                    }
                }

                StopOrder();
                UsePart(mCreature.Forward);
            }
            else
            {
                ChoosePart().FinishUse(mCreature.Forward);
                DurdleOrder();
            }
        }

        protected override void UsePartUpdate(GameTime time)
        {
            if (mUsingPart)
            {
                ChoosePart().Use(mUsePartDirection);
                mUsingPart = false;
            }
        }

        protected override void FinishUsePart()
        { }
    }
}
