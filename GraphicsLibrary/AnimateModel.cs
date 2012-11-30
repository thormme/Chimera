using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace GraphicsLibrary
{
    /// <summary>
    /// Rigged model with baked in animation.
    /// </summary>
    public class AnimateModel : Renderable
    {
        #region Fields

        private string          mModelName;
        private string          mAnimationName;
        private Dictionary<string, AnimationPlayer> mAnimationPlayerDatabase;
        private Dictionary<string, SkinningData>    mSkinningDataDatabase;

        string mBakedAnimationName = "Take 001";

        #endregion

        #region Public Properties

        /// <summary>
        /// Configuration of bone transforms.
        /// </summary>
        public SkinningData SkinningData
        {
            get
            {
                return mSkinningDataDatabase[mAnimationName];
            }
        }

        /// <summary>
        /// Contains state of bones at current frame of animation.
        /// </summary>
        public AnimationPlayer AnimationPlayer
        {
            get
            {
                return mAnimationPlayerDatabase[mAnimationName];
            }
        }

        #endregion

        #region Public Methods

        /// <param name="modelName">Name of model stored in model database at startup.</param>
        public AnimateModel(string modelName, string defaultAnimation)
        {
            mModelName = modelName;

            mAnimationPlayerDatabase = new Dictionary<string, GraphicsLibrary.AnimationPlayer>();
            mSkinningDataDatabase    = new Dictionary<string, GraphicsLibrary.SkinningData>();
            
            PlayAnimation(defaultAnimation);
        }

        /// <summary>
        /// Local animate model space transform matrix for individual bone.
        /// </summary>
        public Matrix GetBoneTransform(string boneName)
        {
            int boneIndex;
            if (!SkinningData.BoneIndices.TryGetValue(boneName, out boneIndex))
            {
                throw new Exception(mModelName + " does not contain bone: " + boneName);
            }

            return GraphicsManager.LookupTweakedBoneOrientation(mModelName, boneName) * AnimationPlayer.GetWorldTransforms()[boneIndex];
        }

        /// <summary>
        /// Begins playing the animation with name animationName.
        /// </summary>
        /// <param name="animationName">Name of the current animation.</param>
        public void PlayAnimation(string animationName)
        {
            string fullAnimationName = "_" + animationName;

            if (fullAnimationName != mAnimationName)
            {
                mAnimationName = fullAnimationName;

                // Create new skinning data
                if (!mSkinningDataDatabase.ContainsKey(mAnimationName))
                {
                    SkinningData skinningData = GraphicsManager.LookupModelSkinningData(mModelName + mAnimationName);
                    if (skinningData == null)
                    {
                        throw new Exception("This model does not contain skinningData.");
                    }

                    mSkinningDataDatabase.Add(mAnimationName, skinningData);

                    mAnimationPlayerDatabase.Add(mAnimationName, new AnimationPlayer(skinningData));
                }

                AnimationClip clip;
                if (!SkinningData.AnimationClips.TryGetValue(mBakedAnimationName, out clip))
                {
                    throw new InvalidTimeZoneException(mBakedAnimationName + " is not a valid animation for " + mModelName + "_" + mAnimationName + ".");
                }

                AnimationPlayer.StartClip(clip);
            }
        }

        /// <summary>
        /// Increments current frame in animation.  Modifies underlying bones accordingly.
        /// </summary>
        /// <param name="gameTime">Time elapsed since last frame.</param>
        public void Update(GameTime gameTime)
        {
            AnimationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
        }

        /// <summary>
        /// Draws animated model to the screen.
        /// </summary>
        /// <param name="worldTransform">Transformation of model in to place in world space.</param>
        protected override void Draw(Matrix worldTransform)
        {
            Matrix[] skinTransforms = AnimationPlayer.GetSkinTransforms();
            GraphicsManager.RenderSkinnedModel(mModelName + mAnimationName, skinTransforms, worldTransform);
        }

        #endregion
    }
}
