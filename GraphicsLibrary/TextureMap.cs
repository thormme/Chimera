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

        private List<Texture2D> mAlphaMaps = new List<Texture2D>();
        private List<Microsoft.Xna.Framework.Color[]> texelWeights = new List<Microsoft.Xna.Framework.Color[]>();
        private List<string> mTextures;

        private int mWidth = 0;
        private int mHeight = 0;

        public TextureMap(int width, int height, GraphicsDevice device)
        {
            mDevice = device;
            mAlphaMaps.Add(new Texture2D(device, width, height));
            mTextures = new List<string>();
            mTextures.Add("default_terrain_detail");
            mWidth = width;
            mHeight = height;
            LoadData(true);
        }

        public TextureMap(List<Texture2D> alphaMaps, List<string> textureNames, GraphicsDevice device)
        {
            mDevice = device;
            mAlphaMaps = alphaMaps;
            mWidth = alphaMaps[0].Width;
            mHeight = alphaMaps[0].Height;
            mTextures = textureNames;
            LoadData(false);
        }

        public TextureMap(TextureMap copy)
        {
            FixTextureMap(copy);
        }

        public void LoadData(bool resized)
        {
            ConstructTexelWeights(resized);
        }

        private void ConstructTexelWeights(bool resized)
        {
            for (int i = 0; i < mAlphaMaps.Count; i++)
            {
                texelWeights.Add(new Microsoft.Xna.Framework.Color[mWidth * mHeight]);
                for (int z = 0; z < mHeight; z++)
                {
                    for (int x = 0; x < mWidth; x++)
                    {
                        if (resized) // If resized rebuild map
                        {
                            texelWeights[i][x * mWidth + z] = new Microsoft.Xna.Framework.Color(0.0f, 0.0f, 0.0f, 1.0f);
                        }
                        else // Otherwise create vertices
                        {
                            texelWeights[i][x * mWidth + z] = Utils.GetTexture2DPixelColor(x, z, mAlphaMaps[i]);
                        }
                    }
                }
            }
        }
        
        public void FixTextureMap(TextureMap copy)
        {
            mDevice = copy.mDevice;
            mWidth = copy.mWidth;
            mHeight = copy.mHeight;

            mAlphaMaps.Clear();
            for (int i = 0; i < copy.mAlphaMaps.Count; i++)
            {
                mAlphaMaps.Add(new Texture2D(mDevice, mWidth, mHeight));
                Microsoft.Xna.Framework.Color[] destinationColorData = new Microsoft.Xna.Framework.Color[mWidth * mHeight];
                copy.mAlphaMaps[i].GetData<Microsoft.Xna.Framework.Color>(destinationColorData);
                mAlphaMaps[i].SetData<Microsoft.Xna.Framework.Color>(destinationColorData);
            }

            texelWeights.Clear();
            for (int i = 0; i < copy.texelWeights.Count; i++)
            {
                texelWeights.Add(new Microsoft.Xna.Framework.Color[mWidth * mHeight]);
                for (int z = 0; z < mHeight; z++)
                {
                    for (int x = 0; x < mWidth; x++)
                    {
                        texelWeights[i][x * mWidth + z] = copy.texelWeights[i][x * mWidth + z];
                    }
                }
            }
        }

        public void ModifyTexelWeights(Vector3 position, string texture, int radius, float alpha)
        {
            radius *= 10;

            Console.WriteLine("cursor at: " + position);
            if (!mTextures.Contains(texture))
            {
                int oldLength = mTextures.Count;
                mTextures.Add(texture);
                if (oldLength % 3 > mTextures.Count % 3)
                {
                    mAlphaMaps.Add(new Texture2D(GraphicsManager.Device, mWidth, mHeight));
                    texelWeights.Add(new Microsoft.Xna.Framework.Color[mWidth * mHeight]);
                }
            }

            // Adjust for the rendered location of the terrain
            position.X /= (Utils.WorldScale.X * 0.1f);
            position.Z /= (Utils.WorldScale.Z * 0.1f);
            position.X += mWidth / 2;
            position.Z += mHeight / 2;

            int alphaMapIndex = mTextures.IndexOf(texture) / 3;

            int dirtyMaps = -1;
            for (int z = (int)position.Z - radius; z <= (int)position.Z + radius; z++)
            {
                for (int x = (int)position.X - radius; x <= (int)position.X + radius; x++)
                {
                    if (x < 0 || x >= mWidth || z < 0 || z >= mHeight) continue;
                    int distance = (int)Math.Sqrt(Math.Pow((x - position.X), 2) + Math.Pow((z - position.Z), 2));
                    if (distance < radius)
                    {
                        for (int j = alphaMapIndex + 1; j < mAlphaMaps.Count; j++)
                        {
                            if (dirtyMaps < 0 && texelWeights[j][z * mWidth + x] != Microsoft.Xna.Framework.Color.Black)
                            {
                                dirtyMaps = j;
                            }
                            texelWeights[j][z * mWidth + x] = new Microsoft.Xna.Framework.Color(0.0f, 0.0f, 0.0f, 1.0f);
                        }

                        int colorIndex = mTextures.IndexOf(texture) % 3;
                        switch (colorIndex)
                        {
                            case 0:
                                texelWeights[alphaMapIndex][z * mWidth + x].R = 255;
                                texelWeights[alphaMapIndex][z * mWidth + x].G = 0;
                                texelWeights[alphaMapIndex][z * mWidth + x].B = 0;
                                break;
                            case 1:
                                texelWeights[alphaMapIndex][z * mWidth + x].G = 255;
                                texelWeights[alphaMapIndex][z * mWidth + x].B = 0;
                                break;
                            case 2:
                                texelWeights[alphaMapIndex][z * mWidth + x].B = 255;
                                break;
                        }
                    }
                }
            }
            mAlphaMaps[alphaMapIndex].SetData(texelWeights[alphaMapIndex]);

            if (dirtyMaps >= 0)
            {
                for (int i = 0; i < mAlphaMaps.Count; i++)
                {
                    mAlphaMaps[i].SetData(texelWeights[i]);
                }
            }
        }

        public void Resize(int width, int height)
        {
            mWidth = width;
            mHeight = height;

            Microsoft.Xna.Framework.Rectangle sourceRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, mWidth, mHeight);
            Microsoft.Xna.Framework.Color[] retrievedColor = new Microsoft.Xna.Framework.Color[mWidth * mHeight];

            for (int i = 0; i < mAlphaMaps.Count; i++)
            {
                mAlphaMaps[i].GetData<Microsoft.Xna.Framework.Color>(
                    0,
                    sourceRectangle,
                    retrievedColor,
                    0,
                    mWidth * mHeight);

                mAlphaMaps[i] = new Texture2D(mDevice, mWidth, mHeight);
                mAlphaMaps[i].SetData<Microsoft.Xna.Framework.Color>(retrievedColor);
            }

            LoadData(true);
        }

        public void Save(string fileName)
        {
            //for (int i = 0; i < texelWeights.Count; i++)
            //{
            //    Bitmap bmp = new Bitmap(mWidth, mHeight);

            //     Update mMap with modifications
            //    for (int z = 0; z < mHeight; z++)
            //    {
            //        for (int x = 0; x < mWidth; x++)
            //        {
            //            bmp.SetPixel(x, z, System.Drawing.Color.FromArgb(texelWeights[i][x * mWidth + z].R, texelWeights[i][x * mWidth + z].G, texelWeights[i][x * mWidth + z].B, texelWeights[i][x * mWidth + z].A));
            //        }
            //    }
            //    bmp.Save(DirectoryManager.GetRoot() + "Chimera/ChimeraContent/textures/sprites/" + fileName + "_alphamap_" + i + ".bmp");
            //}

            //Texture2D terrainComposite = GraphicsManager.LookupTerrainTexture(mName);

            //Bitmap composite = new Bitmap(mWidth, mHeight);
            //for (int z = 0; z < mHeight; z++)
            //{
            //    for (int x = 0; x < mWidth; x++)
            //    {
            //        composite.SetPixel(x, z, terrainComposite.
            //    }
            //}
            //composite.Save(DirectoryManager.GetRoot() + "Chimera/ChimeraContent/levels/maps/" + fileName + "_texture.bmp");
        }
    }
}