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

namespace GameConstructLibrary
{

    public class TerrainHeightMap
    {

        private const int scale = 2;

        private Bitmap mMap;
        private int mWidth;
        private int mHeight;

        private GraphicsDevice mDevice;

        public struct VertexPositionColorNormal
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

        VertexPositionColorNormal[] vertices1D;
        VertexPositionColorNormal[,] vertices2D;
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
            mMap = new Bitmap(width, height);
            mWidth = mMap.Width;
            mHeight = mMap.Height;
            LoadData(true);
        }

        public TerrainHeightMap(string file, GraphicsDevice device)
        {
            mDevice = device;
            mMap = new Bitmap(DirectoryManager.GetRoot() + "finalProject/finalProjectContent/levels/maps/" + file + ".bmp");
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
            vertices1D = new VertexPositionColorNormal[mWidth * mHeight];
            vertices2D = new VertexPositionColorNormal[mWidth, mHeight];
            for (int z = 0; z < mHeight; z++)
            {
                for (int x = 0; x < mWidth; x++)
                {

                    if (resized) // If resized rebuild map
                    {
                        vertices1D[x + z * mWidth].Position = new Vector3(x - mWidth / 2, 100, z - mHeight / 2);
                        vertices1D[x + z * mWidth].Color = Microsoft.Xna.Framework.Color.BurlyWood;
                        vertices2D[x, z] = vertices1D[x + z * mWidth];
                    }
                    else // Otherwise create vertices
                    {
                        vertices1D[x + z * mWidth].Position = new Vector3(x - mWidth / 2, mMap.GetPixel(x, z).R, z - mHeight / 2);
                        vertices1D[x + z * mWidth].Color = Microsoft.Xna.Framework.Color.BurlyWood;
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
            mVertexBuffer = new VertexBuffer(mDevice, VertexPositionColorNormal.VertexDeclaration, vertices1D.Length, BufferUsage.WriteOnly);
            mVertexBuffer.SetData(vertices1D);

            mIndexBuffer = new IndexBuffer(mDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
            mIndexBuffer.SetData(indices);
        }

        public void FixHeightMap(TerrainHeightMap copy)
        {
            mDevice = copy.mDevice;
            mMap = copy.mMap.Clone() as Bitmap;
            mWidth = copy.mWidth;
            mHeight = copy.mHeight;
            vertices1D = new VertexPositionColorNormal[mWidth * mHeight];
            vertices2D = new VertexPositionColorNormal[mWidth, mHeight];
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

        public void ModifyVertices(Vector3 position, int size, int intensity, bool feather, bool setHeight, bool smooth)
        {

            // Adjust for the rendered location of the terrain
            position /= scale;
            position.X += mWidth / 2;
            position.Z += mHeight / 2;

            // Calculate the average height for smoothing
            float average = 0.0f;
            if (smooth)
            {
                float sum = 0.0f;
                int num = 0;
                for (int z = (int)position.Z - size; z < (int)position.Z + size; z++)
                {
                    for (int x = (int)position.X - size; x < (int)position.X + size; x++)
                    {
                        if (x < 0 || x >= mWidth || z < 0 || z >= mHeight) break;
                        int distance = (int)Math.Sqrt(Math.Pow((x - position.X), 2) + Math.Pow((z - position.Z), 2));
                        if (distance < size)
                        {
                            num++;
                            sum += vertices2D[x, z].Position.Y;
                        }
                    }
                }
                average = (float)(sum / num);
            }

            // Build smaller grid for modifying heights
            for (int z = (int)position.Z - size; z < (int)position.Z + size; z++)
            {
                for (int x = (int)position.X - size; x < (int)position.X + size; x++)
                {
                    if (x < 0 || x >= mWidth || z < 0 || z >= mHeight) break;
                    int distance = (int)Math.Sqrt(Math.Pow((x - position.X), 2) + Math.Pow((z - position.Z), 2));
                    if (distance < size)
                    {

                        if (smooth)
                        {
                            float amount = vertices2D[x, z].Position.Y;
                            if (amount > average)
                            {
                                if (amount - intensity < average)
                                    amount = average;
                                else
                                    amount -= intensity;
                            }
                            else if (amount < average)
                            {
                                if (amount + intensity > average)
                                    amount = average;
                                else
                                    amount += intensity;
                            }
                            vertices2D[x, z].Position.Y = amount;
                        }
                        else if (setHeight)
                        {
                            vertices2D[x, z].Position.Y = intensity;
                        }
                        else
                        {
                            float intensityModifier;
                            if (feather)
                            {
                                intensityModifier = (float)Math.Log(size - distance) / size;
                            }
                            else
                            {
                                intensityModifier = 1.0f;
                            }
                            vertices2D[x, z].Position.Y += intensity * intensityModifier;
                        }
                    }
                    if (vertices2D[x, z].Position.Y > 255) vertices2D[x, z].Position.Y = 255;
                    else if (vertices2D[x, z].Position.Y < 0) vertices2D[x, z].Position.Y = 0;
                    vertices1D[x + z * mWidth] = vertices2D[x, z];
                }
            }

            CalculateNormals();
            MakeBuffer();

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
            LoadData(true);
        }

        public void Save(string file)
        {

            mMap = new Bitmap(mMap, new Size(mWidth, mHeight));

            // Update mMap with modifications
            for (int z = 0; z < mHeight; z++)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    mMap.SetPixel(x, z, System.Drawing.Color.FromArgb((int)(vertices2D[x, z].Position.Y), 0, 0));
                }
            }

            // Redefine bitmap for permissions purposes
            Bitmap newMap = new Bitmap(mMap);
            mMap.Dispose();
            newMap.Save(DirectoryManager.GetRoot() + "finalProject/finalProjectContent/levels/maps/" + file + ".bmp");

            // Reassign map
            mMap = new Bitmap(newMap);
            newMap.Dispose();
        }
    }
}