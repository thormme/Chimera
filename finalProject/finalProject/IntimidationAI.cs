using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;

namespace finalProject
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
                if (mMostIntimidatingCreature == null ||
                    (creature.Intimidation > mMostIntimidatingCreature.Intimidation &&
                    creature.GetType() != mCreature.GetType()))
                {
                    mMostIntimidatingCreature = creature;
                }
            }
        }
    }
}
