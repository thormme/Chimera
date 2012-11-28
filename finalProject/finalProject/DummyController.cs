using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;

namespace finalProject
{
    class DummyController : AIController
    {
        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            base.Update(time, collidingCreatures);

            Durdle();
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
