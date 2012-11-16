using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using GameConstructLibrary;

namespace finalProject
{
    /// <summary>
    /// Checks listening sensitivity and vision to find which creatures in the radius the sensor actually notices
    /// </summary>
    public class SensitiveSensor : RadialSensor
    {
        private double mVisionAngle;
        private float mListeningSensitivity;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="visionAngle"></param>
        /// <param name="listeningSensitivity"></param>
        public SensitiveSensor(
            float radius,
            double visionAngle,
            float listeningSensitivity
            ) : base(radius)
        {
            mVisionAngle = Math.Cos(visionAngle);
            mListeningSensitivity = listeningSensitivity;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

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
