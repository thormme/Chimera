using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GraphicsLibrary
{
    public class SkyBoxRenderable : Renderable
    {
        private string mTextureName = null;
        public string TextureName
        {
            get { return mTextureName; }
            set { mTextureName = value; }
        }

        public SkyBoxRenderable(string textureName)
        {
            if (textureName != "default")
            {
                TextureName = textureName;
            }
        }

        protected override void Draw(Microsoft.Xna.Framework.Matrix worldTransform, Color overlayColor, float overlayColorWeight)
        {
            GraphicsManager.RenderSkyBox(TextureName, worldTransform, overlayColor, overlayColorWeight);
        }
    }
}
