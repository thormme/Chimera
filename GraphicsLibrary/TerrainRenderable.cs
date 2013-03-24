using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameConstructLibrary;

namespace GraphicsLibrary
{
    public class TerrainRenderable : Renderable
    {
        private string mTerrainName;

        public new BoundingBox[,] BoundingBoxes
        {
            get { return mBoundingBoxes; }
        }
        private BoundingBox[,] mBoundingBoxes;

        public TerrainRenderable(string terrainName)
        {
            mTerrainName = terrainName;
            mBoundingBoxes = GraphicsManager.BuildTerrainBoundingBox(terrainName);
        }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight)
        {
            BoundingBox[,] transformedBoundingBoxes = new BoundingBox[mBoundingBoxes.GetLength(0), mBoundingBoxes.GetLength(1)];
            for (int row = 0; row < mBoundingBoxes.GetLength(0); ++row)
            {
                for (int col = 0; col < mBoundingBoxes.GetLength(1); ++col)
                {
                    transformedBoundingBoxes[row, col] = new BoundingBox(Vector3.Transform(mBoundingBoxes[row, col].Min, worldTransform), Vector3.Transform(mBoundingBoxes[row, col].Max, worldTransform));
                }
            }

            GraphicsManager.RenderTerrain(mTerrainName, worldTransform, transformedBoundingBoxes, overlayColor, overlayColorWeight);
        }
    }
}
