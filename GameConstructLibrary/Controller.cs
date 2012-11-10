using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    abstract public class Controller
    {
        static const int MaxDurdleMoveTime = 5000;
        static const int MaxDurdleWaitTime = 5000;

        protected Creature mCreature;

        private int mDurdleMoveTimer;
        private int mDurdleWaitTimer;

        public Controller(Creature creature)
        {
            mCreature = creature;
        }

        abstract public void Update(GameTime time, List<Creature> collidingCreatures);

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
            throw new NotImplementedException("Controller.Durdle not yet implemented.");
        }
    }
}
