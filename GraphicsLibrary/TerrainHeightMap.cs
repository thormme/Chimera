using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GraphicsLibrary;
using Utility;

namespace GameConstructLibrary
{

    public class TerrainHeightMap
    {

        private Texture2D mMap;
        private int mWidth;
        private int mHeight;

        private GraphicsDevice mDevice;

        public struct VertexPositionColorTextureNormal
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 TexCoord;
            public Microsoft.Xna.Framework.Color Color;

            public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
            (
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(sizeof(float) * 8, VertexElementFormat.Color, VertexElementUsage.Color, 0)
            );
        }

        VertexPositionColorTextureNormal[] vertices1D;
        VertexPositionColorTextureNormal[,] vertices2D;
        int[] indices;

        private VertexBuffer mVertexBuffer;
        public VertexBuffer VertexBuffer
        {
            get { return this.mVertexBuffer; }
            set { this.mVertexBuffer = value; }
        }
        
        private IndexBuffer mIndexBuffer;
        public IndexBuffer IndexBuffer
        {
            get { return this.mIndexBuffer; }
            set { this.mIndexBuffer = value; }
        }

        public int NumVertices
        {
            get { return this.vertices1D.Length; }
        }

        public int NumIndices
        {
            get { return this.indices.Length; }
        }

        public TerrainHeightMap(int width, int height, GraphicsDevice device)
        {
            mDevice = device;
            mMap = new Texture2D(device, width, height);
            mWidth = mMap.Width;
            mHeight = mMap.Height;
            LoadData(true);
        }

        public TerrainHeightMap(Texture2D heightTexture, GraphicsDevice device)
        {
            mDevice = device;
            mMap = heightTexture;
            mWidth = mMap.Width;
            mHeight = mMap.Height;
            LoadData(false);
        }

        public TerrainHeightMap(TerrainHeightMap copy)
        {
            FixHeightMap(copy);
        }

        public void LoadData(bool resized)
        {
            MakeVertices(resized);
            MakeIndices();
            CalculateNormals();
            MakeBuffer();
        }

        private void MakeVertices(bool resized)
        {
            vertices1D = new VertexPositionColorTextureNormal[mWidth * mHeight];
            vertices2D = new VertexPositionColorTextureNormal[mWidth, mHeight];
            for (int z = 0; z < mHeight; z++)
            {
                for (int x = 0; x < mWidth; x++)
                {

                    if (resized) // If resized rebuild map
                    {
                        vertices1D[x + z * mWidth].Position = new Vector3(x - mWidth / 2, 0.0f, z - mHeight / 2);
                        vertices1D[x + z * mWidth].Color = Microsoft.Xna.Framework.Color.BurlyWood;
                        vertices1D[x + z * mWidth].TexCoord = new Vector2((float)x / (float)mWidth, (float)z / (float)mHeight);
                        vertices2D[x, z] = vertices1D[x + z * mWidth];
                    }
                    else // Otherwise create vertices
                    {

                        vertices1D[x + z * mWidth].Position = new Vector3(x - mWidth / 2, Utils.GetTexture2DPixelColor(x, z, mMap).R * 256 * 256 + Utils.GetTexture2DPixelColor(x, z, mMap).G * 256 + Utils.GetTexture2DPixelColor(x, z, mMap).B, z - mHeight / 2);
                        vertices1D[x + z * mWidth].Color = Microsoft.Xna.Framework.Color.BurlyWood;
                        vertices1D[x + z * mWidth].TexCoord = new Vector2((float)x / (float)mWidth, (float)z / (float)mHeight);
                        vertices2D[x, z] = vertices1D[x + z * mWidth];
                    }
                }
            }
        }

        private void MakeIndices()
        {
            indices = new int[(mWidth - 1) * (mHeight - 1) * 6];
            int counter = 0;
            for (int z = 0; z < mHeight - 1; z++)
            {
                for (int x = 0; x < mWidth - 1; x++)
                {
                    int lowerLeft = x + z * mWidth;
                    int lowerRight = (x + 1) + z * mWidth;
                    int topLeft = x + (z + 1) * mWidth;
                    int topRight = (x + 1) + (z + 1) * mWidth;

                    // Make first triangle
                    indices[counter++] = topLeft;
                    indices[counter++] = lowerLeft;
                    indices[counter++] = lowerRight;

                    // Make second triangle
                    indices[counter++] = topLeft;
                    indices[counter++] = lowerRight;
                    indices[counter++] = topRight;
                }
            }
        }

