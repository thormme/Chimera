using System;
using Microsoft.Xna.Framework;

namespace GraphicsLibrary
{
    abstract public class Renderable
    {
        ///////////////////////////// Internal Constants /////////////////////////////

        private Vector3 mDefaultWorldView  = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 mDefaultWorldScale = new Vector3(1.0f);

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
        public void Render(Vector3 worldPosition, Vector3 worldViewDirection, Vector3 worldScale)
        {
            Vector3 up = new Vector3(0, 1, 0);
            Draw(Matrix.CreateScale(worldScale) * Matrix.CreateWorld(worldPosition, worldViewDirection, up));
        }

        /// <summary>
        /// Draws the IRenderable at worldPositions facing worldRotation using default scale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldRotation">Rotation matrix of the object.</param>
        public void Render(Vector3 worldPosition, Matrix worldRotation)
        {
            Render(worldPosition, worldPosition, mDefaultWorldScale);
        }

        /// <summary>
        /// Draws the IRenderable at worldPositions facing worldRotation, and scaled to worldScale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldRotation">Rotation matrix of the object.</param>
        /// <param name="worldScale">Scale along each axis of the object.</param>
        public void Render(Vector3 worldPosition, Matrix worldRotation, Vector3 worldScale)
        {
            Matrix worldTransforms = Matrix.CreateScale(worldScale);
            worldTransforms *= worldRotation;
            worldTransforms *= Matrix.CreateTranslation(worldPosition);

            Draw(worldTransforms);
        }

        /// <summary>
        /// Draws the IRenderable using the given world transform.
        /// </summary>
        /// <param name="worldTransform">Position of the object's center of mass in world coordinates.</param>
        public void Render(Matrix worldTransform)
        {
            Draw(worldTransform);
        }

        ///////////////////////////// Internal functions /////////////////////////////

        abstract protected void Draw(Matrix worldTransform);
    }
}
