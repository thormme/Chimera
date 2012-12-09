

//#define DEBUGFACING

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
        public override int Sneak
        {
            get;
            set;
        }

        public override int Intimidation
        {
            get;
            set;
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
            int listeningSensitivity,
            int sneak,
            int intimidation,
            Part part
            )
            : base(position, height, radius, mass, renderable, new ListeningSensor(sensitivityRadius, visionAngle, listeningSensitivity), controller, 1)
        {
            mIncapacitated = false;
            Sneak = sneak;
            Intimidation = intimidation;
            Controller = controller;
            AddPart(part, 0);
            CharacterController.HorizontalMotionConstraint.Speed = 9.0f;
        }

        public override void RemovePart(Part part)
        {
            base.RemovePart(part);
            Die();
        }

        public override void Damage(int damage, Creature source)
        {
            int health = 0;
            if (mShield)
            {
                ++health;
            }

            if (damage > health)
            {
                Die();
            }
            else
            {
                base.Damage(damage, source);
            }
        }

        protected void Die()
        {
            Move(Vector2.Zero);
            Incapacitated = true;
            //Console.WriteLine(this + " died.");
        }

#if DEBUGFACING
        public override void Render()
        {
            base.Render();
            InanimateModel m = new InanimateModel("dude_walk");
            m.Render(Position + Forward * 15.0f, -Forward, new Vector3(0.1f));
        }
#endif
    }
}
