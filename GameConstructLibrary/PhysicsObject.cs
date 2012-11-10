using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    public class MyPhysicsObject
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 Acceleration { get; set; }
        public Vector3 Forward { get; set; }
        public Vector3 Left { get; set; }
        public Vector3 Pitch { get; set; }
        public Vector3 Yaw { get; set; }
    }
}
