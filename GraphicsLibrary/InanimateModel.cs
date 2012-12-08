using Microsoft.Xna.Framework;
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
            GraphicsManager.RenderUnskinnedModel(mModelName, worldTransform, overlayColor, overlayColorWeight);
        }
    }
}
