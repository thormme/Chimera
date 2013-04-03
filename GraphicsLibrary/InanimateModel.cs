﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class InanimateModel : Renderable
    {
        private string mModelName;

        public InanimateModel(string modelName)
        {
            mModelName = modelName;
        }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight)
        {
            ModelRenderer.RendererParameters parameters = new RendererBase.RendererParameters();
            parameters.BoundingBox = BoundingBox;
            parameters.Name = mModelName;
            parameters.OverlayColor = overlayColor;
            parameters.OverlayWeight = overlayColorWeight;
            parameters.World = worldTransform;

            GraphicsManager.EnqueueRenderable(parameters);
        }
    }
}
