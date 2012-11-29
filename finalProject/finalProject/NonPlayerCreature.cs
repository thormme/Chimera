using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using BEPUphysics.CollisionShapes;
using BEPUphysics.Entities;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.Collidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using GameConstructLibrary;

namespace finalProject
{
    /// <summary>
    /// Abstract class used for making NPCs.
    /// </summary>
    public abstract class NonPlayerCreature : Creature
    {
        protected float mSneak;
        public override float Sneak
        {
            get
            {
                return mSneak;
            }
        }

        protected bool mIncapacitated;
        public override bool Incapacitated
        {
            get
            {
                return mIncapacitated;
            }
        }

        public NonPlayerCreature(
            Vector3 position,
            float height,
            float radius,
            float mass,
            float sensitivityRadius,
            Controller controller,
            Renderable renderable,
            float visionAngle,
            float listeningSensitivity,
            float sneak,
            Part part
            )
            : base(position, height, radius, mass, renderable, new SensitiveSensor(sensitivityRadius, visionAngle, listeningSensitivity), controller)
        {
            mIncapacitated = false;
            mSneak = sneak;
            mController = controller;
            AddPart(part);
        }

        public override void Damage(int damage)
        {}
    }
}
