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

        public static Vector3 WorldScale = new Vector3(8.0f, 0.01f, 8.0f);
        public static int AlphaMapScale = 10;

        public static Matrix GetMatrixFromLookAtVector(Vector3 lookAt)
        {
            Vector3 up = new Vector3(0, 1, 0);
            Vector3 origin = new Vector3(0);
            return Matrix.CreateWorld(origin, lookAt, up);
        }

        public static BoundingBox GetCameraBoundingBox(Matrix viewTransform, Matrix projectionTransform)
        {
            BoundingFrustum cameraFrustum = new BoundingFrustum(Matrix.Identity);
            cameraFrustum.Matrix = viewTransform * projectionTransform;

            // Get the corners of the frustum
            Vector3[] frustumCorners = cameraFrustum.GetCorners();

            BoundingBox bounds = BoundingBox.CreateFromPoints(frustumCorners);
            return bounds;
        }

        public static Vector2 WorldToScreenCoordinates(Vector3 worldCoordinate, float viewportWidth, float viewportHeight, Matrix viewTransform, Matrix projectionTransform)
        {
            Vector4 position = new Vector4(worldCoordinate.X, worldCoordinate.Y, worldCoordinate.Z, 1);
            Vector4 cameraCoordinate = Vector4.Transform(position, viewTransform * projectionTransform);
            cameraCoordinate /= cameraCoordinate.W;
            Vector2 screenCoordinate = new Vector2((cameraCoordinate.X + 1.0f) * viewportWidth / 2.0f, (cameraCoordinate.Y - 1.0f) * viewportHeight / -2.0f);
            return screenCoordinate;
        }

        public static Color GetTexture2DPixelColor(int x, int y, Texture2D texture)
        {
            Rectangle sourceRectangle = new Rectangle(x, y, 1, 1);

            Color[] retrievedColor = new Color[1];

            texture.GetData<Color>(
                0,
                sourceRectangle,
                retrievedColor,
                0,
                1);

            return retrievedColor[0];
        }
    }
}
