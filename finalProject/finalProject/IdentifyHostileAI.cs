using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using Microsoft.Xna.Framework;

namespace finalProject
{
    class IdentifyHostileAI : AIController
    {
        protected Creature mTarget = null;

        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            base.Update(time, collidingCreatures);

            mTarget = null;

            // if there are no foreign creatures colliding with the radial sensor, durdle.
            bool foreignCreatures = false;
            foreach (Creature i in collidingCreatures)
            {
                if (i != mCreature)
                {
                    foreignCreatures = true;
                    System.Console.WriteLine("found player");
                    break;
                }
            }

            if (!foreignCreatures)
            {
                System.Console.WriteLine("Durdling.");
                Durdle();
                return;
            }

            foreach (Creature cur in collidingCreatures)
            {
                if (cur != mCreature)
                {
                    mTarget = cur;
                    break;
                }
            }
            foreach (Creature cur in collidingCreatures)
            {
                if (cur.CreatureController.Behavior == ControllerBehavior.Hostile && cur != mCreature)
                {
                    mTarget = cur;
                    break;
                }
            }
        }
    }
}
