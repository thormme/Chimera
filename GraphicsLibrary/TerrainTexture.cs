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

    public class TerrainTexture
    {
        #region Public Properties

        /// <summary>
        /// Matrix of Textures containing copies of texels for each chunk.
        /// </summary>
        public Texture2D[,] TextureBuffers
        {
            get { return mTextureBuffers; }
            set { mTextureBuffers = value; }
        }
        private Texture2D[,] mTextureBuffers;

        /// <summary>
        /// Matrix of strings containing names of detail textures at each layer for each chunk.
        /// </summary>
        public string[,,] DetailTextureNames
        {
            get { return mDetailTextureNames; }
            set { mDetailTextureNames = value; }
        }
        private string[, ,] mDetailTextureNames;

        /// <summary>
        /// Total number of texels heightmap is wide.
        /// </summary>
        public int Width
        {
            get { return mWidth; }
            set { mWidth = value; }
        }
        private int mWidth;

        /// <summary>
        /// Total number of texels heightmap is tall.
        /// </summary>
        public int Height
        {
            get { return mHeight; }
            set { mHeight = value; }
        }
        private int mHeight;

        /// <summary>
        /// Total number of texels that each chunk is wide.
        /// </summary>
        public int ChunkWidth
        {
            get { return mChunkWidth; }
            set { mChunkWidth = value; }
        }
        private int mChunkWidth;

        /// <summary>
        /// Total number of texels that each chunk is tall.
        /// </summary>
        public int ChunkHeight
        {
            get { return mChunkHeight; }
            set { mChunkHeight = value; }
        }
        private int mChunkHeight;

        /// <summary>
        /// Total number of chunks texture is horizontally broken in to.
        /// </summary>
        public int NumChunksHorizontal
        {
            get { return mNumChunksHorizontal; }
            set { mNumChunksHorizontal = value; }
        }
        private int mNumChunksHorizontal;

        /// <summary>
        /// Total number of chunks texture is vertically broken in to.
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
        /// Whether paint brush is a block or a circle.
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
        /// Table for saving on texture buffer writes.
        /// </summary>
        private bool[,] mDirtyChunks;

        /// <summary>
        /// Modifiable representation of texture.
        /// </summary>
        private Color[] mTexels;

        #endregion

        #region Constants

        private Color[] FULL_OPACITY_PALETTE = new Color[] 
            { 
                new Color(0,0,0,0), 
                new Color(255, 0, 0, 0), 
                new Color(0, 255, 0, 0), 
                new Color(0, 0, 255, 0), 
                new Color(0, 0, 0, 255) 
            };

        #endregion

        #region Enums

        public enum TextureLayer { BACKGROUND, RED, GREEN, BLUE, ALPHA };

        #endregion

        #region Construction

        public TerrainTexture(Texture2D alphaMap, string[,,] detailTextureNames, int numChunksHorizontal, int numChunksVertical, GraphicsDevice device)
        {
            mDevice = device;

            mHeight = alphaMap.Height;
            mWidth = alphaMap.Width;

            mNumChunksVertical = numChunksVertical;
            mNumChunksHorizontal = numChunksHorizontal;

            mChunkHeight = mHeight / mNumChunksVertical;
            mChunkWidth = mWidth / mNumChunksHorizontal;

            InitializeTexels(alphaMap);

            mTextureBuffers = new Texture2D[mNumChunksVertical, mNumChunksHorizontal];

            mDetailTextureNames = detailTextureNames;

            mDirtyChunks = new bool[mNumChunksVertical, mNumChunksHorizontal];
            for (int row = 0; row < mNumChunksVertical; ++row)
            {
                for (int col = 0; col < mNumChunksHorizontal; ++col)
                {
                    mDirtyChunks[row, col] = true;
                }
            }

            UpdateTextureBuffers();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// 
        /// </summary>
        private void InitializeTexels(Texture2D alphaMap)
        {
            mTexels = new Color[mHeight * mWidth];
            alphaMap.GetData(mTexels);

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="verticalIndex"></param>
        /// <param name="horizontalIndex"></param>
        /// <returns></returns>
        private Color[] ConstructChunk(int verticalIndex, int horizontalIndex)
        {
            Color[] result = new Color[mChunkHeight * mChunkWidth];

            int originRow = verticalIndex * mChunkHeight;
            int originCol = horizontalIndex * mChunkWidth;

            for (int vIndex = 0; vIndex < mChunkHeight; ++vIndex)
            {
                for (int uIndex = 0; uIndex < mChunkWidth; ++uIndex)
                {
                    int texelBufferIndex = (originCol + uIndex) + (originRow + vIndex) * mWidth;
                    int resultIndex = uIndex + vIndex * mChunkWidth;

                    result[resultIndex] = mTexels[texelBufferIndex];
                }
            }

            return result;
        }

        #endregion

        #region Texel Modification

        public void PaintTerrain(Vector2 position, float radius, float alpha, TextureLayer layer, string detailTextureName)
        {
            if (!mIsFeathered)
            {
                PaintSolidBrush(position, radius, alpha, layer, detailTextureName);
            }
            else
            {
                PaintLinearBrush(position, radius, alpha, layer, detailTextureName);
            }
        }

        public void EraseTerrain(Vector2 position, float radius, float alpha, TextureLayer layer, string detailTextureName)
        {
            if (!mIsFeathered)
            {
                EraseSolidBrush(position, radius, alpha, layer, detailTextureName);
            }
            else
            {
                EraseLinearBrush(position, radius, alpha, layer, detailTextureName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <param name="layer"></param>
        /// <param name="detailTextureName"></param>
        public void PaintSolidBrush(Vector2 position, float radius, float alpha, TextureLayer layer, string detailTextureName)
        {
            Brush solidModifier = SolidBrush;
            ModifyTexels(position, radius, alpha, layer, detailTextureName, solidModifier);
        }

        public void PaintLinearBrush(Vector2 position, float radius, float alpha, TextureLayer layer, string detailTextureName)
        {
            Brush lerpModifier = LinearBrush;
            ModifyTexels(position, radius, alpha, layer, detailTextureName, lerpModifier);
        }

        public void PaintQuadraticBrush(Vector2 position, float radius, float alpha, TextureLayer layer, string detailTextureName)
        {
            Brush quadModifier = QuadraticBrush;
            ModifyTexels(position, radius, alpha, layer, detailTextureName, quadModifier);
        }

        public void EraseSolidBrush(Vector2 position, float radius, float alpha, TextureLayer layer, string detailTextureName)
        {
            Brush solidModifier = SolidEraser;
            ModifyTexels(position, radius, alpha, layer, detailTextureName, solidModifier);
        }

        public void EraseLinearBrush(Vector2 position, float radius, float alpha, TextureLayer layer, string detailTextureName)
        {
            Brush lerpModifier = LinearEraser;
            ModifyTexels(position, radius, alpha, layer, detailTextureName, lerpModifier);
        }

        #endregion

        #region Texel Modification Helpers

        private Color CompositeBrushColor(Color baseColor, Color newColor)
        {
            return new Color(baseColor.R + newColor.R, baseColor.G + newColor.G, baseColor.B + newColor.B, baseColor.A + newColor.A);
        }

        private delegate void Brush(int u, int v, float distance, float radius, float alpha, TextureLayer layer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="distance"></param>
        /// <param name="radiusTextureLayer"></param>
        private void SolidBrush(int u, int v, float distance, float radius, float alpha, TextureLayer layer)
        {
            Color newComposite = FULL_OPACITY_PALETTE[(int)layer] * alpha;
            Color oldComposite = mTexels[u + v * mWidth] * (1.0f - alpha);

            mTexels[u + v * mWidth] = CompositeBrushColor(oldComposite, newComposite);
        }

        private void SolidEraser(int u, int v, float distance, float radius, float alpha, TextureLayer layer)
        {
            Color newComposite = FULL_OPACITY_PALETTE[(int)layer] * -1.0f * alpha;
            Color oldComposite = mTexels[u + v * mWidth] * (1.0f - alpha);

            mTexels[u + v * mWidth] = CompositeBrushColor(oldComposite, newComposite);
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
            Color newComposite = FULL_OPACITY_PALETTE[(int)layer] * (1.0f - InterpolateWeight) * alpha;
            Color oldComposite = mTexels[u + v * mWidth] * InterpolateWeight * (1.0f - alpha);

            mTexels[u + v * mWidth] = CompositeBrushColor(oldComposite, newComposite);
        }

        private void LinearEraser(int u, int v, float distance, float radius, float alpha, TextureLayer layer)
        {
            float InterpolateWeight = distance / radius;
            Color newComposite = FULL_OPACITY_PALETTE[(int)layer] *-1.0f * (1.0f - InterpolateWeight) * alpha;
            Color oldComposite = mTexels[u + v * mWidth] * InterpolateWeight * (1.0f - alpha);

            mTexels[u + v * mWidth] = CompositeBrushColor(oldComposite, newComposite);
        }

        private void QuadraticBrush(int u, int v, float distance, float radius, float alpha, TextureLayer layer)
        {
            float InterpolateWeight = (float)Math.Pow(distance / radius, 2);
            Color newComposite = FULL_OPACITY_PALETTE[(int)layer] * (1.0f - InterpolateWeight) * alpha;
            Color oldComposite = mTexels[u + v * mWidth] * InterpolateWeight * (1.0f - alpha);

            mTexels[u + v * mWidth] = CompositeBrushColor(oldComposite, newComposite);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        private void WriteDirtyBit(int u, int v)
        {
            int chunkRowIndex = v / mChunkHeight;
            int chunkColIndex = u / mChunkWidth;

            mDirtyChunks[chunkRowIndex, chunkColIndex] = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="layer"></param>
        /// <param name="detailTextureName"></param>
        private void SetTextureName(int u, int v, TextureLayer layer, string detailTextureName)
        {
            int chunkRowIndex = v / mChunkHeight;
            int chunkColIndex = u / mChunkWidth;

            mDetailTextureNames[chunkRowIndex, chunkColIndex, (int)layer] = detailTextureName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <param name="layer"></param>
        /// <param name="magnitude"></param>
        /// <param name="modifyTexel"></param>
        private void ModifyTexels(Vector2 position, float radius, float alpha, TextureLayer layer, string detailTextureName, Brush modifyTexel)
        {
            radius = radius / 100.0f * mHeight;
            float radiusSquared = radius * radius;

            // Normalize origin of modification to [0, mHeight), [0, mWidth).
            position.X = (position.X / Utils.WorldScale.X + 50.0f) / 100.0f * mWidth;// mWidth / 2;
            position.Y = (position.Y / Utils.WorldScale.Z + 50.0f) / 100.0f * mHeight;// mHeight / 2;

            int minZ = (int)Math.Max(0, position.Y - radius), maxZ = (int)Math.Min(mHeight - 1, position.Y + radius);
            int minX = (int)Math.Max(0, position.X - radius), maxX = (int)Math.Min(mWidth - 1, position.X + radius);

            for (int vIndex = minZ; vIndex <= maxZ; ++vIndex)
            {
                for (int uIndex = minX; uIndex <= maxX; ++uIndex)
                {
                    Vector2 displacement = new Vector2(uIndex, vIndex) - position;

                    float distanceSquared = displacement.LengthSquared();
                    if (mIsBlock || distanceSquared < radiusSquared)
                    {
                        modifyTexel(uIndex, vIndex, distanceSquared, radiusSquared, alpha, layer);

                        SetTextureName(uIndex, vIndex, layer, detailTextureName);
                        WriteDirtyBit(uIndex, vIndex);
                    }
                }
            }

            UpdateTextureBuffers();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateTextureBuffers()
        {
            for (int chunkVerticalIndex = 0; chunkVerticalIndex < mNumChunksVertical; ++chunkVerticalIndex)
            {
                for (int chunkHorizontalIndex = 0; chunkHorizontalIndex < mNumChunksHorizontal; ++chunkHorizontalIndex)
                {
                    if (mDirtyChunks[chunkVerticalIndex, chunkHorizontalIndex] == true)
                    {
                        Color[] texels = ConstructChunk(chunkVerticalIndex, chunkHorizontalIndex);

                        if (mTextureBuffers[chunkVerticalIndex, chunkHorizontalIndex] == null)
                        {
                            mTextureBuffers[chunkVerticalIndex, chunkHorizontalIndex] = new Texture2D(mDevice, mChunkWidth, mChunkHeight);
                        }
                        mTextureBuffers[chunkVerticalIndex, chunkHorizontalIndex].SetData(texels);

                        mDirtyChunks[chunkVerticalIndex, chunkHorizontalIndex] = false;
                    }
                }
            }
        }

        #endregion

        #region Saving

        public MemoryStream ExportTextureToStream()
        {
            Texture2D compositeTexture = new Texture2D(mDevice, mWidth, mHeight);

            Color[] preMultipliedAlphaTexels = new Color[mTexels.Length];
            for (int i = 0; i < mTexels.Length; ++i)
            {
                byte preMultipliedAlpha = (byte)((float)mTexels[i].A / 255.0f * 254.0f);
                preMultipliedAlphaTexels[i] = new Color(mTexels[i].R, mTexels[i].G, mTexels[i].B, preMultipliedAlpha == 0 ? 255 : preMultipliedAlpha);
            }

            compositeTexture.SetData(preMultipliedAlphaTexels);

            MemoryStream ms = new MemoryStream();

            compositeTexture.SaveAsPng(ms, mWidth, mHeight);

            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }

        public MemoryStream ExportDetailListToStream()
        {
            using (var ms = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(ms);

                writer.WriteLine(mNumChunksVertical.ToString() + " " + mNumChunksHorizontal.ToString());
                writer.Flush();

                for (int row = 0; row < mNumChunksVertical; ++row)
                {
                    for (int col = 0; col < mNumChunksHorizontal; ++col)
                    {
                        writer.WriteLine(row.ToString() + " " + col.ToString());
                        writer.Flush();

                        for (int layer = 0; layer < 5; ++layer)
                        {
                            if (mDetailTextureNames[row, col, layer] != null)
                            {
                                writer.WriteLine(mDetailTextureNames[row, col, layer]);
                            }
                            else
                            {
                                writer.WriteLine("NULLTEXTURE");
                            }
                            writer.Flush();
                        }
                    }
                }

                return new MemoryStream(ms.ToArray(), false);
            }
        }

        #endregion
    }
}