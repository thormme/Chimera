using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GraphicsLibrary;
using Utility;
using System.IO;

namespace GameConstructLibrary
{
    /// <summary>
    /// Terrain represented by a height map with the ability to modify itself.
    /// </summary>
    public class TerrainHeightMap
    {
        #region Public Properties

        /// <summary>
        /// Matrix of VertexBuffers containing copies of vertices for each chunk.
        /// </summary>
        public VertexBuffer[,] VertexBuffers
        {
            get { return mVertexBuffers; }
            set { mVertexBuffers = value; }
        }
        private VertexBuffer[,] mVertexBuffers;

        /// <summary>
        /// Matrix of IndexBuffers containing copies of indices for each chunk.
        /// </summary>
        public IndexBuffer[,] IndexBuffers
        {
            get { return mIndexBuffers; }
            set { mIndexBuffers = value; }
        }
        private IndexBuffer[,] mIndexBuffers;

        /// <summary>
        /// Collection of vertices assigned to vertical sides.
        /// </summary>
        public VertexBuffer[] EdgeVertexBuffers
        {
            get { return mEdgeVertexBuffers; }
            set { mEdgeVertexBuffers = value; }
        }
        private VertexBuffer[] mEdgeVertexBuffers;

        /// <summary>
        /// Collection of indices assigned to vertical sides.
        /// </summary>
        public IndexBuffer[] EdgeIndexBuffers
        {
            get { return mEdgeIndexBuffers; }
            set { mEdgeIndexBuffers = value; }
        }
        private IndexBuffer[] mEdgeIndexBuffers;

        /// <summary>
        /// Total number of vertices heightmap is wide.
        /// </summary>
        public int Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }
        private int mWidth;

