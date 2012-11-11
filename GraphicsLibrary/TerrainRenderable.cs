using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class TerrainRenderable : Renderable
    {
        private TerrainDescription mHeightMap;

        public TerrainRenderable(string terrainName)
        {
            mHeightMap = GraphicsManager.LookupTerrain(terrainName);
        }

        protected override void Draw(Matrix worldTransform)
        {
            GraphicsManager.RenderTerrain(mHeightMap, worldTransform);
        }
    }
}
