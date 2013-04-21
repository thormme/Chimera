using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class SkyBoxRenderable : Renderable
    {
        #region Constants

        const string DefaultTextureName = "overcastSkyBox";

        #endregion

        #region Public Properties

        private string mTextureName = null;
        public string TextureName
        {
            get { return mTextureName; }
            set { mTextureName = value; }
        }

        #endregion

        public SkyBoxRenderable(string textureName)
            : base("SKY_BOX_RENDERER", typeof(SkyBoxRenderer))
        {
            if (textureName != "default")
            {
                TextureName = textureName;
            }
            else
            {
                TextureName = DefaultTextureName;
            }
        }

        protected override void AlertAssetLibrary() { }

        protected override void Draw(Microsoft.Xna.Framework.Matrix worldTransform, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            SkyBoxRenderer.SkyBoxParameters parameters = new SkyBoxRenderer.SkyBoxParameters();
            parameters.Name = Name;
            parameters.OverlayColor  = overlayColor;
            parameters.OverlayWeight = overlayColorWeight;
            parameters.TextureName   = mTextureName;
            parameters.TryCull       = tryCull;
            parameters.World         = worldTransform;

            GraphicsManager.EnqueueRenderable(parameters, RendererType);
        }
    }
}
