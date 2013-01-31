﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Chimera.Parts;

namespace Chimera.AI
{
    class OverConfidenceAI : IntimidationAI
    {
        private const float RunRatio = 0.25f;

        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            base.Update(time, collidingCreatures);

            //FrilledLizardHead part = mCreature.PartAttachments[ChoosePartSlot()].Part as FrilledLizardHead;

            bool noTarget = mTargetCreature == null || (mCreature.Position - mTargetCreature.Position).Length() > mCreature.Sensor.Radius;
            int intimidation = mCreature.Intimidation;//part.Active ? mCreature.Intimidation - FrilledLizardHead.IntimidationIncrease : mCreature.Intimidation;
            if (mMostIntimidatingCreature != null && mMostIntimidatingCreature.Intimidation > intimidation)
            {
                foreach (Creature creature in collidingCreatures)
                {
                    if (mCreature.CollideDistance(creature) < mCreature.Sensor.Radius * RunRatio && creature.Intimidation > intimidation)
                    {
                        FleeOrder(creature);
                        return;
                    }
                }

                if (noTarget)
                {
                    FollowOrder(mMostIntimidatingCreature);
                }
            }
            else if (noTarget)
            {
                mCreature.FinishUsePart(ChoosePartSlot(), mCreature.Forward);
                DurdleOrder();
            }
        }

        protected override void UsePartUpdate(GameTime time)
        {
            if (mUsingPart)
            {
                mCreature.UsePart(ChoosePartSlot(), mUsePartDirection);
                mUsingPart = false;
            }
        }

        protected override void FinishUsePart()
        { }
    }
}