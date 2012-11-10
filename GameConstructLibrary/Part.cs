using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    abstract public class Part : MyPhysicsObject
    {
        private Creature mCreature;

        public Part(Creature creature)
        {
            mCreature = creature;
        }

        abstract public void Update(GameTime time);
        abstract public void Use(Vector3 direction);
    }
}
