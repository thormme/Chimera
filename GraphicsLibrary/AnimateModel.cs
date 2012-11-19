using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GraphicsLibrary
{
    /// <summary>
    /// Rigged model with baked in animation.
    /// </summary>
    public class AnimateModel : Renderable
    {
        #region Fields

        private string          mModelName;
        private AnimationPlayer mAnimationPlayer;
        private SkinningData    mSkinningData;

        #endregion

        #region Public Properties

        /// <summary>
        /// Configuration of bone transforms.
        /// </summary>
        public SkinningData SkinningData
        {
            get
            {
                return mSkinningData;
            }
        }

        #endregion

        #region Public Methods

        /// <param name="modelName">Name of model stored in model database at startup.</param>
        public AnimateModel(string modelName)
        {
            mModelName = modelName;

            mSkinningData = GraphicsManager.LookupModelSkinningData(mModelName);
            if (mSkinningData == null)
            {
                throw new Exception("This model does not contain skinningData.");
            }

            mAnimationPlayer = new AnimationPlayer(mSkinningData);
        }

        /// <summary>
        /// Begins playing the animation with name animationName.
        /// </summary>
        /// <param name="animationName">Name of the current animation.</param>
        public void PlayAnimation(string animationName)
        {
            AnimationClip clip;
            if (!mSkinningData.AnimationClips.TryGetValue(animationName, out clip))
            {
                throw new InvalidTimeZoneException(animationName + " is not a valid animation for " + mModelName + ".");
            }

            mAnimationPlayer.StartClip(clip);
        }

        /// <summary>
        /// Increments current frame in animation.  Modifies underlying bones accordingly.
        /// </summary>
        /// <param name="gameTime">Time elapsed since last frame.</param>
        public void Update(GameTime gameTime)
        {
            mAnimationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
        }

        /// <summary>
        /// Draws animated model to the screen.
        /// </summary>
        /// <param name="worldTransform">Transformation of model in to place in world space.</param>
        protected override void Draw(Matrix worldTransform)
        {
            Matrix[] skinTransforms = mAnimationPlayer.GetSkinTransforms();
            GraphicsManager.RenderSkinnedModel(mModelName, skinTransforms, worldTransform);
        }

        #endregion
    }
}
