using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GraphicsLibrary
{
    public class AnimateModel : Renderable
    {
        private string          mModelName;
        private AnimationPlayer mAnimationPlayer;
        private SkinningData    mSkinningData;

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

        public void PlayAnimation(string animationName)
        {
            AnimationClip clip;
            if (!mSkinningData.AnimationClips.TryGetValue(animationName, out clip))
            {
                throw new InvalidTimeZoneException(animationName + " is not a valid animation for " + mModelName + ".");
            }

            mAnimationPlayer.StartClip(clip);
        }

        public void Update(GameTime gameTime)
        {
            mAnimationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
        }

        protected override void Draw(Matrix worldTransform)
        {
            Matrix[] skinTransforms = mAnimationPlayer.GetSkinTransforms();
            GraphicsManager.RenderSkinnedModel(mModelName, skinTransforms, worldTransform);
        }
    }
}
