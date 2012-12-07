﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using finalProject.Parts;
using finalProject.Creatures;
using GameConstructLibrary;

namespace finalProject.AI
{
    class CobraAI : IntimidationAI
    {
        private const float RunRatio = 0.5f;

        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            List<Creature> noCobras = new List<Creature>();
            foreach (Creature creature in collidingCreatures)
            {
                if (!(creature is Cobra))
                {
                    noCobras.Add(creature);
                }
            }

            base.Update(time, noCobras);

            if (mMostIntimidatingCreature != null && mMostIntimidatingCreature.Intimidation > mCreature.Intimidation)
            {
                foreach (Creature creature in collidingCreatures)
                {
                    if (creature.Intimidation > mCreature.Intimidation && mCreature.CollideDistance(creature) < mCreature.Sensor.Radius * RunRatio)
                    {
                        FleeOrder(creature);
                        return;
                    }
                }

                FollowOrder(mMostIntimidatingCreature);
            }
            else
            {
                ChoosePart().FinishUse(mCreature.Forward);
                DurdleOrder();
            }
        }

        protected override void UsePartUpdate(GameTime time)
        {
            if (mUsingPart && State != AIState.FleeCreature)
            {
                ChoosePart().Use(mUsePartDirection);
            }
            mUsingPart = false;
        }

        protected override void FinishUsePart()
        { }

        protected override Part ChoosePart()
        {
            List<Part> parts = new List<Part>();
            foreach (Creature.PartAttachment pa in mCreature.PartAttachments)
            {
                if (pa != null)
                {
                    parts.Add(pa.Part);
                }
            }
            return parts[Rand.rand.Next(parts.Count)];
        }
    }
}
