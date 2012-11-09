using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphicsLibrary;
using BEPUphysics.Entities;

namespace finalProject
{
    public class Actor : PhysicsObject
    {
        public Actor(Renderable renderable, Entity physicsEntity) : base(renderable, physicsEntity)
        {

        }

        public virtual void Update()
        {

        }
    }
}
