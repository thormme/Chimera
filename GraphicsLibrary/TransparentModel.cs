using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class TransparentModel : Renderable
    {
        private string mModelName;

        public new BoundingBox BoundingBox
        {
            get { return mBoundingBox; }
        }
        private BoundingBox mBoundingBox;

        public TransparentModel(string modelName)
        {
            mModelName = modelName;
            mBoundingBox = GraphicsManager.BuildModelBoundingBox(mModelName);
        }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight)
        {
            BoundingBox transformedBBox = new BoundingBox(Vector3.Transform(mBoundingBox.Min, worldTransform), Vector3.Transform(mBoundingBox.Max, worldTransform));
            GraphicsManager.RenderTransparentModel(mModelName, worldTransform, transformedBBox, overlayColor, overlayColorWeight, Vector2.Zero);
        }
    }
}