        /// <summary>
        /// Total number of vertices heightmap is tall.
        /// </summary>
        public int Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }
        private int mHeight;

        /// <summary>
        /// Total number of vertices (including redundant edges) that each chunk is wide.
        /// </summary>
        public int ChunkWidth
        {
            get { return mChunkWidth; }
            set { mChunkWidth = value; }
        }
        private int mChunkWidth;

        /// <summary>
        /// Total number of vertices (including redundant edges) that each chunk is tall.
        /// </summary>
        public int ChunkHeight
        {
            get { return mChunkHeight; }
            set { mChunkHeight = value; }
        }
        private int mChunkHeight;

        /// <summary>
        /// Total number of chunks height map is horizontally broken in to.
        /// </summary>
        public int NumChunksHorizontal
        {
            get { return mNumChunksHorizontal; }
            set { mNumChunksHorizontal = value; }
        }
        private int mNumChunksHorizontal;

        /// <summary>
        /// Total number of chunks heightmap is vertically broken in to.
        /// </summary>
        public int NumChunksVertical
        {
            get { return mNumChunksVertical; }
            set { mNumChunksVertical = value; }
        }
        private int mNumChunksVertical;

        /// <summary>
        /// Whether terrain modification falls off as radius increases.
        /// </summary>
        public bool IsFeathered
        {
            get { return mIsFeathered; }
            set { mIsFeathered = value; }
        }
        private bool mIsFeathered = false;

        /// <summary>
        /// Whether the brush is a block or a circle.
        /// </summary>
        public bool IsBlock
        {
            get { return mIsBlock; }
            set { mIsBlock = value; }
        }
        private bool mIsBlock = false;

        #endregion

        #region Private Variables

        /// <summary>
        /// 
        /// </summary>
        GraphicsDevice mDevice;

        /// <summary>
        /// Table for saving on vertex buffer writes.
        /// </summary>
        private bool[,] mDirtyChunks;

        /// <summary>
        /// Table for saving on edge vertex buffer writes.
        /// </summary>
        private bool[] mEdgeDirtyChunks;

        /// <summary>
        /// Constant time random-access lookup of chunks a vertex belongs to.
        /// </summary>
        private List<Tuple<int, int>>[,] mChunkLookupTable;

        /// <summary>
        /// Modifiable representation of heightmap.
        /// </summary>
        private VertexPositionColorTextureNormal[] mVertices;

        private VertexPositionColorTextureNormal[] mEdgeVertices;

        private float mFlattenHeightSum = 0.0f;
        private int mFlattenNumVertices = 0;

        private float[,] mSmoothHeightAverages;
        private int mSmoothXBaseIndex = 0;
        private int mSmoothZBaseIndex = 0;

        #endregion

        #region Constants

        private Vector2[] mVertex0Offset = new Vector2[]
        {
            new Vector2(-1, -1),
            new Vector2(-1, -1),
            new Vector2(0, -1),
            new Vector2(-1, 0),
            new Vector2(0, 0),
            new Vector2(0, 0)
        };

        private Vector2[] mVertex1Offset = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(0, -1),
            new Vector2(1, 0),
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(1, 0)
        };

        private Vector2[] mVertex2Offset = new Vector2[]
        {
            new Vector2(-1, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        #endregion

        #region Structures

        public struct VertexPositionColorTextureNormal
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 TexCoord;
            public Color Color;

            public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
            (
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(sizeof(float) * 8, VertexElementFormat.Color, VertexElementUsage.Color, 0)
            );
        }

        #endregion

        #region Construction

        public TerrainHeightMap(Texture2D heightMap, int numChunksHorizontal, int numChunksVertical, GraphicsDevice device)
        {
            mDevice = device;

            mHeight = heightMap.Height;
            mWidth = heightMap.Width;

            mVertices = new VertexPositionColorTextureNormal[mHeight * mWidth];

            mEdgeVertices = new VertexPositionColorTextureNormal[2 * mWidth + 2 * mHeight];

            double[] numQuads = new double[2] { mHeight - 1, mWidth - 1 };
            double[] numChunks = new double[2] { numChunksVertical, numChunksHorizontal };

            int[] numQuadsPerChunk = new int[2];
            for (int dimension = 0; dimension < 2; ++dimension)
            {
                // Maximize number of quads per chunk.
                numQuadsPerChunk[dimension] = (int)Math.Ceiling(numQuads[dimension] / numChunks[dimension]);

                // Trim excess chunks.
                numChunks[dimension] = Math.Ceiling(numQuads[dimension] / numQuadsPerChunk[dimension]);
            }

            mChunkHeight = numQuadsPerChunk[0] + 1;
            mChunkWidth  = numQuadsPerChunk[1] + 1;

            mNumChunksVertical   = (int)numChunks[0];
            mNumChunksHorizontal = (int)numChunks[1];

            mVertexBuffers = new VertexBuffer[mNumChunksVertical, mNumChunksHorizontal];
            mIndexBuffers  = new IndexBuffer [mNumChunksVertical, mNumChunksHorizontal];

            mDirtyChunks = new bool[mNumChunksVertical, mNumChunksHorizontal];

            mEdgeVertexBuffers = new VertexBuffer[4];
            mEdgeIndexBuffers = new IndexBuffer[4];

            mEdgeDirtyChunks = new bool[4];

            mChunkLookupTable = new List<Tuple<int, int>>[mHeight, mWidth];

            InitializeVertices(heightMap);

            UpdateNormalVectors(0, mHeight - 1, 0, mWidth - 1);

            InitializeBuffers();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// 
        /// </summary>
        /// <param name="heightMap"></param>
        private void InitializeVertices(Texture2D heightMap)
        {
            for (int zIndex = 0; zIndex < mHeight; ++zIndex)
            {
                for (int xIndex = 0; xIndex < mWidth; ++xIndex)
                {
                    float vertexHeight = RetrieveHeight(heightMap, xIndex, zIndex);

                    VertexPositionColorTextureNormal vertex = new VertexPositionColorTextureNormal();
                    vertex.Position = new Vector3(xIndex - mWidth / 2, vertexHeight, zIndex - mHeight / 2);
                    vertex.Color = Color.BurlyWood;
                    vertex.TexCoord = new Vector2((float)xIndex / (float)mWidth, (float)zIndex / (float)mHeight);
                    vertex.Normal = Vector3.Zero;

                    mVertices[xIndex + zIndex * mWidth] = vertex;

                    FillLookUpTable(xIndex, zIndex);
                }
            }

            int topSideBaseIndex = 0;
            int bottomSideBaseIndex = topSideBaseIndex + mWidth;

            // Add top and bottom sides.
            for (int xIndex = 0; xIndex < mWidth; ++xIndex)
            {
                VertexPositionColorTextureNormal vertex = new VertexPositionColorTextureNormal();
                vertex.Position = new Vector3(xIndex - mWidth / 2, 0.0f, 0.0f - mHeight / 2);
                vertex.Color = Color.BurlyWood;
                vertex.TexCoord = new Vector2((float)xIndex / (float)mWidth, 0.0f);
                vertex.Normal = Vector3.Zero;

                mEdgeVertices[topSideBaseIndex + xIndex] = vertex;

                vertex.Position.Z = mHeight - 1 - mHeight / 2;
                vertex.TexCoord.Y = 1.0f;

                mEdgeVertices[bottomSideBaseIndex + xIndex] = vertex;
            }

            int leftSideBaseIndex = bottomSideBaseIndex + mWidth;
            int rightSideBaseIndex = leftSideBaseIndex + mHeight;

            // Add left and right sides.
            for (int zIndex = 0; zIndex < mHeight; ++zIndex)
            {
                VertexPositionColorTextureNormal vertex = new VertexPositionColorTextureNormal();
                vertex.Position = new Vector3(0.0f - mWidth / 2, 0.0f, zIndex - mHeight / 2);
                vertex.Color = Color.BurlyWood;
                vertex.TexCoord = new Vector2(0.0f, (float)zIndex / (float)mHeight);
                vertex.Normal = Vector3.Zero;

                mEdgeVertices[leftSideBaseIndex + zIndex] = vertex;

                vertex.Position.X = mWidth - 1 - mWidth / 2;
                vertex.TexCoord.X = 1.0f;

                mEdgeVertices[rightSideBaseIndex + zIndex] = vertex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeBuffers()
        {
            for (int zIndex = 0; zIndex < mNumChunksVertical; ++zIndex)
            {
                for (int xIndex = 0; xIndex < mNumChunksHorizontal; ++xIndex)
                {
                    mDirtyChunks[zIndex, xIndex] = true;
                }
            }

            for (int index = 0; index < mEdgeDirtyChunks.Length; ++index)
            {
                mEdgeDirtyChunks[index] = true;
            }

            UpdateVertexBuffers();
            CreateIndexBuffers();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateIndexBuffers()
        {
            // Construct terrain plane.
            for (int verticalIndex = 0; verticalIndex < mNumChunksVertical; ++verticalIndex)
            {
                for (int horizontalIndex = 0; horizontalIndex < mNumChunksHorizontal; ++horizontalIndex)
                {
                    int counter = 0;
                    int[] chunkIndices = new int[6 * (mChunkHeight - 1) * (mChunkWidth - 1)];

                    for (int z = 0; z < mChunkHeight - 1; ++z)
                    {
                        for (int x = 0; x < mChunkWidth - 1; ++x)
                        {
                            int topLeftIndex = x + z * mChunkWidth;
                            int topRightIndex = (x + 1) + z * mChunkWidth;
                            int bottomRightIndex = (x + 1) + (z + 1) * mChunkWidth;
                            int bottomLeftIndex = x + (z + 1) * mChunkWidth;

                            // Make first triangle.
                            chunkIndices[counter++] = topLeftIndex;
                            chunkIndices[counter++] = topRightIndex;
                            chunkIndices[counter++] = bottomRightIndex;

                            // Make second triangle.
                            chunkIndices[counter++] = topLeftIndex;
                            chunkIndices[counter++] = bottomRightIndex;
                            chunkIndices[counter++] = bottomLeftIndex;
                        }
                    }

                    mIndexBuffers[verticalIndex, horizontalIndex] = new IndexBuffer(mDevice, typeof(int), chunkIndices.Length, BufferUsage.WriteOnly);
                    mIndexBuffers[verticalIndex, horizontalIndex].SetData(chunkIndices);
                }
            }

            // Construct edge chunks.
            int sideHeight = 2;
            int[] sideWidths = { mWidth, mWidth, mHeight, mHeight };

            for (int side = 0; side < 4; ++side)
            {
                int counter = 0;
                int[] chunkIndices = new int[6 * (sideWidths[side] - 1)];

                for (int z = 0; z < sideHeight - 1; ++z)
                {
                    for (int x = 0; x < sideWidths[side] - 1; ++x)
                    {
                        int topLeftIndex = x + z * sideWidths[side];
                        int topRightIndex = (x + 1) + z * sideWidths[side];
                        int bottomRightIndex = (x + 1) + (z + 1) * sideWidths[side];
                        int bottomLeftIndex = x + (z + 1) * sideWidths[side];

                        switch (side)
                        {
                            case 1:
                            case 2:
                            {
                                // Make first triangle.
                                chunkIndices[counter++] = topLeftIndex;
                                chunkIndices[counter++] = topRightIndex;
                                chunkIndices[counter++] = bottomRightIndex;

                                // Make second triangle.
                                chunkIndices[counter++] = topLeftIndex;
                                chunkIndices[counter++] = bottomRightIndex;
                                chunkIndices[counter++] = bottomLeftIndex;

                                break;
                            }
                            case 0:
                            case 3:
                            {
                                // Make first triangle.
                                chunkIndices[counter++] = topLeftIndex;
                                chunkIndices[counter++] = bottomRightIndex;
                                chunkIndices[counter++] = topRightIndex;

                                // Make second triangle.
                                chunkIndices[counter++] = topLeftIndex;
                                chunkIndices[counter++] = bottomLeftIndex;
                                chunkIndices[counter++] = bottomRightIndex;

                                break;
                            }
                        }
                    }
                }

                mEdgeIndexBuffers[side] = new IndexBuffer(mDevice, typeof(int), chunkIndices.Length, BufferUsage.WriteOnly);
                mEdgeIndexBuffers[side].SetData(chunkIndices);
            }
        }

        private void FillLookUpTable(int xIndex, int zIndex)
        {
            mChunkLookupTable[zIndex, xIndex] = new List<Tuple<int, int>>();

            int chunkQuadWidth = mChunkWidth - 1;
            int chunkQuadHeight = mChunkHeight - 1;

            for (int zExtent = zIndex - 1; zExtent <= zIndex; ++zExtent)
            {
                for (int xExtent = xIndex - 1; xExtent <= xIndex; ++xExtent)
                {
                    if (zExtent > 0 && zExtent < mHeight - 1 && xExtent > 0 && xExtent < mWidth - 1)
                    {
                        Tuple<int, int> chunk = new Tuple<int, int>(zExtent / chunkQuadHeight, xExtent / chunkQuadWidth);

                        if (!mChunkLookupTable[zIndex, xIndex].Contains(chunk))
                        {
                            mChunkLookupTable[zIndex, xIndex].Add(chunk);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Copies vertices from total collection in to a chunk.
        /// </summary>
        /// <param name="verticalIndex">Y index in to chunk array.</param>
        /// <param name="horizontalIndex">X index in to chunk array.</param>
        private VertexPositionColorTextureNormal[] ConstructChunk(int verticalIndex, int horizontalIndex)
        {
            int chunkHeight = mChunkHeight;
            int remainingHeight = mHeight - verticalIndex * (mChunkHeight - 1);
            if (remainingHeight < mChunkHeight)
            {
                chunkHeight = remainingHeight;
            }

            int chunkWidth = mChunkWidth;
            int remainingWidth  = mWidth  - horizontalIndex * (mChunkWidth  - 1);
            if (remainingWidth < mChunkWidth)
            {
                chunkWidth = remainingWidth;
            }

            VertexPositionColorTextureNormal[] result = new VertexPositionColorTextureNormal[chunkHeight * chunkWidth];

            for (int zIndex = 0; zIndex < chunkHeight; ++zIndex)
            {
                for (int xIndex = 0; xIndex < chunkWidth; ++xIndex)
                {
                    int z = verticalIndex   * (mChunkHeight - 1) + zIndex;
                    int x = horizontalIndex * (mChunkWidth  - 1) + xIndex;

                    result[xIndex + zIndex * chunkWidth] = mVertices[x + z * mWidth];
                    result[xIndex + zIndex * chunkWidth].TexCoord = new Vector2((float)xIndex / (float)(chunkWidth - 1), (float)zIndex / (float)(chunkHeight - 1));
                }
            }

            return result;
        }

        private VertexPositionColorTextureNormal[] ConstructEdgeChunk(int edgeIndex)
        {
            int edgeHeight = 2;
            int[] edgeLengths = { mWidth, mWidth, mHeight, mHeight };

            VertexPositionColorTextureNormal[] result = new VertexPositionColorTextureNormal[edgeLengths[edgeIndex] * edgeHeight];

            switch (edgeIndex)
            {
                // Top Edge or Bottom Edge.
                case 0:
                case 1:
                {
                    int verticesOffset = edgeIndex == 0 ? 0 : mWidth * (mHeight - 1);
                    int edgeVerticesOffset = edgeIndex == 0 ? 0 : mWidth;
                    Vector3 normal = edgeIndex == 0 ? new Vector3(0.0f, 0.0f, -1.0f) : new Vector3(0.0f, 0.0f, 1.0f);

                    for (int x = 0; x < mWidth; ++x)
                    {
                        result[x] = mVertices[x + verticesOffset];
                        result[x].Normal = normal;

                        result[x + mWidth] = mEdgeVertices[x + edgeVerticesOffset];
                        result[x + mWidth].Normal = normal;
                    }
                    break;
                }

                // Left Edge or Right Edge.
                case 2:
                case 3:
                {
                    int verticesOffset = edgeIndex == 2 ? 0 : mWidth - 1;
                    int edgeVerticesOffset = edgeIndex == 2 ? 2 * mWidth : 2 * mWidth + mHeight;
                    Vector3 normal = edgeIndex == 2 ? new Vector3(-1.0f, 0.0f, 0.0f) : new Vector3(1.0f, 0.0f, 0.0f);

                    for (int z = 0; z < mHeight; ++z)
                    {
                        result[z] = mVertices[z * mWidth + verticesOffset];
                        result[z].Normal = normal;

                        result[z + mHeight] = mEdgeVertices[z + edgeVerticesOffset];
                        result[z + mHeight].Normal = normal;

                        if (result[z].Position == Vector3.Zero || result[z + mHeight].Position == Vector3.Zero)
                        {
                            Console.WriteLine();
                        }
                    }
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="heightMap"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private float RetrieveHeight(Texture2D heightMap, int u, int v)
        {
            Color pixelValue = Utils.GetTexture2DPixelColor(u, v, heightMap);
            return pixelValue.R * 256 * 256 + pixelValue.G * 256 + pixelValue.B;
        }

        #endregion

        #region HeightMap Modification

        /// <summary>
        /// Raises the terrain in the radius around position by intensity meters.
        /// </summary>
        /// <param name="position">HeightMap coordinate at which height change originated.</param>
        /// <param name="radius">Radius around position in which to raise the terrain.</param>
        /// <param name="intensity">Amount by which to raise the terrain.</param>
        public void RaiseTerrain(Vector2 position, float radius, float deltaHeight)
        {
            VertexModifier modifier = RaiseLowerVertex;
            ModifyVertices(position, radius, deltaHeight, 1, modifier);
        }

        /// <summary>
        /// Lowers the terrain in the radius around position by intensity meters.
        /// </summary>
        /// <param name="position">HeightMap coordinate at which height change originated.</param>
        /// <param name="radius">Radius around position in which to lower the terrain.</param>
        /// <param name="intensity">Amount by which to lower the terrain.</param>
        public void LowerTerrain(Vector2 position, float radius, float deltaHeight)
        {
            VertexModifier modifier = RaiseLowerVertex;
            ModifyVertices(position, radius, -deltaHeight, 1, modifier);
        }

        /// <summary>
        /// Sets the height of the terrain in the radius around position to intensity meters.
        /// </summary>
        /// <param name="position">HeightMap coordinate at which height change originated.</param>
        /// <param name="radius">Radius around position in which to set the terrain.</param>
        /// <param name="intensity">New height of the terrain.</param>
        public void SetTerrain(Vector2 position, float radius, float height)
        {
            VertexModifier modifier = SetVertex;
            ModifyVertices(position, radius, height, 1, modifier);
        }

        /// <summary>
        /// Sets height of terrain in the radius around position to the average existing height.
        /// </summary>
        /// <param name="position">HeightMap coordinate at which height change originated.</param>
        /// <param name="radius">Radius around position in which to flatten the terrain.</param>
        public void FlattenTerrain(Vector2 position, float radius)
        {
            mFlattenHeightSum = 0.0f;
            mFlattenNumVertices = 0;

            VertexModifier modifier = FlattenVertex;
            ModifyVertices(position, radius, 0.0f, 2, modifier);
        }

        /// <summary>
        /// Smoothes the terrain in the radius around position by averaging neighboring heights.
        /// </summary>
        /// <param name="position">HeightMap coordinate at which height change originated.</param>
        /// <param name="radius">Radius around position in which to smooth the terrain.</param>
        public void SmoothTerrain(Vector2 position, float radius)
        {
            mSmoothHeightAverages = new float[(int)radius * 2 + 1, (int)radius * 2 + 1];
            mSmoothZBaseIndex = (int)(position.Y / Utils.WorldScale.Z + mHeight / 2) - (int)radius;
            mSmoothXBaseIndex = (int)(position.X / Utils.WorldScale.X + mWidth / 2) - (int)radius;

            VertexModifier modifier = SmoothVertex;
            ModifyVertices(position, radius, 0.0f, 2, modifier);
        }

        #endregion

        #region Heightmap Modification Helpers

        private delegate void VertexModifier(int x, int z, float magnitude, int pass);

        /// <summary>
        /// Increase or decrease the Y coordinate of the vertex at X, Z by magnitude meters.
        /// </summary>
        /// <param name="x">Horizontal vertex coordinate.</param>
        /// <param name="z">Vertical vertex coordinate.</param>
        /// <param name="magnitude">Amount to raise or lower the vertex at X, Z in meters.</param>
        /// <param name="pass">Unused.</param>
        private void RaiseLowerVertex(int x, int z, float magnitude, int pass)
        {
            float height = mVertices[x + z * mWidth].Position.Y + magnitude;
            height = Math.Max(0, Math.Min(height, (256.0f * 256.0f * 256.0f - 1.0f)));

            mVertices[x + z * mWidth].Position.Y = height;
        }

        /// <summary>
        /// Set height of the vertex at X, Z to be the average of the neighboring heights.
        /// </summary>
        /// <param name="x">Horizontal vertex coordinate.</param>
        /// <param name="z">Vertical vertex coordinate.</param>
        /// <param name="magnitude">Unused.</param>
        /// <param name="pass">Current pass over vertices.  First pass finds average.  Second pass sets height.</param>
        private void SmoothVertex(int x, int z, float magnitude, int pass)
        {
            int zSmoothIndex = z - mSmoothZBaseIndex;
            int xSmoothIndex = x - mSmoothXBaseIndex;

            switch(pass)
            {
                case 0:
                {
                    float heightSum = 0.0f;
                    int neighborCount = 0;
                    for (int zIndex = (int)Math.Max(0, z - 1); zIndex <= (int)Math.Min(mHeight - 1, z + 1); ++zIndex)
                    {
                        for (int xIndex = (int)Math.Max(0, x - 1); xIndex <= (int)Math.Min(mWidth - 1, x + 1); ++xIndex)
                        {
                            ++neighborCount;
                            heightSum += mVertices[xIndex + zIndex * mWidth].Position.Y;
                        }
                    }
                    mSmoothHeightAverages[zSmoothIndex, xSmoothIndex] = heightSum / neighborCount;
                    break;
                }

                case 1:
                {
                    float height = (mVertices[x + z * mWidth].Position.Y + mSmoothHeightAverages[zSmoothIndex, xSmoothIndex]) / 2;
                    mVertices[x + z * mWidth].Position.Y = height;
                    break;
                }
            }
        }

        /// <summary>
        /// Set the Y coordinate of the vertex at X, Z to magnitude meters.
        /// </summary>
        /// <param name="x">Horizontal vertex coordinate.</param>
        /// <param name="z">Vertical vertex coordinate.</param>
        /// <param name="magnitude">Height to set the vertex at X, Z to in meters.</param>
        /// <param name="pass">Unused.</param>
        private void SetVertex(int x, int z, float magnitude, int pass)
        {
            mVertices[x + z * mWidth].Position.Y = Math.Max(0, Math.Min(magnitude, (256.0f * 256.0f * 256.0f - 1.0f))); ;
        }

        /// <summary>
        /// Set height of the vertex at X, Z to be the average height of all vertices in a set radius.
        /// </summary>
        /// <param name="x">Horizontal vertex coordinate.</param>
        /// <param name="z">Vertical vertex coordinate.</param>
        /// <param name="magnitude">Unused.</param>
        /// <param name="pass">Current pass over vertices.  First pass finds average.  Second pass sets height.</param>
        private void FlattenVertex(int x, int z, float magnitude, int pass)
        {
            switch (pass)
            {
                case 0:
                {
                    ++mFlattenNumVertices;
                    mFlattenHeightSum += mVertices[x + z * mWidth].Position.Y;
                    break;
                }

                case 1:
                {
                    mVertices[x + z * mWidth].Position.Y = mFlattenHeightSum / mFlattenNumVertices;
                    break;
                }
            }
        }

        /// <summary>
        /// Marks chunk as dirty.  Dirty chunks have their VertexBuffer updated this frame.
        /// </summary>
        /// <param name="x">Horizontal coordinate of dirty vertex.</param>
        /// <param name="z">Vertical coordinate of dirty vertex.</param>
        private void WriteDirtyBit(int x, int z)
        {
            foreach (Tuple<int, int> chunk in mChunkLookupTable[z, x])
            {
                mDirtyChunks[chunk.Item1, chunk.Item2] = true;
            }

            if (z == 0)
            {
                mEdgeDirtyChunks[0] = true;
            }

            if (z == mHeight - 1)
            {
                mEdgeDirtyChunks[1] = true;
            }

            if (x == 0)
            {
                mEdgeDirtyChunks[2] = true;
            }

            if (x == mWidth - 1)
            {
                mEdgeDirtyChunks[3] = true;
            }
        }

        /// <summary>
        /// Iterates over the vertices in the height map and modifies them according to the provided modifier.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <param name="magnitude"></param>
        /// <param name="modifyVertex"></param>
        private void ModifyVertices(Vector2 position, float radius, float magnitude, int numPasses, VertexModifier modifyVertex)
        {
            float radiusSquared = radius * radius;

            // Normalize origin of modification to [0,mHeight), [0, mWidth).
            position.X = position.X / Utils.WorldScale.X + mWidth / 2;
            position.Y = position.Y / Utils.WorldScale.Z + mHeight / 2;

            int minZ = (int)Math.Max(0, position.Y - radius), maxZ = (int)Math.Min(mHeight - 1, position.Y + radius);
            int minX = (int)Math.Max(0, position.X - radius), maxX = (int)Math.Min(mWidth - 1, position.X + radius);

            for (int pass = 0; pass < numPasses; ++pass)
            {
                for (int zIndex = minZ; zIndex <= maxZ; ++zIndex)
                {
                    for (int xIndex = minX; xIndex <= maxX; ++xIndex)
                    {
                        Vector2 displacement = new Vector2(xIndex, zIndex) - position;

                        if (mIsBlock || displacement.LengthSquared() < radiusSquared)
                        {
                            float featherScale = mIsFeathered ? (float)Math.Log(radius - displacement.Length()) / radius : 1.0f;
                            modifyVertex(xIndex, zIndex, magnitude * featherScale, pass);

                            WriteDirtyBit(xIndex, zIndex);
                        }
                    }
                }
            }

            UpdateNormalVectors(minZ, maxZ, minX, maxX);
            UpdateVertexBuffers();
        }

        /// <summary>
        /// Updates VertexBuffers so that the modified terrain is correctly rendered.
        /// </summary>
        private void UpdateVertexBuffers()
        {
            for (int chunkVerticalIndex = 0; chunkVerticalIndex < mNumChunksVertical; ++chunkVerticalIndex)
            {
                for (int chunkHorizontalIndex = 0; chunkHorizontalIndex < mNumChunksHorizontal; ++chunkHorizontalIndex)
                {
                    if (mDirtyChunks[chunkVerticalIndex, chunkHorizontalIndex] == true)
                    {
                        VertexPositionColorTextureNormal[] vertices = ConstructChunk(chunkVerticalIndex, chunkHorizontalIndex);

                        if (mVertexBuffers[chunkVerticalIndex, chunkHorizontalIndex] == null)
                        {
                            mVertexBuffers[chunkVerticalIndex, chunkHorizontalIndex] = new VertexBuffer(mDevice, VertexPositionColorTextureNormal.VertexDeclaration, vertices.Length, BufferUsage.None);
                        }
                        mVertexBuffers[chunkVerticalIndex, chunkHorizontalIndex].SetData(vertices);

                        mDirtyChunks[chunkVerticalIndex, chunkHorizontalIndex] = false;
                    }
                }
            }

            for (int edgeChunkIndex = 0; edgeChunkIndex < mEdgeDirtyChunks.Length; ++edgeChunkIndex)
            {
                if (mEdgeDirtyChunks[edgeChunkIndex] == true)
                {
                    VertexPositionColorTextureNormal[] vertices = ConstructEdgeChunk(edgeChunkIndex);

                    if (mEdgeVertexBuffers[edgeChunkIndex] == null)
                    {
                        mEdgeVertexBuffers[edgeChunkIndex] = new VertexBuffer(mDevice, VertexPositionColorTextureNormal.VertexDeclaration, vertices.Length, BufferUsage.None);
                    }
                    mEdgeVertexBuffers[edgeChunkIndex].SetData(vertices);

                    mEdgeDirtyChunks[edgeChunkIndex] = false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minZ"></param>
        /// <param name="maxZ"></param>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        private void UpdateNormalVectors(int minZ, int maxZ, int minX, int maxX)
        {
            for (int zIndex = minZ; zIndex <= maxZ; ++zIndex)
            {
                for (int xIndex = minX; xIndex <= maxX; ++xIndex)
                {
                    for (int faceIndex = 0; faceIndex < 6; ++faceIndex)
                    {
                        if (xIndex > 0 && xIndex < mWidth - 1 && zIndex > 0 && zIndex < mHeight - 1)
                        {
                            int index0 = xIndex + (int)mVertex0Offset[faceIndex].X + (zIndex + (int)mVertex0Offset[faceIndex].Y) * mWidth;
                            int index1 = xIndex + (int)mVertex1Offset[faceIndex].X + (zIndex + (int)mVertex1Offset[faceIndex].Y) * mWidth;
                            int index2 = xIndex + (int)mVertex2Offset[faceIndex].X + (zIndex + (int)mVertex2Offset[faceIndex].Y) * mWidth;

                            Vector3 edge0 = mVertices[index0].Position - mVertices[index1].Position;
                            Vector3 edge1 = mVertices[index0].Position - mVertices[index2].Position;
                            Vector3 normal = Vector3.Cross(edge1, edge0);

                            mVertices[xIndex + zIndex * mWidth].Normal = Vector3.Normalize(mVertices[xIndex + zIndex * mWidth].Normal + normal);
                        }
                    }
                }
            }
        }

        #endregion

        #region Saving

        /// <summary>
        /// Create bitmap containing heightmap data.
        /// </summary>
        /// <param name="filepath"></param>
        public MemoryStream ExportToStream()
        {
            Texture2D heightMapTexture = new Texture2D(mDevice, mWidth, mHeight);
            Color[] heightTexels = new Color[mWidth * mHeight];

            for (int row = 0; row < mHeight; ++row)
            {
                for (int col = 0; col < mWidth; ++col)
                {
                    heightTexels[col + row * mWidth] = HeightAsColor((int)mVertices[col + row * mWidth].Position.Y);
                }
            }
            
            heightMapTexture.SetData(heightTexels);

            MemoryStream ms = new MemoryStream();
            heightMapTexture.SaveAsPng(ms, mWidth, mHeight);

            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }

        private Color HeightAsColor(int height)
        {
            int blue = height % 256;
            int green = ((height - blue) / 256) % 256;
            int red = ((height - (blue + green)) / (256 * 256) % 256);

            return new Color(red, green, blue);
        }

        // Used for creating a collision entity
        public Single[,] GetHeights()
        {
            Single[,] heights = new Single[mWidth, mHeight];
            for (int z = 0; z < mHeight; z++)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    heights[x, z] = (Single)mVertices[x + z * mWidth].Position.Y;
                }
            }

            return heights;
        }

        #endregion
    }
}