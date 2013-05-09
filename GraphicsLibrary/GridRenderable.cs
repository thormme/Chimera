using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsLibrary
{
    public class GridRenderable : Renderable
    {
        public GridRenderable(string gridName)
            : base(gridName, typeof(GridRenderer))
        {
        }

        protected override void AlertAssetLibrary()
        {
        }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            RendererBase.RendererParameters parameters = new RendererBase.RendererParameters();
            parameters.Name = Name;
            parameters.TryCull = false;
            parameters.World = worldTransform;

            GraphicsManager.EnqueueRenderable(parameters, RendererType);
        }
    }
}
