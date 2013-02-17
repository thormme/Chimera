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

    public class TextureMap
    {

        private GraphicsDevice mDevice = null;

        private Texture2D mMap = null;

        private int mWidth = 0;
        private int mHeight = 0;

        private int mBootTexture = 0;
        private string[] mTextures = new string[4];

        Microsoft.Xna.Framework.Color[,] vertices;

        public TextureMap(int width, int height, GraphicsDevice device)
        {
            mDevice = device;
            mMap = new Texture2D(device, width, height);
            mWidth = mMap.Width;
            mHeight = mMap.Height;
            LoadData(true);
        }

        public TextureMap(Texture2D heightTexture, GraphicsDevice device)
        {
            mDevice = device;
            mMap = heightTexture;
            mWidth = mMap.Width;
            mHeight = mMap.Height;
            LoadData(false);
        }

        public TextureMap(TextureMap copy)
        {
            FixTextureMap(copy);
        }

        public void LoadData(bool resized)
        {
            MakeVertices(resized);
        }

        private void MakeVertices(bool resized)
        {
            vertices = new Microsoft.Xna.Framework.Color[mWidth, mHeight];
            for (int z = 0; z < mHeight; z++)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    if (resized) // If resized rebuild map
                    {
                        vertices[x, z] = new Microsoft.Xna.Framework.Color(0.0f, 0.0f, 0.0f, 0.0f);
                    }
                    else // Otherwise create vertices
                    {
                        vertices[x, z] = Utils.GetTexture2DPixelColor(x, z, mMap);
                    }
                }
            }
        }
        
        public void FixTextureMap(TextureMap copy)
        {
            mDevice = copy.mDevice;
            mMap = new Texture2D(copy.mMap.GraphicsDevice, copy.mWidth, copy.mHeight);
            Microsoft.Xna.Framework.Color[] destinationColorData = new Microsoft.Xna.Framework.Color[copy.mWidth * copy.mHeight];
            copy.mMap.GetData<Microsoft.Xna.Framework.Color>(destinationColorData);
            mMap.SetData<Microsoft.Xna.Framework.Color>(destinationColorData);
            mWidth = copy.mWidth;
            mHeight = copy.mHeight;
            vertices = new Microsoft.Xna.Framework.Color[mWidth, mHeight];
            for (int z = 0; z < mHeight; z++)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    vertices[x, z] = copy.vertices[x, z];
                }
            }
        }

        public void ModifyVertices(Vector3 position, string texture, int radius, float alpha)
        {
            if (!mTextures.Contains(texture))
            {
                
                mTextures[mBootTexture] = texture;

                mBootTexture++;
                if (mBootTexture > 3)
                {
                    mBootTexture = 0;
                }

            }

            for (int z = (int)position.Z - radius; z <= (int)position.Z + radius; z++)
            {
                for (int x = (int)position.X - radius; x <= (int)position.X + radius; x++)
                {
                    if (x < 0 || x >= mWidth || z < 0 || z >= mHeight) continue;
                    int distance = (int)Math.Sqrt(Math.Pow((x - position.X), 2) + Math.Pow((z - position.Z), 2));
                    if (distance < radius)
                    {
                        if (mTextures[0] == texture)
                        {
                            vertices[x, z] = Microsoft.Xna.Framework.Color.Black;
                        }
                        else if (mTextures[1] == texture)
                        {
                            vertices[x, z] = Microsoft.Xna.Framework.Color.Red;
                        }
                        else if (mTextures[2] == texture)
                        {
                            vertices[x, z] = Microsoft.Xna.Framework.Color.Green;
                        }
                        else if (mTextures[3] == texture)
                        {
                            vertices[x, z] = Microsoft.Xna.Framework.Color.Blue;
                        }
                    }
                }
            }
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

        public void Save(string fileName)
        {

            Bitmap bmp = new Bitmap(mMap.Width, mMap.Height);

            // Update mMap with modifications
            for (int z = 0; z < mHeight; z++)
            {
                for (int x = 0; x < mWidth; x++)
                {
                    bmp.SetPixel(x, z, System.Drawing.Color.FromArgb(vertices[x, z].R, vertices[x, z].G, vertices[x, z].B, vertices[x, z].A));
                }
            }

            bmp.Save(DirectoryManager.GetRoot() + "Chimera/ChimeraContent/textures/sprites/" + fileName + "_texture" + ".bmp");
        }
    }
}