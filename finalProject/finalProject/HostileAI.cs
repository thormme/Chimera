using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace finalProject
{
    class HostileAI : IdentifyHostileAI
    {
        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            base.Update(time, collidingCreatures);

            if (mTarget != null)
            {
                Follow(mTarget);
            }
        }

        public override ControllerBehavior Behavior
        {
            get
            {
                return ControllerBehavior.Hostile;
            }
        }
    }
}
