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

            if (collidingCreatures.Count() <= 1)
            {
                //Durdle();
                return;
            }

            mTarget = null;
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
