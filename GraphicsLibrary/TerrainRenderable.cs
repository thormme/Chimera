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
            BuildTerrainBoundingBox();
        }

        public Vector4 LayerMask
        {
            get { return mLayerMask; }
            set { mLayerMask = value; }
        }
        private Vector4 mLayerMask = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

        public enum CursorShape { NONE, CIRCLE, BLOCK };

        public CursorShape DrawCursor
        {
            get { return mDrawCursor; }
            set { mDrawCursor = value; }
        }
        private CursorShape mDrawCursor;

        public Vector3 CursorPosition
        {
            get { return mCursorPosition; }
            set { mCursorPosition = value; }
        }
        private Vector3 mCursorPosition;

        public float CursorInnerRadius
        {
            get { return mCursorInnerRadius; }
            set { mCursorInnerRadius = value; }
        }
        private float mCursorInnerRadius;

        public float CursorOuterRadius
        {
            get { return mCursorOuterRadius; }
            set { mCursorOuterRadius = value; }
        }
        private float mCursorOuterRadius;

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight, bool tryCull)
        {
            BoundingBox[,] transformedBoundingBoxes = new BoundingBox[mBoundingBoxes.GetLength(0), mBoundingBoxes.GetLength(1)];
            for (int row = 0; row < mBoundingBoxes.GetLength(0); ++row)
            {
                for (int col = 0; col < mBoundingBoxes.GetLength(1); ++col)
                {
                    transformedBoundingBoxes[row, col] = new BoundingBox(Vector3.Transform(mBoundingBoxes[row, col].Min, worldTransform), Vector3.Transform(mBoundingBoxes[row, col].Max, worldTransform));
                }
            }

            TerrainRenderer.TerrainParameters parameters = new TerrainRenderer.TerrainParameters();
            parameters.BoundingBoxes = transformedBoundingBoxes;
            parameters.DrawCursor = DrawCursor;
            parameters.CursorPosition = CursorPosition;
            parameters.CursorInnerRadius = CursorInnerRadius;
            parameters.CursorOuterRadius = CursorOuterRadius;
            parameters.Name = mTerrainName;
            parameters.OverlayColor = overlayColor;
            parameters.OverlayWeight = overlayColorWeight;
            parameters.TextureMask = LayerMask;
            parameters.TryCull = tryCull;
            parameters.World = worldTransform;

            GraphicsManager.EnqueueRenderable(parameters);
        }

        private void BuildTerrainBoundingBox()
        {
            TerrainHeightMap terrain = AssetLibrary.LookupTerrain(mTerrainName).HeightMap;

            mBoundingBoxes = new BoundingBox[terrain.NumChunksVertical, terrain.NumChunksHorizontal];

            for (int row = 0; row < terrain.NumChunksVertical; ++row)
            {
                for (int col = 0; col < terrain.NumChunksHorizontal; ++col)
                {
                    Vector3 minExtent = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                    Vector3 maxExtent = new Vector3(float.MinValue, float.MinValue, float.MinValue);

                    BoundVertexBuffer(ref minExtent, ref maxExtent, terrain.VertexBuffers[row, col], terrain.VertexBuffers[row, col].VertexCount);

                    mBoundingBoxes[row, col] = new BoundingBox(minExtent, maxExtent);
                }
            }
        }

        private void BoundVertexBuffer(ref Vector3 min, ref Vector3 max, VertexBuffer vertexBuffer, int vertexCount)
        {
            int vertexStride = vertexBuffer.VertexDeclaration.VertexStride;
            int vertexBufferSize = vertexCount * vertexStride;

            float[] vertexData = new float[vertexBufferSize / sizeof(float)];
            vertexBuffer.GetData<float>(vertexData);

            for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
            {
                Vector3 localPosition = new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]);

                min = Vector3.Min(min, localPosition);
                max = Vector3.Max(max, localPosition);
            }
        }
    }
}
