using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphicsLibrary;
using BEPUphysics.Entities;
using BEPUphysics.Collidables.MobileCollidables;
using Microsoft.Xna.Framework;

namespace finalProject
{
    abstract public class Actor : PhysicsObject
    {
        public Actor(Renderable renderable, EntityCollidable collisionInformation)
            : base(renderable, collisionInformation)
        {}

        abstract public void Update(GameTime time);
    }
}
