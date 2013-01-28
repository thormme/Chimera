using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using GameConstructLibrary;
using BEPUphysics.Entities.Prefabs;

namespace Chimera
{
    /// <summary>
    /// Checks listening sensitivity and vision to find which creatures in the radius the sensor actually notices
    /// </summary>
    public class ListeningSensor : VisionSensor
    {
        //protected const float ListeningMultiplier = 2.0f;
        private int mListeningSensitivity;

        /// <summary>
        /// Constructs a new ListeningSensor.
        /// </summary>
        /// <param name="radius">The distance at which other Creatures can be seen.</param>
        /// <param name="visionAngle">The field of view that where Creatures are seen.</param>
        /// <param name="listeningSensitivity">The distance at which other Creatures can be heard.</param>
        public ListeningSensor(
            float radius,
            float visionAngle,
            int listeningSensitivity
            ) : base(radius, visionAngle)
        {
            mListeningSensitivity = listeningSensitivity;
        }

        protected bool CanHear(Creature creature)
        {
            //float radius = (Entity as Sphere).Radius;
            //float distance = (creature.Position - Position).Length();
            //float noise = distance / radius * ListeningMultiplier;
            return mListeningSensitivity >= creature.Sneak;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            List<Creature> newList = new List<Creature>();
            foreach(Creature creature in mCollidingCreatures)
            {
                if (!creature.Incapacitated)
                {
                    newList.Add(creature);
                }
            }

            mCollidingCreatures = newList;

            foreach (IGameObject gameObject in CollidingObjects)
            {
                Creature creature = gameObject as Creature;
                if (creature != null && !creature.Incapacitated && CanHear(creature) && !CanSee(creature))
                {
                    mCollidingCreatures.Add(creature);
                }
            }

            mCollidingCreatures.Sort(new ClosestPhysicsObject(Position));
        }
    }
}
