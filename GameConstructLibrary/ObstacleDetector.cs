using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;

namespace GameConstructLibrary
{
    public static class ObstacleDetector
    {

        /// <summary>
        /// Determines whether there is a wall nearby.
        /// </summary>
        /// <param name="position">Position to look from.</param>
        /// <param name="facingDirection">Direction to check in.</param>
        /// <param name="filter">Anonymous function to filter out unwanted objects.</param>
        /// <param name="space">The space to check for a wall in.</param>
        /// <param name="distance">The distance to check within.</param>
        /// <returns>True if a wall was detected, false otherwise.</returns>
        public static bool FindWall(Vector3 position, Vector3 facingDirection, Func<BroadPhaseEntry, bool> filter, Space space, float distance, out RayCastResult result)
        {
            Ray forwardRay = new Ray(position, new Vector3(facingDirection.X, 0, facingDirection.Z));
            result = new RayCastResult();
            space.RayCast(forwardRay, filter, out result);

            Vector3 flatNormal = new Vector3(result.HitData.Normal.X, 0, result.HitData.Normal.Z);
            float normalDot = Vector3.Dot(result.HitData.Normal, flatNormal);
            float minDot = (float)Math.Cos(MathHelper.PiOver4) * flatNormal.Length() * result.HitData.Normal.Length();
            if ((result.HitData.Location - forwardRay.Position).Length() < distance && normalDot > minDot)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether there is a cliff nearby.
        /// </summary>
        /// <param name="position">Position to look from.</param>
        /// <param name="facingDirection">Direction to check in.</param>
        /// <param name="filter">Anonymous function to filter out unwanted objects.</param>
        /// <param name="space">The space to check for a cliff in.</param>
        /// <param name="distance">The distance to check at.</param>
        /// <returns>True if a cliff was detected, false otherwise.</returns>
        public static bool FindCliff(Vector3 position, Vector3 facingDirection, Func<BroadPhaseEntry, bool> filter, Space space, float distance)
        {
            // If there is a wall before the requested distance assume there is no cliff.
            Ray forwardRay = new Ray(position, new Vector3(facingDirection.X, 0, facingDirection.Z));
            RayCastResult forwardResult = new RayCastResult();
            space.RayCast(forwardRay, filter, out forwardResult);
            if ((forwardResult.HitData.Location - position).Length() < distance)
            {
                return false;
            }

            facingDirection.Normalize();
            Ray futureDownRay = new Ray(position + new Vector3(facingDirection.X * distance, 0, facingDirection.Z * distance), Vector3.Down);
            RayCastResult result = new RayCastResult();
            space.RayCast(futureDownRay, filter, out result);

            Vector3 drop = result.HitData.Location - futureDownRay.Position;
            if (drop.Y < -6.0f)
            {
                return true;
            }
            else
            {
                return false;
            }
                
        }
    }
}
