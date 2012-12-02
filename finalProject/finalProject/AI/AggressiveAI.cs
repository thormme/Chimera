using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace finalProject.AI
{
    public class AggressiveAI : IntimidationAI
    {
        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            base.Update(time, collidingCreatures);

            if (mMostIntimidatingCreature != null && mMostIntimidatingCreature.Intimidation > mCreature.Intimidation)
            {
                FleeOrder(mMostIntimidatingCreature);
            }
            else if (mMostIntimidatingCreature != null && mMostIntimidatingCreature.Intimidation < mCreature.Intimidation)
            {
                FollowOrder(mMostIntimidatingCreature);
            }
            else
            {
                if (mTargetCreature == null || !mCreature.Sensor.CollidingObjects.Contains(mTargetCreature))
                {
                    DurdleOrder();
                }
            }
        }

        public override void Damage(int damage, Creature source)
        {
            if (source != null)
            {
                FollowOrder(source);
            }
        }
    }
}
