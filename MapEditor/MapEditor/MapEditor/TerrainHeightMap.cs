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


namespace MapEditor
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class TerrainHeightMap
    {
        Bitmap map;

        const int vertexSpacing = 1;
        VertexPositionColor[] vertices; // Change to texture eventually
        int[] indices;

        /// <summary>
        /// Used to create a height map
        /// </summary>
        public TerrainHeightMap(int width, int height)
        {
            map = new Bitmap(width, height);
            LoadData();
        }

        /// <summary>
        /// Used to load a height map
        /// </summary>
        public TerrainHeightMap(string file)
        {
            map = new Bitmap(Globals.MapPath + file);
            LoadData();
        }

        public void LoadData()
        {
            MakeVertices();
            MakeIndices();
        }

        private void MakeVertices()
        {
            vertices = new VertexPositionColor[map.Width * map.Height];
            for (int x = 0; x < map.Width; x++)
            {
                for (int z = 0; z < map.Height; z++)
                {
                    vertices[x + z * map.Width].Position = new Vector3(x, map.GetPixel(x, z).R, z);
                    vertices[x + z * map.Width].Color = Microsoft.Xna.Framework.Color.White;
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
                    int p0 = x + z * map.Width;
                    int p1 = (x + 1) + z * map.Width;
                    int p2 = x + (z + 1) * map.Width;
                    int p3 = (x + 1) + (z + 1) * map.Width;

                    // Make first triangle
                    indices[counter++] = p2;
                    indices[counter++] = p1;
                    indices[counter++] = p0;

                    // Make second triangle
                    indices[counter++] = p2;
                    indices[counter++] = p3;
                    indices[counter++] = p1;
                }
            }
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
                    heights[x,z] = (Single)(vertices[x + z * map.Width].Position.Y);
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
            newMap.Save(Globals.MapPath + file);
            
            // Reassign map
            map = new Bitmap(newMap);
            newMap.Dispose();

        }
    }
}
