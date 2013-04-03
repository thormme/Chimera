using Microsoft.Xna.Framework;
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

        #endregion

        #region Private Variables

        private string mTextureName;
        private float mSeaLevel;

        #endregion

        public WaterRenderable(string textureName, float seaLevel)
        {
            mSeaLevel = seaLevel;
            mTextureName = textureName;
        }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight)
        {
            WaterRenderer.WaterParameters parameters = new WaterRenderer.WaterParameters();
            parameters.AnimationOffset = Vector2.Zero;
            parameters.Name = "WATER_RENDERER";
            parameters.OverlayColor = overlayColor;
            parameters.OverlayWeight = overlayColorWeight;
            parameters.SeaLevel = mSeaLevel;
            parameters.TextureName = mTextureName;
            parameters.World = worldTransform;

            GraphicsManager.EnqueueRenderable(parameters);
        }
    }
}
