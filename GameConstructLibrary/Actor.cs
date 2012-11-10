using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameConstructLibrary
{
    abstract public class MyActor : MyPhysicsObject
    {
        abstract public void Update(float time_step);
    }
}
