using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    class SensitiveSensor : RadialSensor
    {
        private double mVisionAngle;
        private float mListeningSensitivity;

        public SensitiveSensor(
            double visionAngle,
            float listeningSensitivity
            )
        {
            mVisionAngle = Math.Cos(visionAngle);
            mListeningSensitivity = listeningSensitivity;
        }
        
        public override void Collide(List<MyPhysicsObject> objects)
        {
            base.Collide(objects);

            Vector3 normalFacing = Vector3.Normalize(Forward);

            foreach (Creature cur in mCollidingCreatures)
            {
                Vector3 curNormal = Vector3.Normalize(Vector3.Subtract(cur.Position, Position));

                if (mListeningSensitivity < cur.Sneak &&
                    mVisionAngle < Vector3.Dot(normalFacing, curNormal))
                {
                    mCollidingCreatures.Remove(cur);
                }
            }
        }
    }
}
