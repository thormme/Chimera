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
    /// <summary>
    /// A type of physical object which has autonomy.
    /// </summary>
    abstract public class Actor : PhysicsObject
    {
        public Actor(Renderable renderable, Entity entity)
            : base(renderable, entity)
        {}

        abstract public void Update(GameTime time);
    }
}
