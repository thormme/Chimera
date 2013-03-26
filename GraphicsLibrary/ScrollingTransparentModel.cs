using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GraphicsLibrary
{
    public class ScrollingTransparentModel : Renderable
    {
        private string mModelName;

        public float HorizontalVelocity
        {
            get { return mScrollVelocity.X; }
            set { mScrollVelocity.X = value; }
        }

        public float VerticalVelocity
        {
            get { return mScrollVelocity.Y; }
            set { mScrollVelocity.Y = value; }
        }

        public Vector2 ScrollVelocity
        {
            get { return mScrollVelocity; }
            set { mScrollVelocity = value; }
        }
        private Vector2 mScrollVelocity = Vector2.Zero;

        public ScrollingTransparentModel(string modelName)
        {
            mModelName = modelName;
        }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight)
        {
            GraphicsManager.RenderTransparentModel(mModelName, worldTransform, mBoundingBox, overlayColor, overlayColorWeight, ScrollVelocity);
        }
    }
}
