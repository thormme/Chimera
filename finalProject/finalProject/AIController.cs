using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace finalProject
{
    public class AIController : Controller
    {
        private const int MaxDurdleMoveTime = 5000;
        private const int MaxDurdleWaitTime = 5000;

        private int mDurdleMoveTimer;
        private int mDurdleWaitTimer;

        /// <summary>
        /// Tells the creature to durdle around.
        /// </summary>
        /// <param name="time">
        /// The game time.
        /// </param>
        public virtual void Durdle(GameTime time)
        {
            if (mDurdleWaitTimer == 0)
            {
                mDurdleMoveTimer = Rand.rand.Next(MaxDurdleMoveTime);
                mDurdleWaitTimer = Rand.rand.Next(MaxDurdleWaitTime);

                // TODO: This should not kill the z component of Forward.
                Vector3 newDirection = new Vector3(0.0f, mCreature.Forward.Z, 0.0f);
                newDirection.X = Rand.rand.Next();
                newDirection.Z = Rand.rand.Next();
                newDirection = Vector3.Normalize(newDirection);
            }
            throw new NotImplementedException("Controller.Durdle not fully implemented.");
        }

        public override void Update(GameTime time, List<Creature> collidingCreatures)
        {
        }
    }
}
