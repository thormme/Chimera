using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics.Entities;

namespace finalProject
{
    class PhysicsObject : GameObject
    {
        Entity mPhysicsEntity;

        public override Vector3 Position
        {
            virtual get
            {
                return mPhysicsEntity.Position;
            }
            virtual set 
            {
                mPhysicsEntity.Position = value;
                base.Position = value;
            }
        }
    }
}
