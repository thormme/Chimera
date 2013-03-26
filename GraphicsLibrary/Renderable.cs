using System;
using Microsoft.Xna.Framework;

namespace GraphicsLibrary
{
    abstract public class Renderable
    {
        ///////////////////////////// Internal Constants /////////////////////////////

        private Vector3 mDefaultWorldView  = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 mDefaultWorldScale = new Vector3(1.0f);
        private Color mDefaultOverlayColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        private float mDefaultOverlayWeight = 0.0f;

        ///////////////////////////////// Interface //////////////////////////////////

        public BoundingBox BoundingBox
        {
            get { return mBoundingBox; }
            set { mBoundingBox = value; }
        }
        protected BoundingBox mBoundingBox;

        /// <summary>
        /// Draws the IRenderable object at worldPosition using default scale and orientation.
        /// </summary>
        /// <param name="worldPosition">Poition of the object's center of mass in world coordinates.</param>
        public void Render(Vector3 worldPosition)
        {
            Render(worldPosition, mDefaultWorldView, mDefaultWorldScale);
        }

        /// <summary>
        /// Draws the IRenderable object at worldPosition using default scale and orientation.
        /// </summary>
        /// <param name="worldPosition">Poition of the object's center of mass in world coordinates.</param>
        public void Render(Vector3 worldPosition, Color overlayColor, float overlayColorWeight)
        {
            Render(worldPosition, mDefaultWorldView, mDefaultWorldScale, overlayColor, overlayColorWeight);
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
        /// Draws the IRenderable at worldPosition facing worldViewDirection using default scale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldViewDirection">View vector of object in world coordinates.  Used to calculate rotation.</param>
        public void Render(Vector3 worldPosition, Vector3 worldViewDirection, Color overlayColor, float overlayColorWeight)
        {
            Render(worldPosition, worldViewDirection, mDefaultWorldScale, overlayColor, overlayColorWeight);
        }

        /// <summary>
        /// Draws the IRenderable at worldPositions facing worldViewDirection, and scaled to worldScale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldViewDirection">View vector of object in world coordinates.  Used to calculate rotation.</param>
        /// <param name="worldScale">Scale along each axis of the object.</param>
        public void Render(Vector3 worldPosition, Vector3 worldViewDirection, Vector3 worldScale)
        {
            Draw(Matrix.CreateScale(worldScale) * Matrix.CreateWorld(worldPosition, worldViewDirection, Vector3.Up), mDefaultOverlayColor, mDefaultOverlayWeight);
        }

        /// <summary>
        /// Draws the IRenderable at worldPositions facing worldViewDirection, and scaled to worldScale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldViewDirection">View vector of object in world coordinates.  Used to calculate rotation.</param>
        /// <param name="worldScale">Scale along each axis of the object.</param>
        public void Render(Vector3 worldPosition, Vector3 worldViewDirection, Vector3 worldScale, Color overlayColor, float overlayColorWeight)
        {
            Draw(Matrix.CreateScale(worldScale) * Matrix.CreateWorld(worldPosition, worldViewDirection, Vector3.Up), overlayColor, overlayColorWeight);
        }

        /// <summary>
        /// Draws the IRenderable at worldPositions facing worldRotation using default scale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldRotation">Rotation matrix of the object.</param>
        public void Render(Vector3 worldPosition, Matrix worldRotation)
        {
            Render(worldPosition, worldRotation, mDefaultWorldScale);
        }

        /// <summary>
        /// Draws the IRenderable at worldPositions facing worldRotation using default scale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldRotation">Rotation matrix of the object.</param>
        public void Render(Vector3 worldPosition, Matrix worldRotation, Color overlayColor, float overlayColorWeight)
        {
            Render(worldPosition, worldRotation, mDefaultWorldScale, overlayColor, overlayColorWeight);
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

            Draw(worldTransforms, mDefaultOverlayColor, mDefaultOverlayWeight);
        }

        /// <summary>
        /// Draws the IRenderable at worldPositions facing worldRotation, and scaled to worldScale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldRotation">Rotation matrix of the object.</param>
        /// <param name="worldScale">Scale along each axis of the object.</param>
        /// <param name="overlayColor">Color with which to modify the object.</param>
        /// <param name="overlayColorWeight">Amount to colorify the object. 0-none 1-full</param>
        public void Render(Vector3 worldPosition, Matrix worldRotation, Vector3 worldScale, Color overlayColor, float overlayColorWeight)
        {
            Matrix worldTransforms = Matrix.CreateScale(worldScale);
            worldTransforms *= worldRotation;
            worldTransforms *= Matrix.CreateTranslation(worldPosition);

            Draw(worldTransforms, overlayColor, overlayColorWeight);
        }

        /// <summary>
        /// Draws the IRenderable using the given world transform.
        /// </summary>
        /// <param name="worldTransform">The object's world transform.</param>
        public void Render(Matrix worldTransform)
        {
            Draw(worldTransform, mDefaultOverlayColor, mDefaultOverlayWeight);
        }

        /// <summary>
        /// Draws the IRenderable using the given world transform.
        /// </summary>
        /// <param name="worldTransform">Position of the object's center of mass in world coordinates.</param>
        /// <param name="overlayColor">Color with which to modify the object.</param>
        /// <param name="overlayColorWeight">Amount to colorify the object. 0-none 1-full</param>
        public void Render(Matrix worldTransform, Color overlayColor, float overlayColorWeight)
        {
            Draw(worldTransform, overlayColor, overlayColorWeight);
        }

        ///////////////////////////// Internal functions /////////////////////////////

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldTransform">The object's world transform.</param>
        /// <param name="overlayColor">Color with which to modify the object.</param>
        /// <param name="overlayColorWeight">Amount to colorify the object. 0-none 1-full</param>
        abstract protected void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight);
    }
}
