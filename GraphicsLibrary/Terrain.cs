using GameConstructLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class Terrain : Renderable
    {
        private string mName;
        private TerrainHeightMap mHeightMap;

        public Terrain(string terrainName)
        {
            mName = terrainName;
        }

        protected override void Draw(Matrix worldTransform)
        {
            GraphicsManager.RenderTerrain(mName, worldTransform);
        }
    }
}
