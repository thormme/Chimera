using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GraphicsLibrary
{
    public class ScrollingTransparentModel : Renderable
    {
        private string mModelName;

        public new BoundingBox BoundingBox
        {
            get { return mBoundingBox; }
        }
        private BoundingBox mBoundingBox;

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
            mBoundingBox = GraphicsManager.BuildModelBoundingBox(mModelName);
        }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight)
        {
            BoundingBox transformedBoundingBox = new BoundingBox(Vector3.Transform(mBoundingBox.Min, worldTransform), Vector3.Transform(mBoundingBox.Max, worldTransform));
            GraphicsManager.RenderTransparentModel(mModelName, worldTransform, transformedBoundingBox, overlayColor, overlayColorWeight, ScrollVelocity);
        }
    }
}
