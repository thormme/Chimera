using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace finalProject
{
    /// <summary>
    /// Abstract class used to define the behavior of a creature.
    /// </summary>
    abstract public class Controller
    {
        private const int MaxDurdleMoveTime = 5000;
        private const int MaxDurdleWaitTime = 5000;

        protected Creature mCreature;

        private int mDurdleMoveTimer;
        private int mDurdleWaitTimer;

        public Controller() {}

        /// <summary>
        /// Sets the creature this controller will control.
        /// </summary>
        /// <param name="creature">
        /// The creature to control.
        /// </param>
        public void SetCreature(Creature creature)
        {
            mCreature = creature;
        }

        /// <summary>
        /// Called every frame. Defines the behavior of the creature based on the nearby creatures.
        /// </summary>
        /// <param name="time">
        /// The game time.
        /// </param>
        /// <param name="collidingCreatures">
        /// The nearby creatures, in order of distance from the creature.
        /// </param>
        abstract public void Update(GameTime time, List<Creature> collidingCreatures);

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
    }
}
