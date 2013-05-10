using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GraphicsLibrary
{
    public class HeightMapMesh
    {
        #region Constants

        public const int NUM_SIDE_VERTICES = 10;

        public const int NUM_SIDE_TEXELS_PER_QUAD = 10;

        private Vector2[] VERTEX_0_OFFSET = new Vector2[]
        {
            new Vector2(-1, -1),
            new Vector2(-1, -1),
            new Vector2(0, -1),
            new Vector2(-1, 0),
            new Vector2(0, 0),
            new Vector2(0, 0)
        };

        private Vector2[] VERTEX_1_OFFSET = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(0, -1),
            new Vector2(1, 0),
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(1, 0)
        };

        private Vector2[] VERTEX_2_OFFSET = new Vector2[]
        {
            new Vector2(-1, 0),
            new Vector2(0, 0),
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };

        #endregion

        #region Vertices

        public VertexBuffer VertexBuffer
        {
            get 
            {
                if (mDirtyVertices)
                {
                    UpdateVertexBuffer();
                    mDirtyVertices = false;
                }
                return mVertexBuffer;
            }
        }
        private VertexBuffer                  mVertexBuffer = null;
        private VertexPositionNormalTexture[] mVertices;

        private bool mDirtyVertices = true;

        public float[,] Heights { get { return mHeights; } }
        private float[,] mHeights = null;

        #endregion

        #region Indices

        public IndexBuffer IndexBuffer
        {
            get { return mIndexBuffer; }
        }
        static private IndexBuffer mIndexBuffer = null;
        static private int[] mIndices = null;

        #endregion

        #region Texture

        public Texture2D AlphaMap
        {
            get
            {
                if (mDirtyTexture)
                {
                    UpdateTextureBuffers();
                    mDirtyTexture = false;
                }
                return mAlphaMap;
            }
        }
        private Texture2D mAlphaMap = null;

        private Color[] mTexels = null;

        public string[] DetailTextureNames
        {
            get { return mDetailTextureNames; }
        }
        private string[] mDetailTextureNames;

        public Vector2[] DetailTextureUVOffset { get { return mDetailTextureUVOffset; } }
        private Vector2[] mDetailTextureUVOffset;

        public Vector2[] DetailTextureUVScale { get { return mDetailTextureUVScale; } }
        private Vector2[] mDetailTextureUVScale;

        private bool mDirtyTexture = true;

        #endregion

        #region Enums

        public enum TextureLayer { BACKGROUND, RED, GREEN, BLUE, ALPHA };

        #endregion

        #region Terrain Modification Variables

        private float mFlattenHeightSum = 0.0f;
        private int mFlattenNumVertices = 0;

        private float[,] mSmoothHeightAverages;
        private int mSmoothXBaseIndex = 0;
        private int mSmoothZBaseIndex = 0;

        #endregion

        #region Public Interface

        public HeightMapMesh(float[,] heights, Texture2D alphaMap, string[] detailTextureNames, Vector2[] uvOffsets, Vector2[] uvScales)
        {
            mHeights = heights;

            InitializeVertices(heights);

            if (mIndexBuffer == null)
            {
                InitializeIndices();
            }

            mAlphaMap              = alphaMap;
            mDetailTextureNames    = detailTextureNames;
            mDetailTextureUVOffset = uvOffsets;
            mDetailTextureUVScale  = uvScales;

            if (mAlphaMap != null)
            {
                InitializeTexture();
            }
        }

        public void SetVertexHeight(Vector2 index, float newHeight)
        {
            mHeights[(int)index.X, (int)index.Y] = newHeight;
            mVertices[(int)index.X + (int)index.Y * NUM_SIDE_VERTICES].Position.Y = newHeight;
            mDirtyVertices = true;
        }

        public float GetVertexHeight(Vector2 index)
        {
            return mHeights[(int)index.X, (int)index.Y];
        }

        public void SetVertexNormal(Vector2 index, Vector3 normal)
        {
            mVertices[(int)index.X + (int)index.Y * NUM_SIDE_VERTICES].Normal = normal;
        }

        public Vector3 GetVertexNormal(Vector2 index)
        {
            return mVertices[(int)index.X + (int)index.Y * NUM_SIDE_VERTICES].Normal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="minZ"></param>
        /// <param name="maxZ"></param>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        public void UpdateNormalVectors()
        {
            for (int i = 0; i < mVertices.Length; i++)
            {
                mVertices[i].Normal = Vector3.Zero;
            }

            for (int index = 0; index < mIndices.Length; index += 3)
            {
                int[] polyIndices = new int[3] { mIndices[index], mIndices[index + 1], mIndices[index + 2] };
                Vector3[] positions = new Vector3[3] { mVertices[polyIndices[0]].Position, mVertices[polyIndices[1]].Position, mVertices[polyIndices[2]].Position };
                Vector3[] edges = new Vector3[2] { positions[1] - positions[0], positions[2] - positions[0] };

                Vector3 normal = Vector3.Normalize(Vector3.Cross(edges[1], edges[0]));

                foreach (int polyIndex in polyIndices)
                {
                    mVertices[polyIndex].Normal += normal;
                }
            }

            for (int i = 0; i < mVertices.Length; i++)
            {
                Vector3.Normalize(mVertices[i].Normal);
            }
        }

        #endregion

        #region Initialization

        private void InitializeVertices(float[,] heights)
        {
            mVertices = new VertexPositionNormalTexture[NUM_SIDE_VERTICES * NUM_SIDE_VERTICES + 4];
            QuadIterator(VertexConstructor, null);

            InitializeBottomFaceVertices();
        }

        private void InitializeIndices()
        {
            List<int> indices = new List<int>();

            QuadIterator(IndexConstructor, new object[] { indices });

            InitializeBottomFaceIndices(indices);

            mIndices = indices.ToArray();

            mIndexBuffer = new IndexBuffer(GraphicsManager.Device, typeof(int), indices.Count, BufferUsage.WriteOnly);
            mIndexBuffer.SetData(mIndices);
        }

        private void InitializeTexture()
        {
            mTexels = new Color[mAlphaMap.Width * mAlphaMap.Height];
            mAlphaMap.GetData(mTexels);

            for (int i = 0; i < mTexels.Length; ++i)
            {
                if (mTexels[i].A == 255)
                {
                    mTexels[i].A = 0;
                }
                else
                {
                    mTexels[i].A = (byte)((float)mTexels[i].A / 254.0f * 255.0f);
                }
            }

            mAlphaMap = new Texture2D(GraphicsManager.Device, mAlphaMap.Width, mAlphaMap.Height);
            mAlphaMap.SetData(mTexels);
        }

        private void UpdateVertexBuffer()
        {
            mDirtyVertices = false;

            if (mVertexBuffer == null)
            {
                mVertexBuffer = new VertexBuffer(GraphicsManager.Device, VertexPositionNormalTexture.VertexDeclaration, mVertices.Length, BufferUsage.None);
            }

            mVertexBuffer.SetData(mVertices);
        }

        private void InitializeBottomFaceVertices()
        {
            int[] cornerIndices = new int[4] { 0, NUM_SIDE_VERTICES - 1, (NUM_SIDE_VERTICES - 1) * NUM_SIDE_VERTICES, NUM_SIDE_VERTICES * NUM_SIDE_VERTICES - 1 };
            int counter = NUM_SIDE_VERTICES * NUM_SIDE_VERTICES;
            for (int i = 0; i < 4; i++)
            {
                VertexPositionNormalTexture vertex = new VertexPositionNormalTexture();
                vertex.Position = mVertices[cornerIndices[i]].Position;
                vertex.Position.Y = 0;

                vertex.Normal = Vector3.Down;
                vertex.TextureCoordinate = mVertices[cornerIndices[i]].TextureCoordinate;

                mVertices[counter++] = vertex;
            }
        }

        private void InitializeBottomFaceIndices(List<int> indices)
        {
            int topLeftIndex = NUM_SIDE_VERTICES * NUM_SIDE_VERTICES;
            int topRightIndex = topLeftIndex + 1;
            int botLeftIndex = topRightIndex + 1;
            int botRightIndex = botLeftIndex + 1;

            indices.Add(botRightIndex);
            indices.Add(topRightIndex);
            indices.Add(topLeftIndex);

            indices.Add(botRightIndex);
            indices.Add(topLeftIndex);
            indices.Add(botLeftIndex);
        }

        #endregion

        #region Quad Iterator / Modifiers

        private void QuadIterator(QuadModifier modifier, object[] optionalParameters)
        {
            for (int vertVertex = 0; vertVertex < NUM_SIDE_VERTICES; vertVertex++)
            {
                for (int horizVertex = 0; horizVertex < NUM_SIDE_VERTICES; horizVertex++)
                {
                    modifier(vertVertex, horizVertex, optionalParameters);
                }
            }
        }

        private delegate void QuadModifier(int vertVertex, int horizVertex, object[] optionalParameters);

        private void VertexConstructor(int vertVertex, int horizVertex, object[] optionalParameters)
        {
            VertexPositionNormalTexture vertex = new VertexPositionNormalTexture();
            vertex.TextureCoordinate = new Vector2((float)horizVertex / (float)(NUM_SIDE_VERTICES - 1), (float)vertVertex / (float)(NUM_SIDE_VERTICES - 1));
            vertex.Position = new Vector3((float)horizVertex / (float)(NUM_SIDE_VERTICES - 1), mHeights[horizVertex, vertVertex], (float)vertVertex / (float)(NUM_SIDE_VERTICES - 1));
            vertex.Normal            = Vector3.Up;

            mVertices[horizVertex + vertVertex * NUM_SIDE_VERTICES] = vertex;
        }

        private void IndexConstructor(int vertVertex, int horizVertex, object[] optionalParameters)
        {
            if (vertVertex >= NUM_SIDE_VERTICES - 1 || horizVertex >= NUM_SIDE_VERTICES - 1)
            {
                return;
            }

            List<int> indices = optionalParameters[0] as List<int>;

            int topLeftIndex = horizVertex + vertVertex * NUM_SIDE_VERTICES;
            int topRightIndex = (horizVertex + 1) + vertVertex * NUM_SIDE_VERTICES;
            int botLeftIndex = horizVertex + (vertVertex + 1) * NUM_SIDE_VERTICES;
            int botRightIndex = (horizVertex + 1) + (vertVertex + 1) * NUM_SIDE_VERTICES;

            indices.Add(botLeftIndex);
            indices.Add(topLeftIndex);
            indices.Add(topRightIndex);

            indices.Add(botLeftIndex);
            indices.Add(topRightIndex);
            indices.Add(botRightIndex);
        }

        #endregion

        //#region HeightMap Modification

        ///// <summary>
        ///// Sets height of terrain in the radius around position to the average existing height.
        ///// </summary>
        ///// <param name="position">HeightMap coordinate at which height change originated.</param>
        ///// <param name="radius">Radius around position in which to flatten the terrain.</param>
        //public void FlattenTerrain(Vector3 position, float radius, bool isFeathered, bool isBlock)
        //{
        //    mFlattenHeightSum = 0.0f;
        //    mFlattenNumVertices = 0;

        //    VertexModifier modifier = FlattenVertex;
        //    ModifyVertices(position, radius, 0.0f, 2, modifier, isFeathered, isBlock);
        //}

        ///// <summary>
        ///// Smoothes the terrain in the radius around position by averaging neighboring heights.
        ///// </summary>
        ///// <param name="position">HeightMap coordinate at which height change originated.</param>
        ///// <param name="radius">Radius around position in which to smooth the terrain.</param>
        //public void SmoothTerrain(Vector3 position, float radius, bool isFeathered, bool isBlock)
        //{
        //    mSmoothHeightAverages = new float[(int)radius * 2 + 1, (int)radius * 2 + 1];
        //    //mSmoothZBaseIndex = (int)(position.Y / Utils.WorldScale.Z + mHeight / 2) - (int)radius;
        //    //mSmoothXBaseIndex = (int)(position.X / Utils.WorldScale.X + mWidth / 2) - (int)radius;

        //    VertexModifier modifier = SmoothVertex;
        //    ModifyVertices(position, radius, 0.0f, 2, modifier, isFeathered, isBlock);
        //}

        //#endregion

        ///// <summary>
        ///// Set height of the vertex at X, Z to be the average of the neighboring heights.
        ///// </summary>
        ///// <param name="x">Horizontal vertex coordinate.</param>
        ///// <param name="z">Vertical vertex coordinate.</param>
        ///// <param name="magnitude">Unused.</param>
        ///// <param name="pass">Current pass over vertices.  First pass finds average.  Second pass sets height.</param>
        //private void SmoothVertex(int x, int z, float magnitude, int pass)
        //{
        //    int zSmoothIndex = z - mSmoothZBaseIndex;
        //    int xSmoothIndex = x - mSmoothXBaseIndex;

        //    switch (pass)
        //    {
        //        case 0:
        //            {
        //                float heightSum = 0.0f;
        //                int neighborCount = 0;
        //                for (int zIndex = (int)Math.Max(0, z - 1); zIndex <= (int)Math.Min(NUM_VERT_VERTICES - 1, z + 1); ++zIndex)
        //                {
        //                    for (int xIndex = (int)Math.Max(0, x - 1); xIndex <= (int)Math.Min(NUM_HORIZ_VERTICES - 1, x + 1); ++xIndex)
        //                    {
        //                        ++neighborCount;
        //                        heightSum += mVertices[xIndex + zIndex * NUM_HORIZ_VERTICES].Position.Y;
        //                    }
        //                }
        //                mSmoothHeightAverages[zSmoothIndex, xSmoothIndex] = heightSum / neighborCount;
        //                break;
        //            }

        //        case 1:
        //            {
        //                mHeights[x, z] = (mVertices[x + z * NUM_HORIZ_VERTICES].Position.Y + mSmoothHeightAverages[zSmoothIndex, xSmoothIndex]) / 2;
        //                mVertices[x + z * NUM_HORIZ_VERTICES].Position.Y = mHeights[x, z];
        //                break;
        //            }
        //    }
        //}

        ///// <summary>
        ///// Set height of the vertex at X, Z to be the average height of all vertices in a set radius.
        ///// </summary>
        ///// <param name="x">Horizontal vertex coordinate.</param>
        ///// <param name="z">Vertical vertex coordinate.</param>
        ///// <param name="magnitude">Unused.</param>
        ///// <param name="pass">Current pass over vertices.  First pass finds average.  Second pass sets height.</param>
        //private void FlattenVertex(int x, int z, float magnitude, int pass)
        //{
        //    switch (pass)
        //    {
        //        case 0:
        //            {
        //                ++mFlattenNumVertices;
        //                mFlattenHeightSum += mVertices[x + z * NUM_HORIZ_VERTICES].Position.Y;
        //                break;
        //            }

        //        case 1:
        //            {
        //                mHeights[x, z] = mFlattenHeightSum / mFlattenNumVertices;
        //                mVertices[x + z * NUM_HORIZ_VERTICES].Position.Y = mHeights[x, z];
        //                break;
        //            }
        //    }
        //}

        #region Texel Modification

        public void PaintTerrain(Vector3 position, float radius, float alpha, TextureLayer layer, string detailTextureName, Vector2 uVOffset, Vector2 uVScale, bool isFeathered, bool isBlock)
        {
            if (!isFeathered)
            {
                PaintSolidBrush(position, radius, alpha, layer, detailTextureName, uVOffset, uVScale, isFeathered, isBlock);
            }
            else
            {
                PaintLinearBrush(position, radius, alpha, layer, detailTextureName, uVOffset, uVScale, isFeathered, isBlock);
            }
        }

        public void EraseTerrain(Vector3 position, float radius, float alpha, TextureLayer layer, string detailTextureName, Vector2 uVOffset, Vector2 uVScale, bool isFeathered, bool isBlock)
        {
            if (!isFeathered)
            {
                EraseSolidBrush(position, radius, alpha, layer, detailTextureName, uVOffset, uVScale, isFeathered, isBlock);
            }
            else
            {
                EraseLinearBrush(position, radius, alpha, layer, detailTextureName, uVOffset, uVScale, isFeathered, isBlock);
            }
        }

        public void SmoothPaint(Vector3 position, float radius, bool isFeathered, bool isBlock)
        {
            SmoothBrush(position, radius, isFeathered, isBlock);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <param name="layer"></param>
        /// <param name="detailTextureName"></param>
        public void PaintSolidBrush(Vector3 position, float radius, float alpha, TextureLayer layer, string detailTextureName, Vector2 uVSOffset, Vector2 uVScale, bool isFeathered, bool isBlock)
        {
            Brush solidModifier = SolidBrush;
            ModifyTexels(position, radius, alpha, layer, detailTextureName, uVSOffset, uVScale, solidModifier, isFeathered, isBlock);
        }

        public void PaintLinearBrush(Vector3 position, float radius, float alpha, TextureLayer layer, string detailTextureName, Vector2 uVSOffset, Vector2 uVScale, bool isFeathered, bool isBlock)
        {
            Brush lerpModifier = LinearBrush;
            ModifyTexels(position, radius, alpha, layer, detailTextureName, uVSOffset, uVScale, lerpModifier, isFeathered, isBlock);
        }

        public void PaintQuadraticBrush(Vector3 position, float radius, float alpha, TextureLayer layer, string detailTextureName, Vector2 uVSOffset, Vector2 uVScale, bool isFeathered, bool isBlock)
        {
            Brush quadModifier = QuadraticBrush;
            ModifyTexels(position, radius, alpha, layer, detailTextureName, uVSOffset, uVScale, quadModifier, isFeathered, isBlock);
        }

        public void EraseSolidBrush(Vector3 position, float radius, float alpha, TextureLayer layer, string detailTextureName, Vector2 uVSOffset, Vector2 uVScale, bool isFeathered, bool isBlock)
        {
            Brush solidModifier = SolidEraser;
            ModifyTexels(position, radius, alpha, layer, detailTextureName, uVSOffset, uVScale, solidModifier, isFeathered, isBlock);
        }

        public void EraseLinearBrush(Vector3 position, float radius, float alpha, TextureLayer layer, string detailTextureName, Vector2 uVSOffset, Vector2 uVScale, bool isFeathered, bool isBlock)
        {
            Brush lerpModifier = LinearEraser;
            ModifyTexels(position, radius, alpha, layer, detailTextureName, uVSOffset, uVScale, lerpModifier, isFeathered, isBlock);
        }

        public void SmoothBrush(Vector3 position, float radius, bool isFeathered, bool isBlock)
        {
            Brush smoothModifier = SmoothBrush;
            ModifyTexels(position, radius, 0, 0, null, Vector2.Zero, Vector2.Zero, smoothModifier, isFeathered, isBlock);
        }

        #endregion

        #region Texel Modification Helpers

        private void CompositeBrushColor(ref Color result, TextureLayer layer, float alpha)
        {
            float newColor = 255.0f * alpha;

            switch (layer)
            {
                case TextureLayer.ALPHA:
                    result.A = (byte)Math.Max(Math.Min(result.A + newColor, 255), 0);
                    break;
                case TextureLayer.RED:
                    result.R = (byte)Math.Max(Math.Min(result.R + newColor, 255), 0);
                    break;
                case TextureLayer.GREEN:
                    result.G = (byte)Math.Max(Math.Min(result.G + newColor, 255), 0);
                    break;
                case TextureLayer.BLUE:
                    result.B = (byte)Math.Max(Math.Min(result.B + newColor, 255), 0);
                    break;
                case TextureLayer.BACKGROUND:
                    break;
            }
        }

        public delegate void Brush(int u, int v, float distance, float radius, float alpha, TextureLayer layer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="distance"></param>
        /// <param name="radiusTextureLayer"></param>
        private void SolidBrush(int u, int v, float distance, float radius, float alpha, TextureLayer layer)
        {
            CompositeBrushColor(ref mTexels[u + v * NUM_SIDE_VERTICES * NUM_SIDE_TEXELS_PER_QUAD], layer, alpha);
        }

        private void SolidEraser(int u, int v, float distance, float radius, float alpha, TextureLayer layer)
        {
            SolidBrush(u, v, distance, radius, -alpha, layer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="distance"></param>
        /// <param name="radius"></param>
        /// <param name="layer"></param>
        private void LinearBrush(int u, int v, float distance, float radius, float alpha, TextureLayer layer)
        {
            float InterpolateWeight = distance / radius;
            CompositeBrushColor(ref mTexels[u + v * NUM_SIDE_VERTICES * NUM_SIDE_TEXELS_PER_QUAD], layer, (1.0f - alpha) * alpha);
        }

        private void LinearEraser(int u, int v, float distance, float radius, float alpha, TextureLayer layer)
        {
            LinearBrush(u, v, distance, radius, -alpha, layer);
        }

        private void QuadraticBrush(int u, int v, float distance, float radius, float alpha, TextureLayer layer)
        {
            float InterpolateWeight = (float)Math.Pow(distance / radius, 2);
            CompositeBrushColor(ref mTexels[u + v * NUM_SIDE_VERTICES * NUM_SIDE_TEXELS_PER_QUAD], layer, (1.0f - alpha) * alpha);
        }

        private void SmoothBrush(int u, int v, float distance, float radius, float alpha, TextureLayer layer)
        {
            float aSum = 0.0f, rSum = 0.0f, gSum = 0.0f, bSum = 0.0f;
            int count = 0;
            for (int row = Math.Max(0, v - 1); row <= Math.Min(NUM_SIDE_VERTICES * NUM_SIDE_TEXELS_PER_QUAD - 1, v + 1); ++row)
            {
                for (int col = Math.Max(0, u - 1); col <= Math.Min(NUM_SIDE_VERTICES * NUM_SIDE_TEXELS_PER_QUAD - 1, u + 1); ++col)
                {
                    ++count;
                    Color neighborColor = mTexels[col + row * NUM_SIDE_VERTICES * NUM_SIDE_TEXELS_PER_QUAD];

                    aSum += neighborColor.A;
                    rSum += neighborColor.R;
                    gSum += neighborColor.G;
                    bSum += neighborColor.B;
                }
            }

            mTexels[u + v * NUM_SIDE_VERTICES * NUM_SIDE_TEXELS_PER_QUAD].A = (byte)(aSum / count);
            mTexels[u + v * NUM_SIDE_VERTICES * NUM_SIDE_TEXELS_PER_QUAD].R = (byte)(rSum / count);
            mTexels[u + v * NUM_SIDE_VERTICES * NUM_SIDE_TEXELS_PER_QUAD].G = (byte)(gSum / count);
            mTexels[u + v * NUM_SIDE_VERTICES * NUM_SIDE_TEXELS_PER_QUAD].B = (byte)(bSum / count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="layer"></param>
        /// <param name="detailTextureName"></param>
        private void SetTextureName(TextureLayer layer, string detailTextureName, Vector2 uVOffset, Vector2 uVScale)
        {
            if (detailTextureName != null)
            {
                mDetailTextureNames[(int)layer] = detailTextureName;
                mDetailTextureUVOffset[(int)layer] = uVOffset;
                mDetailTextureUVScale[(int)layer] = uVScale;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <param name="layer"></param>
        /// <param name="magnitude"></param>
        /// <param name="modifyTexel"></param>
        private void ModifyTexels(Vector3 position, float radius, float alpha, TextureLayer layer, string detailTextureName, Vector2 uVOffset, Vector2 uVScale, Brush modifyTexel, bool isFeathered, bool isBlock)
        {
            radius *= NUM_SIDE_VERTICES * NUM_SIDE_TEXELS_PER_QUAD;
            float radiusSquared = radius * radius;

            Vector2 position2D = new Vector2(position.X, position.Z) * NUM_SIDE_VERTICES * NUM_SIDE_TEXELS_PER_QUAD;

            int minZ = (int)Math.Max(0, position2D.Y - radius), maxZ = (int)Math.Min(NUM_SIDE_VERTICES * NUM_SIDE_TEXELS_PER_QUAD - 1, position2D.Y + radius);
            int minX = (int)Math.Max(0, position2D.X - radius), maxX = (int)Math.Min(NUM_SIDE_VERTICES * NUM_SIDE_TEXELS_PER_QUAD - 1, position2D.X + radius);

            for (int vIndex = minZ; vIndex <= maxZ; ++vIndex)
            {
                for (int uIndex = minX; uIndex <= maxX; ++uIndex)
                {
                    Vector2 displacement = new Vector2(uIndex, vIndex) - position2D;

                    float distanceSquared = displacement.LengthSquared();
                    if (isBlock || distanceSquared < radiusSquared)
                    {
                        int intRadius = (int)radius;

                        modifyTexel(uIndex, vIndex, distanceSquared, radiusSquared, alpha, layer);

                        SetTextureName(layer, detailTextureName, uVOffset, uVScale);
                    }
                }
            }

            mDirtyTexture = true;
        }

        private void UpdateTextureBuffers()
        {
            mAlphaMap.SetData(mTexels);
        }

        #endregion

        #region Serialization

        public MemoryStream SerializeHeightMap()
        {
            Texture2D heightMapTexture = new Texture2D(GraphicsManager.Device, NUM_SIDE_VERTICES, NUM_SIDE_VERTICES);
            Color[] heightTexels = new Color[NUM_SIDE_VERTICES * NUM_SIDE_VERTICES];

            for (int row = 0; row < NUM_SIDE_VERTICES; ++row)
            {
                for (int col = 0; col < NUM_SIDE_VERTICES; ++col)
                {
                    heightTexels[col + row * NUM_SIDE_VERTICES] = HeightAsColor(mVertices[col + row * NUM_SIDE_VERTICES].Position.Y);
                }
            }

            heightMapTexture.SetData(heightTexels);

            MemoryStream ms = new MemoryStream();
            heightMapTexture.SaveAsPng(ms, NUM_SIDE_VERTICES, NUM_SIDE_VERTICES);

            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }

        public MemoryStream SerializeDetailTextures()
        {
            using (var ms = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(ms);

                for (int layer = 0; layer < 5; ++layer)
                {
                    if (mDetailTextureNames[layer] != null)
                    {
                        writer.WriteLine(mDetailTextureNames[layer] + " " +
                            mDetailTextureUVOffset[layer].X + " " + mDetailTextureUVOffset[layer].Y + " " +
                            mDetailTextureUVScale[layer].X  + " " + mDetailTextureUVScale[layer].Y);
                    }
                    else
                    {
                        writer.WriteLine("NULLTEXTURE");
                    }
                    writer.Flush();
                }

                return new MemoryStream(ms.ToArray(), false);
            }
        }

        public MemoryStream SerializeAlphaMap()
        {
            Texture2D compositeTexture = new Texture2D(GraphicsManager.Device, mAlphaMap.Width, mAlphaMap.Height);

            Color[] preMultipliedAlphaTexels = new Color[mTexels.Length];
            for (int i = 0; i < mTexels.Length; ++i)
            {
                byte preMultipliedAlpha = (byte)((float)mTexels[i].A / 255.0f * 254.0f);
                preMultipliedAlphaTexels[i] = new Color(mTexels[i].R, mTexels[i].G, mTexels[i].B, preMultipliedAlpha == 0 ? 255 : preMultipliedAlpha);
            }

            compositeTexture.SetData(preMultipliedAlphaTexels);

            MemoryStream ms = new MemoryStream();

            compositeTexture.SaveAsPng(ms, mAlphaMap.Width, mAlphaMap.Height);

            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }

        private Color HeightAsColor(float height)
        {
            return new Color(height, height, height);
        }

        #endregion
    }
}
