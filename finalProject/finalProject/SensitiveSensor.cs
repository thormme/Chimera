using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using GameConstructLibrary;
using BEPUphysics.Entities.Prefabs;

namespace finalProject
{
    /// <summary>
    /// Checks listening sensitivity and vision to find which creatures in the radius the sensor actually notices
    /// </summary>
    public class SensitiveSensor : RadialSensor
    {
        //protected const float ListeningMultiplier = 2.0f;
        private int mListeningSensitivity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="visionAngle"></param>
        /// <param name="listeningSensitivity"></param>
        public SensitiveSensor(
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

            foreach (IGameObject gameObject in CollidingObjects)
            {
                Creature creature = gameObject as Creature;
                if (creature != null && (CanHear(creature) || !CanSee(creature)))
                {
                    mCollidingCreatures.Add(creature);
                }
            }

            mCollidingCreatures.Sort(new ClosestPhysicsObject(Position));
        }
    }
}
