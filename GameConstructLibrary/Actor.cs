using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    abstract public class MyActor : MyPhysicsObject
    {
        abstract public void Update(GameTime time);
    }
}
