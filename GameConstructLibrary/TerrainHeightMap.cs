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

namespace GameConstructLibrary
{
    public class TerrainHeightMap
    {
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

        private GraphicsDevice mDevice;
        public GraphicsDevice Device
        {
            get { return this.mDevice; }
            set { this.mDevice = value; }
        }

        Bitmap map;

        const int vertexSpacing = 1;
        VertexPositionColorNormal[] vertices;
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
            get { return this.vertices.Length; }
        }

        public int NumIndices
        {
            get { return this.indices.Length; }
        }

        /// <summary>
        /// Used to create a height map
        /// </summary>
        public TerrainHeightMap(int width, int height, GraphicsDevice device)
        {
            mDevice = device;
            map = new Bitmap(width, height);
            LoadData();
        }

        /// <summary>
        /// Used to load a height map
        /// </summary>
        public TerrainHeightMap(string file, GraphicsDevice device)
        {
            mDevice = device;
            map = new Bitmap(DirectoryManager.GetRoot() + "finalProject/finalProjectContent/levels/maps/" + file + ".bmp");
            LoadData();
        }

        public void LoadData()
        {
            MakeVertices();
            MakeIndices();

            CalculateNormals();

            MakeBuffer();
        }

        private void MakeVertices()
        {
            vertices = new VertexPositionColorNormal[map.Width * map.Height];
            for (int x = 0; x < map.Width; x++)
            {
                for (int z = 0; z < map.Height; z++)
                {
                    vertices[x + z * map.Width].Position = new Vector3(x - map.Width / 2, map.GetPixel(x, z).R, z - map.Height / 2);
                    vertices[x + z * map.Width].Color = Microsoft.Xna.Framework.Color.BurlyWood;
                }
            }
        }

        private void MakeIndices()
        {
            indices = new int[(map.Width - 1) * (map.Height - 1) * 6];
            int counter = 0;
            for (int z = 0; z < map.Height - 1; z++)
            {
                for (int x = 0; x < map.Width - 1; x++)
                {
                    int lowerLeft = x + z * map.Width;
                    int lowerRight = (x + 1) + z * map.Width;
                    int topLeft = x + (z + 1) * map.Width;
                    int topRight = (x + 1) + (z + 1) * map.Width;

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
            for (int i = 0; i < vertices.Length; ++i)
            {
                vertices[i].Normal = new Vector3(0.0f, 0.0f, 0.0f);
            }

            for (int i = 0; i < indices.Length / 3; ++i)
            {
                int index0 = indices[i * 3];
                int index1 = indices[i * 3 + 1];
                int index2 = indices[i * 3 + 2];

                Vector3 edge0  = vertices[index0].Position - vertices[index2].Position;
                Vector3 edge1  = vertices[index0].Position - vertices[index1].Position;
                Vector3 normal = Vector3.Cross(edge0, edge1);

                vertices[index0].Normal += normal;
                vertices[index1].Normal += normal;
                vertices[index2].Normal += normal;
            }

            for (int i = 0; i < vertices.Length; ++i)
            {
                vertices[i].Normal.Normalize();
            }
        }

        private void MakeBuffer()
        {
            mVertexBuffer = new VertexBuffer(mDevice, VertexPositionColorNormal.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            mVertexBuffer.SetData(vertices);

            mIndexBuffer = new IndexBuffer(mDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
            mIndexBuffer.SetData(indices);
        }

        public void ModifyVertices(Vector3 position, int size, int intensity, bool setHeight)
        {
            Vector3 vertexPosition = position / vertexSpacing;
            for (int x = (int)(vertexPosition.X - size); x < (int)(vertexPosition.X + size); x++)
            {
                for (int z = (int)(vertexPosition.Z - size); z < (int)(vertexPosition.Z + size); z++)
                {
                    int currentPixel = x + z * map.Width;
                    if (currentPixel >= 0 && currentPixel <= map.Width * map.Height)
                    {
                        if (setHeight)
                        {
                            vertices[currentPixel].Position.Y = intensity;
                        }
                        else
                        {
                            vertices[currentPixel].Position.Y += intensity;
                        }
                    }
                }
            }
        }

        public Single[,] GetHeights()
        {
            Single[,] heights = new Single[map.Width, map.Height];
            for (int x = 0; x < map.Width; x++)
            {
                for (int z = 0; z < map.Height; z++)
                {
                    heights[x, z] = (Single)(vertices[x + z * map.Width].Position.Y);
                }
            }

            return heights;
        }

        public void Save(string file)
        {
            // Update map with modifications
            for (int x = 0; x < map.Width; x++)
            {
                for (int z = 0; z < map.Height; z++)
                {
                    map.SetPixel(x, z, System.Drawing.Color.FromArgb((int)(vertices[x + z * map.Width].Position.Y), 0, 0));
                }
            }

            // Redefine bitmap for permissions purposes
            Bitmap newMap = new Bitmap(map);
            map.Dispose();
            newMap.Save(DirectoryManager.GetRoot() + "finalProject/finalProjectContent/levels/maps/" + file);

            // Reassign map
            map = new Bitmap(newMap);
            newMap.Dispose();
        }
    }
}
