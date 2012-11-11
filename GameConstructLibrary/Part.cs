using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.CollisionShapes;

namespace GameConstructLibrary
{
    abstract public class Part : PhysicsObject
    {
        private Creature mCreature;

        public Part(Creature creature, Renderable renderable, EntityShape shape)
            : base(renderable, shape)
        {
            mCreature = creature;
        }

        abstract public void Update(GameTime time);
        abstract public void Use(Vector3 direction);
    }
}
