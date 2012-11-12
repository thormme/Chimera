using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameConstructLibrary;

namespace finalProject
{
    /// <summary>
    /// Comparator used to sort objects by distance from a position.
    /// </summary>
    class ClosestPhysicsObject : IComparer<PhysicsObject>
    {
        private Vector3 mPos;

        /// <summary>
        /// </summary>
        /// <param name="pos">
        /// The position to determine distance from.
        /// </param>
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
