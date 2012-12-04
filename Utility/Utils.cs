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
    }
}
