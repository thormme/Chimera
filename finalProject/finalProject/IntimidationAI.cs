using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;

namespace finalProject
{
    class IntimidationAI : AIController
    {
        private Creature mTarget = null;

        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            base.Update(time, collidingCreatures);

            // if there are no foreign creatures colliding with the radial sensor, durdle.
            bool foreignCreatures = false;
            foreach (Creature i in collidingCreatures)
            {
                if (i != mCreature)
                {
                    foreignCreatures = true;
                    break;
                }
            }

            // If a more intimidating creature is sensed, the creature runs away from it.
            // Otherwise, follow the most intimidating creature <= mCreature's intimidation
            Creature target = null;
            foreach (Creature i in collidingCreatures)
            {
                if (i != mCreature)
                {
                    if (i.Intimidation > mCreature.Intimidation)
                    {
                        Run(i);
                        return;
                    }
                    else if (target == null || i.Intimidation > target.Intimidation)
                    {
                        target = i;
                    }
                }
            }

            if (target != null)
            {
                Follow(target);
                return;
            }

            if (!foreignCreatures)
            {
                Durdle();
                return;
            }
        }
    }
}
