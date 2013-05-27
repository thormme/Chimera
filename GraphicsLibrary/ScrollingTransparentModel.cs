using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GraphicsLibrary
{
    public class ScrollingTransparentModel : Renderable
    {
        public Vector2 AnimationRate
        {
            get { return mAnimationRate; }
            set { mAnimationRate = value; }
        }

        private Vector2 mAnimationRate = Vector2.Zero;

        public ScrollingTransparentModel(string modelName, Vector2 animationRate)
            : base(modelName, typeof(TransparentModelRenderer))
        {
            mAnimationRate = animationRate;
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
            parameters.AnimationTransformation = Matrix.CreateRotationZ((mAnimationRate.X * mElapsedTime) % MathHelper.TwoPi);
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
