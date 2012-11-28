using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;

namespace finalProject
{
    class PassiveAI : IdentifyHostileAI
    {
        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            base.Update(time, collidingCreatures);

            if (mTarget != null)
            {
                Run(mTarget);
            }
        }

        public override ControllerBehavior Behavior
        {
            get
            {
                return ControllerBehavior.Passive;
            }
        }
    }
}
