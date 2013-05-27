using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class TransparentModel : Renderable
    {
        public TransparentModel(string modelName) : base(modelName, typeof(TransparentModelRenderer))
        {
        }

        protected override void AlertAssetLibrary()
        {
            if (AssetLibrary.LookupTransparentModel(Name) == null)
            {
                AssetLibrary.AddTransparentModel(Name, new TransparentModelRenderer(AssetLibrary.LookupInanimateModel(Name).Model));
            }
        }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            TransparentModelRenderer.TransparentModelParameters parameters = new TransparentModelRenderer.TransparentModelParameters();
            parameters.AnimationTransformation = Matrix.Identity;
            parameters.BoundingBox = BoundingBox;
            parameters.Name = Name;
            parameters.OverlayColor = overlayColor;
            parameters.OverlayWeight = overlayColorWeight;
            parameters.TryCull = tryCull;
            parameters.World = worldTransform;

            GraphicsManager.EnqueueTransparentRenderable(parameters);
        }
    }
}
