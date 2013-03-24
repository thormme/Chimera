using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GraphicsLibrary
{
    public class SkyBoxRenderable : Renderable
    {
        #region Public Properties

        private string mTextureName = null;
        public string TextureName
        {
            get { return mTextureName; }
            set { mTextureName = value; }
        }

        #endregion

        #region Private Variables

        VertexBuffer mVertexBuffer;
        IndexBuffer mIndexBuffer;

        #endregion
        
        public SkyBoxRenderable(string textureName)
        {
            if (textureName != "default")
            {
                TextureName = textureName;
            }

            CreateBuffers();
        }

        protected override void Draw(Microsoft.Xna.Framework.Matrix worldTransform, Color overlayColor, float overlayColorWeight)
        {
            GraphicsManager.RenderSkyBox(mVertexBuffer, mIndexBuffer, TextureName, worldTransform, overlayColor, overlayColorWeight);
        }

        private void CreateBuffers()
        {
            const int bottomLength = 2, rightLength = 2, numSides = 6;
            const float uLength = 1.0f / 4.0f, vLength = 1.0f / 3.0f;

            float[] sideUOrigin = { uLength, uLength, 2 * uLength, 3 * uLength, 0.0f, uLength };
            float[] sideVOrigin = { 0.0f, vLength, vLength, vLength, vLength, 2 * vLength };
            Vector3[] sideTopLeft = { new Vector3(-0.5f, 1.0f, 0.5f), new Vector3(-0.5f, 1.0f, -0.5f), new Vector3(0.5f, 1.0f, -0.5f), new Vector3(0.5f, 1.0f, 0.5f), new Vector3(-0.5f, 1.0f, 0.5f), new Vector3(-0.5f, 0.0f, -0.5f) };
            Vector3[] sideRight = { new Vector3(1.0f, 0.0f, 0.0f), new Vector3(1.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f), new Vector3(-1.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, -1.0f), new Vector3(1.0f, 0.0f, 0.0f) };
            Vector3[] sideBottom = { new Vector3(0.0f, 0.0f, -1.0f), new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), new Vector3(0.0f, 0.0f, 1.0f) };
            Vector3[] sideNormal = { new Vector3(0, -1, 0), new Vector3(0, 0, 1), new Vector3(-1, 0, 0), new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector3(0, 1, 0) };

            VertexPositionNormalTexture[] skyBoxVertices = new VertexPositionNormalTexture[numSides * bottomLength * rightLength];

            int sideIndex;
            for (sideIndex = 0; sideIndex < 6; ++sideIndex)
            {
                for (int bottom = 0; bottom <= 1; ++bottom)
                {
                    for (int right = 0; right <= 1; ++right)
                    {
                        VertexPositionNormalTexture vertex = new VertexPositionNormalTexture();
                        vertex.Position = sideTopLeft[sideIndex] + bottom * sideBottom[sideIndex] + right * sideRight[sideIndex];
                        vertex.Normal = sideNormal[sideIndex];
                        vertex.TextureCoordinate = new Vector2(sideUOrigin[sideIndex] + right * uLength, sideVOrigin[sideIndex] + bottom * vLength);

                        skyBoxVertices[sideIndex * bottomLength * rightLength + bottom * bottomLength + right] = vertex;
                    }
                }
            }

            mVertexBuffer = new VertexBuffer(GraphicsManager.Device, VertexPositionNormalTexture.VertexDeclaration, skyBoxVertices.Length, BufferUsage.WriteOnly);
            mVertexBuffer.SetData(skyBoxVertices);

            const int numQuadVertices = 6;
            int[] skyBoxIndices = new int[numSides * numQuadVertices];

            int count = 0;
            for (sideIndex = 0; sideIndex < numSides; ++sideIndex)
            {
                int topLeftIndex = sideIndex * bottomLength * rightLength;
                int topRightIndex = topLeftIndex + 1;
                int bottomLeftIndex = topRightIndex + 1;
                int bottomRightIndex = bottomLeftIndex + 1;

                skyBoxIndices[count++] = topLeftIndex;
                skyBoxIndices[count++] = topRightIndex;
                skyBoxIndices[count++] = bottomRightIndex;

                skyBoxIndices[count++] = topLeftIndex;
                skyBoxIndices[count++] = bottomRightIndex;
                skyBoxIndices[count++] = bottomLeftIndex;
            }

            mIndexBuffer = new IndexBuffer(GraphicsManager.Device, typeof(int), skyBoxIndices.Length, BufferUsage.WriteOnly);
            mIndexBuffer.SetData(skyBoxIndices);
        }
    }
}
