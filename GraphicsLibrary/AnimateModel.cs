﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using AnimationUtilities;

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

        /// <summary>
        /// Contains state of bones at current frame of animation.
        /// </summary>
        public AnimationPlayer AnimationPlayer
        {
            get
            {
                return mAnimationPlayer;
            }
        }

        #endregion

        #region Public Methods

        /// <param name="modelName">Name of model stored in model database at startup.</param>
        public AnimateModel(string modelName, string defaultAnimation)
        {
            mModelName = modelName;

            mSkinningData = AssetLibrary.LookupModelSkinningData(mModelName + mAnimationName);
            if (mSkinningData == null)
            {
                throw new Exception("This model does not contain skinningData.");
            }

            mAnimationPlayer = new AnimationPlayer(mSkinningData);

            PlayAnimation(defaultAnimation, false);
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

            return AssetLibrary.LookupTweakedBoneOrientation(mModelName, boneName) * AnimationPlayer.GetWorldTransforms()[boneIndex];
        }

        /// <summary>
        /// Begins playing the animation with name animationName.
        /// </summary>
        /// <param name="animationName">Name of the current animation.</param>
        /// <param name="loop">Whether the animation will loop.</param>
        public void PlayAnimation(string animationName, bool loop)
        {
            if (animationName != mAnimationName)
            {
                mAnimationName = animationName;

                AnimationClip clip;
                if (!SkinningData.AnimationClips.TryGetValue(animationName, out clip))
                {
                    throw new Exception(animationName + " is not a valid animation for " + mModelName);
                }

                AnimationPlayer.StartClip(clip, loop);
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
        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            AnimateModelRenderer.AnimateModelParameters parameters = new AnimateModelRenderer.AnimateModelParameters();
            parameters.BoundingBox = BoundingBox;
            parameters.Name = mModelName;
            parameters.OverlayColor = overlayColor;
            parameters.OverlayWeight = overlayColorWeight;
            parameters.SkinTransforms = AnimationPlayer.GetSkinTransforms(); ;
            parameters.TryCull = tryCull;
            parameters.World = worldTransform;

            GraphicsManager.EnqueueRenderable(parameters);
        }

        #endregion
    }
}
