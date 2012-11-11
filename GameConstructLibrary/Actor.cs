using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphicsLibrary;
using BEPUphysics.Entities;
using Microsoft.Xna.Framework;
using BEPUphysics.CollisionShapes;

namespace GameConstructLibrary
{
    abstract public class Actor : PhysicsObject
    {
        public Actor(Renderable renderable, EntityShape shape)
            : base(renderable, shape)
        {}

        abstract public void Update(GameTime time);
    }
}