        private void CalculateNormals()
        {
            for (int i = 0; i < vertices1D.Length; ++i)
            {
                vertices1D[i].Normal = new Vector3(0.0f, 0.0f, 0.0f);
            }

            for (int i = 0; i < indices.Length / 3; ++i)
            {
                int index0 = indices[i * 3];
                int index1 = indices[i * 3 + 1];
                int index2 = indices[i * 3 + 2];

                Vector3 edge0 = vertices1D[index0].Position - vertices1D[index2].Position;
                Vector3 edge1 = vertices1D[index0].Position - vertices1D[index1].Position;
                Vector3 normal = Vector3.Cross(edge0, edge1);

                vertices1D[index0].Normal += normal;
                vertices1D[index1].Normal += normal;
                vertices1D[index2].Normal += normal;
            }

            for (int i = 0; i < vertices1D.Length; ++i)
            {
                vertices1D[i].Normal.Normalize();
            }
        }

        private void MakeBuffer()
        {
            mVertexBuffer = new VertexBuffer(mDevice, VertexPositionColorTextureNormal.VertexDeclaration, vertices1D.Length, BufferUsage.None);
            mVertexBuffer.SetData(vertices1D);

            mIndexBuffer = new IndexBuffer(mDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
            mIndexBuffer.SetData(indices);
        }

        public void FixHeightMap(TerrainHeightMap copy)
        {
            mDevice = copy.mDevice;
            mMap = new Texture2D(copy.mMap.GraphicsDevice, copy.mWidth, copy.mHeight);
            Microsoft.Xna.Framework.Color[] destinationColorData = new Microsoft.Xna.Framework.Color[copy.mWidth * copy.mHeight];
            copy.mMap.GetData<Microsoft.Xna.Framework.Color>(destinationColorData);
            mMap.SetData<Microsoft.Xna.Framework.Color>(destinationColorData);
            mWidth = copy.mWidth;
            mHeight = copy.mHeight;
            vertices1D = new VertexPositionColorTextureNormal[mWidth * mHeight];
            vertices2D = new VertexPositionColorTextureNormal[mWidth, mHeight];
            indices = new int[mWidth * mHeight * 6];
            for (int z = 0; z < mHeight; z++)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    vertices1D[x + z * mWidth] = copy.vertices1D[x + z * mWidth];
                    vertices2D[x, z] = copy.vertices2D[x, z];
                }
            }

            for (int count = 0; count < copy.indices.Length; count++)
            {
                indices[count] = copy.indices[count];
            }
            
