using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;

namespace Chimera
{
    public class IntimidationAI : AIController
    {
        protected Creature mMostIntimidatingCreature = null;

        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            base.Update(time, collidingCreatures);

            // Find the most intimidating creature in the sensor.
            mMostIntimidatingCreature = null;
            foreach (Creature creature in collidingCreatures)
            {
                if (creature.Incapacitated == false &&
                    creature.GetType() != mCreature.GetType() &&
                    (mMostIntimidatingCreature == null || creature.Intimidation > mMostIntimidatingCreature.Intimidation))
                {
                    mMostIntimidatingCreature = creature;
                }
            }
        }
    }
}
