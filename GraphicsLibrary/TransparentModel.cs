using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class TransparentModel : Renderable
    {
        private string mModelName;

        public TransparentModel(string modelName)
        {
            mModelName = modelName;
            mBoundingBox = GraphicsManager.BuildModelBoundingBox(mModelName);
        }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight)
        {
            GraphicsManager.RenderTransparentModel(mModelName, worldTransform, mBoundingBox, overlayColor, overlayColorWeight, Vector2.Zero);
        }
    }
}
