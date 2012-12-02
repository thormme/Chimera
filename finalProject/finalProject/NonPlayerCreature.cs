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
        protected int Health
        {
            get;
            set;
        }

        protected bool mIncapacitated;
        public override bool Incapacitated
        {
            get
            {
                return mIncapacitated;
            }
        }

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
            int startingHealth,
            Part part
            )
            : base(position, height, radius, mass, renderable, new SensitiveSensor(sensitivityRadius, visionAngle, listeningSensitivity), controller, 1)
        {
            mIncapacitated = false;
            Sneak = sneak;
            Intimidation = intimidation;
            Controller = controller;
            mIncapacitated = false;
            Health = startingHealth;
            AddPart(part, 0);
        }

        public override void Damage(int damage, Creature source)
        {
            if (Invulnerable)
            {
                return;
            }

            base.Damage(damage, source);

            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
                mIncapacitated = true;
                System.Console.WriteLine(this + " died.");
            }
        }

#if DEBUGFACING
        public override void Render()
        {
            base.Render();
            InanimateModel m = new InanimateModel("dude_walk");
            m.Render(Position + Forward * 15.0f, -Forward, new Vector3(0.1f));
        }
#endif

        public override void RemovePart(Part part)
        {
            base.RemovePart(part);

            Move(Vector2.Zero);
            mIncapacitated = true;
            System.Console.WriteLine(this + " died.");
        }
    }
}
