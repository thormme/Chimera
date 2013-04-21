using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsLibrary
{
    public class UIModel : Renderable
    {
        public UInt32 ObjectID;

        public UIModel(string modelName)
            : base(modelName, typeof(UIModelRenderer))
        {
        }

        protected override void AlertAssetLibrary()
        {
            if (AssetLibrary.LookupUIModel(Name) == null)
            {
                AssetLibrary.AddUIModel(Name, new UIModelRenderer(AssetLibrary.LookupInanimateModel(Name).Model));
            }
        }

        protected override void Draw(Microsoft.Xna.Framework.Matrix worldTransform, Microsoft.Xna.Framework.Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            RendererBase.RendererParameters parameters = new RendererBase.RendererParameters();
            parameters.Name = Name;
            parameters.ObjectID = ObjectID;
            parameters.OverlayColor = overlayColor;
            parameters.OverlayWeight = overlayColorWeight;
            parameters.TryCull = tryCull;
            parameters.World = worldTransform;

            GraphicsManager.EnqueueRenderable(parameters, RendererType);
        }
    }
}
