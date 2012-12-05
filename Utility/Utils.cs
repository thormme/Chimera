using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics;

namespace Utility
{
    /// <summary>
    /// One utility class to rule them all.
    /// </summary>
    public static class Utils
    {
        public static Matrix GetMatrixFromLookAtVector(Vector3 lookAt)
        {
            // TODO: Stolen from renderable and making assumptions.
            Vector3 worldViewXZ = new Vector3(lookAt.X, 0.0f, lookAt.Z);
            worldViewXZ.Normalize();

            Vector3 defaultViewXZ = new Vector3(0.0f, 0.0f, -1.0f);
            defaultViewXZ.Normalize();

            Vector3 worldViewYZ = new Vector3(0.0f, new Vector3(0.0f, 0.0f, -1.0f).Y, 1.0f);
            worldViewYZ.Normalize();

            Vector3 defaultViewYZ = new Vector3(0.0f, 0.0f, 1.0f);
            defaultViewYZ.Normalize();

            float yawAngle = (float)Math.Acos(Vector3.Dot(worldViewXZ, defaultViewXZ));

            if (Vector3.Cross(worldViewXZ, defaultViewXZ).Y > 0)
            {
                yawAngle *= -1.0f;
            }

            float pitchAngle = (float)Math.Acos(Vector3.Dot(worldViewYZ, defaultViewYZ));

            if (Vector3.Cross(worldViewYZ, defaultViewYZ).X < 0)
            {
                pitchAngle *= -1.0f;
            }

            Matrix worldTransforms = Matrix.CreateRotationX(pitchAngle);
            worldTransforms *= Matrix.CreateRotationY(yawAngle);
            return worldTransforms;
        }

        /// <summary>
        /// Determines whether there is a wall nearby.
        /// </summary>
        /// <param name="position">Position to look from.</param>
        /// <param name="facingDirection">Direction to check in.</param>
        /// <param name="filter">Anonymous function to filter out unwanted objects.</param>
        /// <param name="space">The space to check for a wall in.</param>
        /// <param name="distance">The distance to check within.</param>
        /// <returns>True if a wall was detected, false otherwise.</returns>
        public static bool FindWall(Vector3 position, Vector3 facingDirection, Func<BroadPhaseEntry, bool> filter, Space space, float distance)
        {
            Ray forwardRay = new Ray(position, new Vector3(facingDirection.X, 0, facingDirection.Z));
            RayCastResult result = new RayCastResult();
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
