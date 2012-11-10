using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    abstract class NonPlayerCreature : Creature
    {
        private float mSneak;
        public override float Sneak
        {
            get { return mSneak; }
        }

        NonPlayerCreature(
            float visionAngle,
            float listeningSensitivity,
            float sneak,
            Part part
            )
        {
            mSensor = new SensitiveSensor(visionAngle, listeningSensitivity);
            mSneak = sneak;
            mController = new HostileController(this);
            mParts.Add(part);
        }
    }
}
