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
        }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            TransparentModelRenderer.TransparentModelParameters parameters = new TransparentModelRenderer.TransparentModelParameters();
            parameters.AnimationOffset = Vector2.Zero;
            parameters.BoundingBox = BoundingBox;
            parameters.Name = mModelName;
            parameters.OverlayColor = overlayColor;
            parameters.OverlayWeight = overlayColorWeight;
            parameters.TryCull = tryCull;
            parameters.World = worldTransform;

            GraphicsManager.EnqueueTransparentRenderable(parameters);
        }
    }
}
