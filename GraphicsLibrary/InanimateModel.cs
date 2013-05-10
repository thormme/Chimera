using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class InanimateModel : Renderable
    {
        public UInt32 ObjectID;

        public InanimateModel(string modelName)
            : base(modelName, typeof(ModelRenderer))
        {
        }

        protected override void AlertAssetLibrary() { }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            
            if (Name == "rock1dark_collision")
            {
                overlayColor = Color.Beige;
                overlayColorWeight = 1.0f;
            }

            ModelRenderer.RendererParameters parameters = new RendererBase.RendererParameters();
            parameters.BoundingBox = BoundingBox;
            parameters.Name = Name;
            parameters.OverlayColor = overlayColor;
            parameters.OverlayWeight = overlayColorWeight;
            parameters.TryCull = tryCull;
            parameters.World = worldTransform;
            parameters.ObjectID = ObjectID;

            GraphicsManager.EnqueueRenderable(parameters, RendererType);
        }
    }
}
