using System;
using Microsoft.Xna.Framework;

namespace GraphicsLibrary
{
    abstract public class Renderable
    {
        #region Constants

        private Vector3 mDefaultWorldView  = new Vector3(0.0f, 0.0f, -1.0f);
        private Vector3 mDefaultWorldScale = new Vector3(1.0f);
        private Color mDefaultOverlayColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        private float mDefaultOverlayWeight = 0.0f;

        #endregion

        #region State

        protected string Name;
        protected Type RendererType;

        public BoundingBox BoundingBox
        {
            get { return mBoundingBox; }
            set { mBoundingBox = value; }
        }
        protected BoundingBox mBoundingBox;

        protected float mElapsedTime = 0.0f;

        #endregion

        #region Public Interface

        public Renderable(string name, Type rendererType)
        {
            Name = name;
            RendererType = rendererType;

            AlertAssetLibrary();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            mElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

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
        public void Render(Vector3 worldPosition, bool tryCull)
        {
            Render(worldPosition, mDefaultWorldView, mDefaultWorldScale, tryCull);
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
        /// Draws the IRenderable object at worldPosition using default scale and orientation.
        /// </summary>
        /// <param name="worldPosition">Poition of the object's center of mass in world coordinates.</param>
        public void Render(Vector3 worldPosition, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            Render(worldPosition, mDefaultWorldView, mDefaultWorldScale, overlayColor, overlayColorWeight, tryCull);
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
        public void Render(Vector3 worldPosition, Vector3 worldViewDirection, bool tryCull)
        {
            Render(worldPosition, worldViewDirection, mDefaultWorldScale, tryCull);
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
        /// Draws the IRenderable at worldPosition facing worldViewDirection using default scale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldViewDirection">View vector of object in world coordinates.  Used to calculate rotation.</param>
        public void Render(Vector3 worldPosition, Vector3 worldViewDirection, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            Render(worldPosition, worldViewDirection, mDefaultWorldScale, overlayColor, overlayColorWeight, tryCull);
        }

        /// <summary>
        /// Draws the IRenderable at worldPositions facing worldViewDirection, and scaled to worldScale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldViewDirection">View vector of object in world coordinates.  Used to calculate rotation.</param>
        /// <param name="worldScale">Scale along each axis of the object.</param>
        public void Render(Vector3 worldPosition, Vector3 worldViewDirection, Vector3 worldScale)
        {
            Draw(Matrix.CreateScale(worldScale) * Matrix.CreateWorld(worldPosition, worldViewDirection, Vector3.Up), mDefaultOverlayColor, mDefaultOverlayWeight, true);
        }

        /// <summary>
        /// Draws the IRenderable at worldPositions facing worldViewDirection, and scaled to worldScale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldViewDirection">View vector of object in world coordinates.  Used to calculate rotation.</param>
        /// <param name="worldScale">Scale along each axis of the object.</param>
        public void Render(Vector3 worldPosition, Vector3 worldViewDirection, Vector3 worldScale, bool tryCull)
        {
            Draw(Matrix.CreateScale(worldScale) * Matrix.CreateWorld(worldPosition, worldViewDirection, Vector3.Up), mDefaultOverlayColor, mDefaultOverlayWeight, tryCull);
        }

        /// <summary>
        /// Draws the IRenderable at worldPositions facing worldViewDirection, and scaled to worldScale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldViewDirection">View vector of object in world coordinates.  Used to calculate rotation.</param>
        /// <param name="worldScale">Scale along each axis of the object.</param>
        public void Render(Vector3 worldPosition, Vector3 worldViewDirection, Vector3 worldScale, Color overlayColor, float overlayColorWeight)
        {
            Draw(Matrix.CreateScale(worldScale) * Matrix.CreateWorld(worldPosition, worldViewDirection, Vector3.Up), overlayColor, overlayColorWeight, true);
        }

        /// <summary>
        /// Draws the IRenderable at worldPositions facing worldViewDirection, and scaled to worldScale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldViewDirection">View vector of object in world coordinates.  Used to calculate rotation.</param>
        /// <param name="worldScale">Scale along each axis of the object.</param>
        public void Render(Vector3 worldPosition, Vector3 worldViewDirection, Vector3 worldScale, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            Draw(Matrix.CreateScale(worldScale) * Matrix.CreateWorld(worldPosition, worldViewDirection, Vector3.Up), overlayColor, overlayColorWeight, tryCull);
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
        public void Render(Vector3 worldPosition, Matrix worldRotation, bool tryCull)
        {
            Render(worldPosition, worldRotation, mDefaultWorldScale, tryCull);
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
        /// Draws the IRenderable at worldPositions facing worldRotation using default scale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldRotation">Rotation matrix of the object.</param>
        public void Render(Vector3 worldPosition, Matrix worldRotation, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            Render(worldPosition, worldRotation, mDefaultWorldScale, overlayColor, overlayColorWeight, tryCull);
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

            Draw(worldTransforms, mDefaultOverlayColor, mDefaultOverlayWeight, true);
        }

        /// <summary>
        /// Draws the IRenderable at worldPositions facing worldRotation, and scaled to worldScale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldRotation">Rotation matrix of the object.</param>
        /// <param name="worldScale">Scale along each axis of the object.</param>
        public void Render(Vector3 worldPosition, Matrix worldRotation, Vector3 worldScale, bool tryCull)
        {
            Matrix worldTransforms = Matrix.CreateScale(worldScale);
            worldTransforms *= worldRotation;
            worldTransforms *= Matrix.CreateTranslation(worldPosition);

            Draw(worldTransforms, mDefaultOverlayColor, mDefaultOverlayWeight, tryCull);
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

            Draw(worldTransforms, overlayColor, overlayColorWeight, true);
        }

        /// <summary>
        /// Draws the IRenderable at worldPositions facing worldRotation, and scaled to worldScale.
        /// </summary>
        /// <param name="worldPosition">Position of the object's center of mass in world coordinates.</param>
        /// <param name="worldRotation">Rotation matrix of the object.</param>
        /// <param name="worldScale">Scale along each axis of the object.</param>
        /// <param name="overlayColor">Color with which to modify the object.</param>
        /// <param name="overlayColorWeight">Amount to colorify the object. 0-none 1-full</param>
        public void Render(Vector3 worldPosition, Matrix worldRotation, Vector3 worldScale, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            Matrix worldTransforms = Matrix.CreateScale(worldScale);
            worldTransforms *= worldRotation;
            worldTransforms *= Matrix.CreateTranslation(worldPosition);

            Draw(worldTransforms, overlayColor, overlayColorWeight, tryCull);
        }

        /// <summary>
        /// Draws the IRenderable using the given world transform.
        /// </summary>
        /// <param name="worldTransform">The object's world transform.</param>
        public void Render(Matrix worldTransform)
        {
            Draw(worldTransform, mDefaultOverlayColor, mDefaultOverlayWeight, true);
        }

        /// <summary>
        /// Draws the IRenderable using the given world transform.
        /// </summary>
        /// <param name="worldTransform">The object's world transform.</param>
        public void Render(Matrix worldTransform, bool tryCull)
        {
            Draw(worldTransform, mDefaultOverlayColor, mDefaultOverlayWeight, tryCull);
        }

        /// <summary>
        /// Draws the IRenderable using the given world transform.
        /// </summary>
        /// <param name="worldTransform">Position of the object's center of mass in world coordinates.</param>
        /// <param name="overlayColor">Color with which to modify the object.</param>
        /// <param name="overlayColorWeight">Amount to colorify the object. 0-none 1-full</param>
        public void Render(Matrix worldTransform, Color overlayColor, float overlayColorWeight)
        {
            Draw(worldTransform, overlayColor, overlayColorWeight, true);
        }

        /// <summary>
        /// Draws the IRenderable using the given world transform.
        /// </summary>
        /// <param name="worldTransform">Position of the object's center of mass in world coordinates.</param>
        /// <param name="overlayColor">Color with which to modify the object.</param>
        /// <param name="overlayColorWeight">Amount to colorify the object. 0-none 1-full</param>
        public void Render(Matrix worldTransform, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            Draw(worldTransform, overlayColor, overlayColorWeight, tryCull);
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// 
        /// </summary>
        protected abstract void AlertAssetLibrary();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldTransform">The object's world transform.</param>
        /// <param name="overlayColor">Color with which to modify the object.</param>
        /// <param name="overlayColorWeight">Amount to colorify the object. 0-none 1-full</param>
        protected abstract void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight, bool tryCull);

        #endregion
    }
}
