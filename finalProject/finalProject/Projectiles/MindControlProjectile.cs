using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities.Prefabs;

namespace finalProject.Projectiles
{
    public class MindControlProjectile : Projectile
    {
        private const double ControlLength = 10.0f;

        private Creature mOriginalCreature = null;
        private Controller mOriginalController = null;

        private double mControlTimer = -1.0f;

        public MindControlProjectile(Actor owner, Vector3 direction)
            : base(
            new InanimateModel("box"),
            new Box(owner.Position, 0.25f, 0.25f, 0.25f, 0.25f),
            owner,
            direction,
            40.0f,
            new Vector3(1.0f)
            )
        { }

        protected override void Hit(IGameObject gameObject)
        {
            //base.Hit(gameObject);

            Creature creature = gameObject as Creature;
            if (creature != null)
            {
                Creature owner = mOwner as Creature;
                Controller ownerController = owner.Controller;

                mOriginalController = creature.Controller;
                mOriginalCreature = creature;

                owner.Move(Vector2.Zero);
                creature.Controller = ownerController;
                ownerController.SetCreature(creature);
                mControlTimer = ControlLength;

                Position = new Vector3(float.MaxValue);
            }
            else
            {
                base.Hit(gameObject);
            }
        }

        public void Stop()
        {
            Creature owner = mOwner as Creature;
            mOriginalCreature.Controller = mOriginalController;
            owner.Controller.SetCreature(owner);
            World.Remove(this);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            if (mControlTimer >= 0.0f)
            {
                mControlTimer -= time.ElapsedGameTime.TotalSeconds;
                if (mControlTimer < 0.0f)
                {
                    Stop();
                }
            }
        }
    }
}
