using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics.Collidables;

namespace GameConstructLibrary
{
    public interface IStaticCollidableOwner
    {
        StaticCollidable StaticCollidable
        {
            get;
        }
    }
}
