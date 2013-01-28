using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;

namespace Chimera
{
    class DummyController : AIController
    {
        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            base.Update(time, collidingCreatures);

            DurdleOrder();
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