            CalculateNormals();
            MakeBuffer();
        }

        public void ModifyVertices(Vector3 position, int size, int intensity, bool feather, bool set, bool inverse, bool smooth, bool flatten)
        {

            intensity = (int)(intensity / Utils.WorldScale.Y);

            // Adjust for the rendered location of the terrain
            position.X /= Utils.WorldScale.X;
            position.Z /= Utils.WorldScale.Z;
            position.X += mWidth / 2;
            position.Z += mHeight / 2;

            // Correct for inverse
            if (inverse)
            {
                intensity = -intensity;
            }

            if (smooth) SmoothVertices(position, size);
            else if (flatten) FlattenVertices(position, size);
            else
            {

                // Build smaller grid for modifying heights
                for (int z = (int)position.Z - size; z <= (int)position.Z + size; z++)
                {
                    for (int x = (int)position.X - size; x <= (int)position.X + size; x++)
                    {
                        if (x < 0 || x >= mWidth || z < 0 || z >= mHeight) continue;
                        int distance = (int)Math.Sqrt(Math.Pow((x - position.X), 2) + Math.Pow((z - position.Z), 2));
                        if (distance < size)
                        {

                            if (set) vertices2D[x, z].Position.Y = intensity;
                            else
                            {
                                float intensityModifier;

                                if (feather) intensityModifier = (float)Math.Log(size - distance) / size;
                                else intensityModifier = 1.0f;

                                vertices2D[x, z].Position.Y += intensity * intensityModifier;
                            }
                        }

                        if (vertices2D[x, z].Position.Y > (256 * 256 * 256) - 1) vertices2D[x, z].Position.Y = (256 * 256 * 256) - 1;
                        else if (vertices2D[x, z].Position.Y < 0) vertices2D[x, z].Position.Y = 0;

                        vertices1D[x + z * mWidth] = vertices2D[x, z];

                    }
                }
            }

            CalculateNormals();
            MakeBuffer();

        }

        private void SmoothVertices(Vector3 position, int size)
        {

            // Build additional grid if smoothing
            VertexPositionColorTextureNormal[,] original = new VertexPositionColorTextureNormal[size * 2 + 1, size * 2 + 1];
            for (int z = (int)position.Z - size; z <= (int)position.Z + size; z++)
            {
                for (int x = (int)position.X - size; x <= (int)position.X + size; x++)
                {
                    if (x < 0 || x >= mWidth || z < 0 || z >= mHeight) continue;
                    original[x - (int)position.X + size, z - (int)position.Z + size] = vertices2D[x, z];
                }
            }

            for (int z = (int)position.Z - size; z < (int)position.Z + size; z++)
            {
                for (int x = (int)position.X - size; x < (int)position.X + size; x++)
                {
                    if (x < 0 || x >= mWidth || z < 0 || z >= mHeight) continue;
                    int distance = (int)Math.Sqrt(Math.Pow((x - position.X), 2) + Math.Pow((z - position.Z), 2));
                    if (distance < size)
                    {

                        float sum = 0.0f;
                        int num = 0;

                        for (int zCoord = (z - 1) - (int)position.Z + size; zCoord <= (z + 1) - (int)position.Z + size; zCoord++)
                        {
                            for (int xCoord = (x - 1) - (int)position.X + size; xCoord <= (x + 1) - (int)position.X + size; xCoord++)
                            {
                                if (zCoord < 0 || zCoord >= size * 2 ||
                                    xCoord < 0 || xCoord >= size * 2 ||
                                    (zCoord == z && xCoord == x)) continue;
                                sum += original[xCoord, zCoord].Position.Y;
                                num++;
                            }
                        }

                        vertices2D[x, z].Position.Y = (vertices2D[x, z].Position.Y + (sum / num)) / 2;
                        vertices1D[x + z * mWidth] = vertices2D[x, z];
                    }
                }
            }
        }

        private void FlattenVertices(Vector3 position, int size)
        {
            // Calculate the average height for flattening
            float average = 0.0f;
            float sum = 0.0f;
            int num = 0;
            for (int z = (int)position.Z - size; z <= (int)position.Z + size; z++)
            {
                for (int x = (int)position.X - size; x <= (int)position.X + size; x++)
                {
                    if (x < 0 || x >= mWidth || z < 0 || z >= mHeight) continue;
                    int distance = (int)Math.Sqrt(Math.Pow((x - position.X), 2) + Math.Pow((z - position.Z), 2));
                    if (distance < size)
                    {
                        num++;
                        sum += vertices2D[x, z].Position.Y;
                    }
                }
            }
            average = (float)(sum / num);

            for (int z = (int)position.Z - size; z <= (int)position.Z + size; z++)
            {
                for (int x = (int)position.X - size; x <= (int)position.X + size; x++)
                {
                    if (x < 0 || x >= mWidth || z < 0 || z >= mHeight) continue;
                    int distance = (int)Math.Sqrt(Math.Pow((x - position.X), 2) + Math.Pow((z - position.Z), 2));
                    if (distance < size)
                    {
                        vertices2D[x, z].Position.Y = average;
                        vertices1D[x + z * mWidth] = vertices2D[x, z];
                    }
                }
            }
        }

        // Used for creating a collision entity
        public Single[,] GetHeights()
        {
            Single[,] heights = new Single[mWidth, mHeight];
            for (int z = 0; z < mHeight; z++)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    heights[x, z] = (Single)(vertices2D[x, z].Position.Y);
                }
            }

            return heights;
        }

        public void Resize(int width, int height)
        {

            mWidth = width;
            mHeight = height;

            Microsoft.Xna.Framework.Rectangle sourceRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, mWidth, mHeight);
            Microsoft.Xna.Framework.Color[] retrievedColor = new Microsoft.Xna.Framework.Color[mWidth * mHeight];

            mMap.GetData<Microsoft.Xna.Framework.Color>(
                0,
                sourceRectangle,
                retrievedColor,
                0,
                mWidth * mHeight);

            mMap = new Texture2D(mDevice, mWidth, mHeight);
            mMap.SetData<Microsoft.Xna.Framework.Color>(retrievedColor);

            LoadData(true);

        }

        public void Save(string file)
        {

            Bitmap bmp = new Bitmap(mMap.Width, mMap.Height);

            // Update mMap with modifications
            for (int z = 0; z < mHeight; z++)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    int height = (int)(vertices2D[x, z].Position.Y);
                    int blue = height % 256;
                    int green = ((height - blue) / 256) % 256;
                    int red = ((height - (blue + green)) / (256 * 256) % 256);
                    bmp.SetPixel(x, z, System.Drawing.Color.FromArgb(red, green, blue));
                }
            }

            // Redefine bitmap for permissions purposes
            bmp.Save(DirectoryManager.GetRoot() + "Chimera/ChimeraContent/levels/maps/" + file + ".bmp");
        }
    }
}