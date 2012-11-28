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
        private float mSneak;
        public override float Sneak
        {
            get
            {
                return mSneak;
            }
        }

        private bool mIncapacitated;
        public override bool Incapacitated
        {
            get
            {
                return mIncapacitated;
            }
        }

        public NonPlayerCreature(
            float sensitivityRadius,
            Controller controller,
            Renderable renderable,
            Entity entity,
            float visionAngle,
            float listeningSensitivity,
            float sneak,
            Part part
            )
            : base(renderable, entity, new SensitiveSensor(sensitivityRadius, visionAngle, listeningSensitivity), controller)
        {
            mSneak = sneak;
            mController = controller;
            World.Add(part);
            AddPart(part);
        }

        public override void Damage(int damage)
        {
            mIncapacitated = true;
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            foreach (IGameObject i in mCollidingObjects)
            {
                if (i is Creature)
                {
                    Damage(1);
                }
            }
        }
    }
}
