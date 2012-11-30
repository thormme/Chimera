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
        private double mVisionAngle;
        private int mListeningSensitivity;

        public List<Creature> IgnoredCreatures
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="visionAngle"></param>
        /// <param name="listeningSensitivity"></param>
        public SensitiveSensor(
            float radius,
            double visionAngle,
            int listeningSensitivity
            ) : base(radius)
        {
            mVisionAngle = Math.Cos(visionAngle);
            mListeningSensitivity = listeningSensitivity;
            IgnoredCreatures = new List<Creature>();
        }

        protected bool CanHear(Creature creature)
        {
            //float radius = (Entity as Sphere).Radius;
            //float distance = (creature.Position - Position).Length();
            //float noise = distance / radius * ListeningMultiplier;
            return mListeningSensitivity >= creature.Sneak;
        }

        protected bool CanSee(Creature creature)
        {
            Vector3 curNormal = Vector3.Normalize(Vector3.Subtract(creature.Position, Position));
            Vector3 normalFacing = Vector3.Normalize(Forward);

            return Vector3.Dot(curNormal, normalFacing) > mVisionAngle;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            IgnoredCreatures.Clear();

            List<Creature> newList = new List<Creature>();
            foreach (Creature creature in mCollidingCreatures)
            {
                if (CanHear(creature) || CanSee(creature))
                {
                    newList.Add(creature);
                }
                else
                {
                    IgnoredCreatures.Add(creature);
                }
            }
            mCollidingCreatures = newList;
        }
    }
}
