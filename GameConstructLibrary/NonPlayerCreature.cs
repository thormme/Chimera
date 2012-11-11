using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using BEPUphysics.CollisionShapes;

namespace GameConstructLibrary
{
    abstract class NonPlayerCreature : Creature
    {
        private float mSneak;
        public override float Sneak
        {
            get
            {
                return mSneak;
            }
        }

        NonPlayerCreature(
            float sensitivityRadius,
            Controller controller,
            Renderable renderable,
            EntityShape shape,
            float visionAngle,
            float listeningSensitivity,
            float sneak,
            Part part
            )
            : base(renderable, shape, new SensitiveSensor(sensitivityRadius, visionAngle, listeningSensitivity), controller)
        {
            mSneak = sneak;
            mController = new HostileController(this);
            mParts.Add(part);
        }
    }
}
