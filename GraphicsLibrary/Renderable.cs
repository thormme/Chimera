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
            // Gather axis and angle for rotation.
            Vector3 rotationAxis;
            if (worldPosition.Length() != 0.0f)
            {
                rotationAxis = Vector3.Cross(worldPosition, mDefaultWorldView);
            }
            else
            {
                rotationAxis = Vector3.Up;
            }
            rotationAxis.Normalize();

            float rotationAngle = (float)Math.Acos(Vector3.Dot(worldPosition, mDefaultWorldView));

            Matrix worldTransforms = Matrix.CreateScale(worldScale);
            worldTransforms *= Matrix.CreateFromAxisAngle(rotationAxis, rotationAngle);
            worldTransforms *= Matrix.CreateTranslation(-1.0f * worldPosition);

            Draw(worldTransforms);
        }

        ///////////////////////////// Internal functions /////////////////////////////

        abstract protected void Draw(Matrix worldTransform);
    }
}
