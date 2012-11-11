using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    class HostileController : Controller
    {
        public HostileController(Creature creature) {}

        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
            if (collidingCreatures.Count() == 0)
            {
                Durdle(time);
                return;
            }

            Creature target = collidingCreatures[0];
            foreach (Creature cur in collidingCreatures)
            {
                if (cur.CreatureController as HostileController != null)
                {
                    target = cur;
                    break;
                }
            }

            Vector3 targetVector = Vector3.Subtract(target.Position, mCreature.Position);
            mCreature.Forward = new Vector3(targetVector.X, 0, targetVector.Y);
            mCreature.Move(new Vector2(0.0f, 1.0f));
            mCreature.UsePart(0, targetVector);
        }
    }
}
