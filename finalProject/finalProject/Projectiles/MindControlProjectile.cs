using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;
using finalProject.Creatures;
using finalProject.AI;

namespace finalProject.Projectiles
{
    public class MindControlProjectile : Projectile
    {
        public bool Active
        {
            get;
            protected set;
        }

        public const double ControlLength = 5.0f;

        private Creature mOriginalCreature = null;
        private Controller mOriginalController = null;

        private double mControlTimer = -1.0f;

        public MindControlProjectile(Actor owner, Vector3 direction)
            : base(
            new InanimateModel("box"),
            new Box(owner.Position, 0.05f, 0.05f, 0.05f, 0.25f),
            owner,
            direction,
            40.0f,
            new Vector3(0.2f)
            )
        {
            Active = false;
        }

        protected override void Hit(IGameObject gameObject)
        {
            if (Active)
            {
                return;
            }

            Creature creature = gameObject as Creature;
            if (creature != null && !creature.Incapacitated)
            {
                if (creature is Cobra ||
                    !(creature is Cobra) && creature.Controller is CobraAI ||
                    !(creature is PlayerCreature) && creature.Controller is PlayerController)
                {
                    return;
                }

                Creature owner = mOwner as Creature;
                creature.Damage(0, owner);

                Controller ownerController = owner.Controller;

                mOriginalController = creature.Controller;
                mOriginalCreature = creature;

                mOriginalController.NoControl = true;
                owner.Move(Vector2.Zero);
                creature.Controller = ownerController;
                ownerController.SetCreature(creature);
                mControlTimer = ControlLength;
                owner.Controller = new AIController();
                owner.Controller.SetCreature(owner);
                Active = true;

                StickToEntity(creature.Entity);
                //Position = new Vector3(float.MaxValue);
            }
            else
            {
                World.Remove(this);
            }
        }

        public void Stop()
        {
            if (Active)
            {
                Creature owner = mOwner as Creature;
                owner.Controller = mOriginalCreature.Controller;
                mOriginalController.NoControl = false;
                mOriginalCreature.Controller = mOriginalController;
                owner.Controller.SetCreature(owner);
                World.Remove(this);
                Active = false;
                mControlTimer = -1.0f;
            }
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (mControlTimer >= 0.0f)
            {
                mOriginalController.Update(time, new List<Creature>());
                mControlTimer -= time.ElapsedGameTime.TotalSeconds;
                if (mControlTimer < 0.0f)
                {
                    Stop();
                }
            }
        }
    }
}
