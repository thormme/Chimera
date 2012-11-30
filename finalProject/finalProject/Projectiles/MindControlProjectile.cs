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
        //private Creature mJackedCreature = null;
        private Controller mJackedController = null;

        public MindControlProjectile(Actor owner, Vector3 direction)
            : base(
            new InanimateModel("box"),
            new Box(owner.Position, 1.0f, 1.0f, 1.0f, 1.0f),
            owner,
            direction,
            40.0f,
            new Vector3(1.0f)
            )
        { }

        protected override void Hit(IGameObject gameObject)
        {
            base.Hit(gameObject);

            Creature creature = gameObject as Creature;
            if (creature != null)
            {
                Controller ownerController = (mOwner as Creature).Controller;
                mJackedController = creature.Controller;
                creature.Controller = ownerController;
                ownerController.SetCreature(creature);
            }
        }
    }
}
