﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsLibrary
{
    public class WaterRenderable : Renderable
    {
        #region Public Properties

        public float SeaLevel
        {
            get { return mSeaLevel; }
            set { mSeaLevel = value; }
        }
        
        public string TextureName
        {
            get { return mTextureName; }
            set { mTextureName = value; }
        }

        public Vector2 AnimationRate
        {
            get { return mAnimationRate; }
            set { mAnimationRate = value; }
        }

        #endregion

        #region Private Variables

        private string mTextureName;
        private float mSeaLevel;
        private Vector2 mAnimationRate;

        #endregion

        public WaterRenderable(string textureName, Vector2 animationRate, float seaLevel)
            : base("WATER_RENDERER", typeof(WaterRenderer))
        {
            mAnimationRate = animationRate;
            mSeaLevel = seaLevel;
            mTextureName = textureName;
        }

        protected override void AlertAssetLibrary() { }
        
        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            WaterRenderer.WaterParameters parameters = new WaterRenderer.WaterParameters();
            parameters.TextureAnimationOffset = new Vector2((mAnimationRate.X * mElapsedTime) % 1.0f, (mAnimationRate.Y * mElapsedTime) % 1.0f);
            parameters.Name = Name;
            parameters.OverlayColor = overlayColor;
            parameters.OverlayWeight = overlayColorWeight;
            parameters.SeaLevel = mSeaLevel;
            parameters.TextureName = mTextureName;
            parameters.TryCull = tryCull;
            parameters.World = worldTransform;

            GraphicsManager.EnqueueRenderable(parameters, RendererType);
        }
    }
}
