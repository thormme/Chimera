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

        private bool mSpaghettify = false;
        public bool Spaghettify
        {
            get { return mSpaghettify; }
            set { mSpaghettify = value; }
        }

        private Vector3 mWormholePosition = Vector3.Zero;
        public Vector3 WormholePosition
        {
            get { return mWormholePosition; }
            set { mWormholePosition = value; }
        }

        private float mMaxWormholeDistance = 0.0f;
        public float MaxWormholeDistance
        {
            get { return mMaxWormholeDistance; }
            set { mMaxWormholeDistance = value; }
        }

        #endregion

        #region Public Methods

        /// <param name="modelName">Name of model stored in model database at startup.</param>
        public AnimateModel(string modelName, string defaultAnimation)
            : base(modelName, typeof(AnimateModelRenderer))
        {
            mSkinningData = AssetLibrary.LookupModelSkinningData(Name + mAnimationName);
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
                throw new Exception(Name + " does not contain bone: " + boneName);
            }

            return AssetLibrary.LookupTweakedBoneOrientation(Name, boneName) * AnimationPlayer.GetWorldTransforms()[boneIndex];
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
                    throw new Exception(animationName + " is not a valid animation for " + Name);
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

        protected override void AlertAssetLibrary()
        {
            if (AssetLibrary.LookupAnimateModel(Name) == null)
            {
                AssetLibrary.AddAnimateModel(Name, new AnimateModelRenderer(AssetLibrary.LookupInanimateModel(Name).Model));
            }
        }

        /// <summary>
        /// Draws animated model to the screen.
        /// </summary>
        /// <param name="worldTransform">Transformation of model in to place in world space.</param>
        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            AnimateModelRenderer.AnimateModelParameters parameters = new AnimateModelRenderer.AnimateModelParameters();
            parameters.BoundingBox = BoundingBox;
            parameters.Spaghettify = mSpaghettify;
            parameters.MaxWormholeDistance = mMaxWormholeDistance;
            parameters.Name = Name;
            parameters.OverlayColor = overlayColor;
            parameters.OverlayWeight = overlayColorWeight;
            parameters.SkinTransforms = AnimationPlayer.GetSkinTransforms(); ;
            parameters.TryCull = tryCull;
            parameters.World = worldTransform;
            parameters.WormholePosition = mWormholePosition;

            GraphicsManager.EnqueueRenderable(parameters, RendererType);
        }

        #endregion
    }
}
