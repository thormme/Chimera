using GameConstructLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class Terrain : Renderable
    {
        private TerrainDescription mHeightMap;

        public Terrain(string terrainName)
        {
            mHeightMap = GraphicsManager.LookupTerrain(terrainName);
        }

        protected override void Draw(Matrix worldTransform)
        {
            GraphicsManager.RenderTerrain(mHeightMap, worldTransform);
        }
    }
}
