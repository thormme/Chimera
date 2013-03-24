using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphicsLibrary
{
    public class WaterRenderable : Renderable
    {
        #region Constants

        float SEA_WIDTH  = 800.0f;
        float SEA_HEIGHT = 800.0f;

        #endregion

        #region Public Properties

        public VertexBuffer VertexBuffer
        {
            get { return mVertexBuffer; }
            set { mVertexBuffer = value; }
        }

        public IndexBuffer IndexBuffer
        {
            get { return mIndexBuffer; }
            set { mIndexBuffer = value; }
        }
        
        public float SeaLevel
        {
            get { return mSeaLevel; }
            set
            { 
                mSeaLevel = value;
                ChangeSeaLevel();
                FillBuffers();
            }
        }

        public Vector2 Resolution
        {
            get { return mResolution; }
            set
            {
                mResolution = value;
                ResizeVertices();
                ResizeIndices();
                FillBuffers();
            }
        }

        public string TextureName
        {
            get { return mTextureName; }
            set { mTextureName = value; }
        }
        private string mTextureName;

        #endregion

        #region Private Variables

        VertexPositionNormalTexture[] mVertices = null;
        int[] mIndices = null;

        private VertexBuffer mVertexBuffer = null;
        private IndexBuffer mIndexBuffer = null;

        private float mSeaLevel;
        private Vector2 mResolution;

        #endregion

        public WaterRenderable(string textureName, float seaLevel, Vector2 resolution)
        {
            mSeaLevel = seaLevel;
            mResolution = resolution;
            mTextureName = textureName;

            ResizeVertices();
            ResizeIndices();
            FillBuffers();
        }

        protected override void Draw(Matrix worldTransform, Color overlayColor, float overlayColorWeight)
        {
            GraphicsManager.RenderWater(mVertexBuffer, mIndexBuffer, mTextureName, worldTransform, overlayColor, overlayColorWeight);
        }

        private void ResizeVertices()
        {
            Vector2 quadResolution = mResolution - Vector2.One;

            mVertices = new VertexPositionNormalTexture[(int)mResolution.X * (int)mResolution.Y];

            float quadWidth  = SEA_WIDTH  / (float)quadResolution.X;
            float quadHeight = SEA_HEIGHT / (float)quadResolution.Y;

            for (int y = 0; y < mResolution.Y; ++y)
            {
                for (int x = 0; x < mResolution.X; ++x)
                {
                    VertexPositionNormalTexture vertex = new VertexPositionNormalTexture();
                    vertex.Position = new Vector3(-SEA_WIDTH / 2.0f + (float)x * quadWidth, mSeaLevel, -SEA_HEIGHT / 2.0f + (float)y * quadHeight);
                    vertex.Normal = Vector3.Up;
                    vertex.TextureCoordinate = new Vector2((float)x / quadResolution.X * 800.0f, (float)y / (float)quadResolution.Y * 800.0f);

                    mVertices[x + y * (int)mResolution.X] = vertex;
                }
            }
        }

        private void ChangeSeaLevel()
        {
            for (int i = 0; i < mVertices.Length; ++i)
            {
                mVertices[i].Position.Y = mSeaLevel;
            }
        }

        private void ResizeIndices()
        {
            Vector2 quadResolution = mResolution - Vector2.One;

            mIndices = new int[6 * (int)quadResolution.X * (int)quadResolution.Y];

            int indicesIndex = 0;
            for (int y = 0; y < quadResolution.Y; ++y)
            {
                for (int x = 0; x < quadResolution.X; ++x)
                {
                    int TLIndex = x +      y      * (int)mResolution.X;
                    int TRIndex = x + 1 +  y      * (int)mResolution.X;
                    int BRIndex = x + 1 + (y + 1) * (int)mResolution.X;
                    int BLIndex = x +     (y + 1) * (int)mResolution.X;

                    mIndices[indicesIndex++] = TLIndex;
                    mIndices[indicesIndex++] = TRIndex;
                    mIndices[indicesIndex++] = BRIndex;

                    mIndices[indicesIndex++] = TLIndex;
                    mIndices[indicesIndex++] = BRIndex;
                    mIndices[indicesIndex++] = BLIndex;
                }
            }
        }

        private void FillBuffers()
        {
            if (mVertexBuffer == null || mVertexBuffer.VertexCount != mVertices.Length)
            {
                mVertexBuffer = new VertexBuffer(GraphicsManager.Device, VertexPositionNormalTexture.VertexDeclaration, mVertices.Length, BufferUsage.None);
            }
            mVertexBuffer.SetData(mVertices);

            if (mIndexBuffer == null || mIndexBuffer.IndexCount != mIndices.Length)
            {
                mIndexBuffer = new IndexBuffer(GraphicsManager.Device, typeof(int), mIndices.Length, BufferUsage.None);
            }
            mIndexBuffer.SetData(mIndices);
        }
    }
}
