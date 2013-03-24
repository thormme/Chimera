using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GraphicsLibrary
{
    public class Billboard : Renderable
    {
        private string mName;

        public new BoundingBox BoundingBox
        {
            get { return mBoundingBox; }
        }
        private BoundingBox mBoundingBox;

        public float Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }
        private float mWidth;

        public float Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }
        private float mHeight;

        public Billboard(string textureName, float width, float height)
        {
            mName = textureName;
            mWidth = width;
            mHeight = height;
        }

        protected override void Draw(Microsoft.Xna.Framework.Matrix worldTransform, Microsoft.Xna.Framework.Color overlayColor, float overlayColorWeight)
        {
            return;
        }
    }
}
