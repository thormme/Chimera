using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using BEPUphysics.CollisionShapes;

namespace finalProject
{
    /// <summary>
    /// Abstract class used for making NPCs.
    /// </summary>
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
            Vector3 position,
            float sensitivityRadius,
            Controller controller,
            Renderable renderable,
            EntityShape shape,
            float visionAngle,
            float listeningSensitivity,
            float sneak,
            Part part
            )
            : base(position, renderable, shape, new SensitiveSensor(sensitivityRadius, visionAngle, listeningSensitivity), controller)
        {
            mSneak = sneak;
            mController = new HostileController(this);
            mParts.Add(part);
        }
    }
}
