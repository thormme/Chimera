using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Chimera.Parts;
using Chimera.Creatures;
using GameConstructLibrary;

namespace Chimera.AI
{
    class RangedAI : IntimidationAI
    {
        private const float RunRatio = 0.5f;

        protected virtual List<Creature> FilterCreatures(List<Creature> collidingCreatures)
        {
            Type type = mCreature.GetType();
            List<Creature> noLikeType = new List<Creature>();
            foreach (Creature creature in collidingCreatures)
            {
                if (!(creature.GetType() == type))
                {
                    noLikeType.Add(creature);
                }
            }
            return noLikeType;
        }

        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            List<Creature> filteredCreatures = FilterCreatures(collidingCreatures);
            base.Update(time, filteredCreatures);

            if (mMostIntimidatingCreature != null && mMostIntimidatingCreature.Intimidation > mCreature.Intimidation)
            {
                foreach (Creature creature in filteredCreatures)
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
                mCreature.FinishUsePart(ChoosePartSlot(), mCreature.Forward);
                DurdleOrder();
            }
        }

        protected override void UsePartUpdate(GameTime time)
        {
            if (mUsingPart && State != AIState.FleeCreature)
            {
                mCreature.UsePart(ChoosePartSlot(), mUsePartDirection);
            }
            mUsingPart = false;
        }

        protected override void FinishUsePart()
        { }
    }
}
