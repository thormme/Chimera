using System;
using Microsoft.Xna.Framework;

namespace GraphicsLibrary
{
    abstract public class Renderable
    {
        ///////////////////////////// Internal Constants /////////////////////////////

        private Vector3 mDefaultWorldView  = new Vector3(0.0f, 0.0f, -1.0f);
        private float mDefaultWorldScale = 1.0f;

        ///////////////////////////////// Interface //////////////////////////////////

        /// <summary>
        /// Draws the IRenderable object at worldPosition using default scale and orientation.
        /// </summary>
        /// <param name="worldPosition">Poition of the object's center of mass in world coordinates.</param>
        public void Render(Vector3 worldPosition)
        {
            Render(worldPosition, mDefaultWorldView, mDefaultWorldScale);
        }

        /// <summary>
        /// Draws the IRenderable at worldPosition facing worldViewDirection using default scale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldViewDirection">View vector of object in world coordinates.  Used to calculate rotation.</param>
        public void Render(Vector3 worldPosition, Vector3 worldViewDirection)
        {
            Render(worldPosition, worldViewDirection, mDefaultWorldScale);
        }

        /// <summary>
        /// Draws the IRenderable at worldPositions facing worldViewDirection, and scaled to worldScale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldViewDirection">View vector of object in world coordinates.  Used to calculate rotation.</param>
        /// <param name="worldScale">Scale along each axis of the object.</param>
        public void Render(Vector3 worldPosition, Vector3 worldViewDirection, float worldScale)
        {
            Vector3 worldViewXZ = new Vector3(worldViewDirection.X, 0.0f, worldViewDirection.Z);
            worldViewXZ.Normalize();

            Vector3 defaultViewXZ = new Vector3(mDefaultWorldView.X, 0.0f, mDefaultWorldView.Z);
            defaultViewXZ.Normalize();

            Vector3 worldViewYZ = new Vector3(0.0f, worldViewDirection.Y, 1.0f);
            worldViewYZ.Normalize();

            Vector3 defaultViewYZ = new Vector3(0.0f, mDefaultWorldView.Y, 1.0f);
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

            Matrix worldTransforms = Matrix.CreateScale(worldScale);
            worldTransforms *= Matrix.CreateRotationX(pitchAngle);
            worldTransforms *= Matrix.CreateRotationY(yawAngle);
            worldTransforms *= Matrix.CreateTranslation(worldPosition);

            Draw(worldTransforms);
        }

        ///////////////////////////// Internal functions /////////////////////////////

        abstract protected void Draw(Matrix worldTransform);
    }
}
