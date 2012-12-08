using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameConstructLibrary;

namespace GraphicsLibrary
{
    public class TerrainRenderable : Renderable
    {
        private string mTerrainName;

        public TerrainRenderable(string terrainName)
        {
            mTerrainName = terrainName;
        }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight)
        {
            GraphicsManager.RenderTerrain(mTerrainName, worldTransform, overlayColor, overlayColorWeight);
        }
    }
}
