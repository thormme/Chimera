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
        public static string PlayerTypeName = "Chimera.PlayerCreature, Chimera";

        public static Vector3 WorldScale = new Vector3(4.0f, 0.01f, 4.0f);

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

        public static Matrix GetViewMatrixFromRay(Ray ray)
        {
            Vector3 rayRight = Vector3.Cross(ray.Direction, Vector3.Up);
            Vector3 rayUp = Vector3.Cross(rayRight, ray.Direction);
            return Matrix.CreateLookAt(ray.Position, ray.Position + ray.Direction, rayUp);
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

        public static void ProjectRayToScreenSpace(Ray ray, Viewport viewport, Matrix viewTransform, Matrix projectionTransform,
            out Vector2 rayPosition,
            out Vector2 rayDirection)
        {
            Vector3 screenPosition1 = viewport.Project(ray.Position, projectionTransform, viewTransform, Matrix.Identity);
            Vector3 screenPosition2 = viewport.Project(ray.Position + ray.Direction, projectionTransform, viewTransform, Matrix.Identity);

            Vector3 direction = screenPosition2 - screenPosition1;
            rayPosition = new Vector2(screenPosition1.X, screenPosition1.Y);
            rayDirection = new Vector2(direction.X, direction.Y);
        }

        public static Vector3 ProjectVectorOntoPlane(Ray ray, Vector3 pointOnPlane, Vector3 planeNormal)
        {
            float r = Vector3.Dot(planeNormal, pointOnPlane - ray.Position) / Vector3.Dot(planeNormal, ray.Direction);
            return ray.Position + ray.Direction * r;
        }

        public static Vector2 NearestPointOnLine(Vector2 linePoint, Vector2 lineDirection, Vector2 point)
        {
            float t = Vector2.Dot(lineDirection, point - linePoint) / lineDirection.LengthSquared();
            return linePoint + t * lineDirection;
        }

        public static Ray CreateWorldRayFromScreenPoint(Vector2 screenPosition, Viewport viewport, Vector3 cameraPosition, Matrix viewTransform, Matrix projectionTransform)
        {
            Vector3 nearScreen = new Vector3(screenPosition.X, screenPosition.Y, 0.0f);
            Vector3 farScreen = new Vector3(screenPosition.X, screenPosition.Y, 1.0f);
            Vector3 nearWorld = viewport.Unproject(nearScreen, projectionTransform, viewTransform, Matrix.Identity);
            Vector3 farWorld = viewport.Unproject(farScreen, projectionTransform, viewTransform, Matrix.Identity);
            Vector3 projectionDirection = (farWorld - nearWorld);
            projectionDirection.Normalize();
            Ray ray = new Ray(cameraPosition, projectionDirection);
            return ray;
        }
    }
}
