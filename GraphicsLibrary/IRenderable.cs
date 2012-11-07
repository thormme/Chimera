using Microsoft.Xna.Framework;

namespace GraphicsLibrary
{
    abstract class Renderable
    {
        private Vector3 mDefaultWorldView  = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 mDefaultWorldScale = new Vector3(1.0f, 1.0f, 1.0f);

        protected Matrix mWorldTransforms;

        /// <summary>
        /// Positions the IRenderable object using default or previously set scale and orientation so that is it drawn correctly in Render.
        /// </summary>
        /// <param name="worldPosition">Poition of the object's center of mass in world coordinates.</param>
        public void Update(Vector3 worldPosition)
        {
            Update(worldPosition, mDefaultWorldView, mDefaultWorldScale);
        }

        /// <summary>
        /// Positions and orients the IRenderable object using default or previously set scale so that it is drawn correctly in Render.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldViewDirection">View vector of object in world coordinates.  Used to calculate rotation.</param>
        public void Update(Vector3 worldPosition, Vector3 worldViewDirection)
        {
            Update(worldPosition, worldViewDirection, mDefaultWorldScale);
        }

        /// <summary>
        /// Positions, orients, and scales the IRenderable object so that it is drawn correctly in Render.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldViewDirection">View vector of object in world coordinates.  Used to calculate rotation.</param>
        /// <param name="worldScale">Scale along each axis of the object relative to normalized model scale ([-0.5, -0.5, -0.5] to [0.5, 0.5, 0.5]).</param>
        public void Update(Vector3 worldPosition, Vector3 worldViewDirection, Vector3 worldScale)
        {
            mWorldTransforms = Matrix.Identity;
        }

        /// <summary>
        /// Draws the IRenderable object to the screen with the configuration provided in Update.
        /// </summary>
        public abstract void Render();
    }
}
