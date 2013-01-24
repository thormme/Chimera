using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameConstructLibrary;

namespace GraphicsLibrary
{
    public class TerrainRenderable : Renderable
    {
        private string mTerrainName;

        public BoundingBox BoundingBox
        {
            get { return mBoundingBox; }
        }
        private BoundingBox mBoundingBox;

        public TerrainRenderable(string terrainName)
        {
            mTerrainName = terrainName;
            mBoundingBox = GraphicsManager.BuildTerrainBoundingBox(terrainName);
        }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight)
        {
            GraphicsManager.RenderTerrain(mTerrainName, worldTransform, mBoundingBox, overlayColor, overlayColorWeight);
        }
    }
}
