using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameConstructLibrary
{
    class ClosestPhysicsObject : IComparer<PhysicsObject>
    {
        private Vector3 mPos;

        public ClosestPhysicsObject(Vector3 pos)
        {
            mPos = pos;
        }

        public int Compare(PhysicsObject x, PhysicsObject y)
        {
            float xDist = Vector3.Subtract(x.Position, mPos).Length();
            float yDist = Vector3.Subtract(y.Position, mPos).Length();

            float result = xDist - yDist;

            return (int)result;
        }
    }
}
