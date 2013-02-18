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
        private List<Microsoft.Xna.Framework.Color[]> mTexelWeights = new List<Microsoft.Xna.Framework.Color[]>();
        private List<string> mTextures;

        public TextureMap(List<Texture2D> alphaMaps, List<string> textureNames, GraphicsDevice device)
        {
            mDevice = device;
            mAlphaMaps = alphaMaps;
            mTextures = textureNames;
            LoadData();
        }

        public TextureMap(TextureMap copy)
        {
            mDevice = copy.mDevice;
            for (int map = 0; map < copy.mAlphaMaps.Count; map++)
            {
                Texture2D tempMap = new Texture2D(copy.mAlphaMaps[map].GraphicsDevice, copy.mAlphaMaps[map].Width, copy.mAlphaMaps[map].Height);
                Microsoft.Xna.Framework.Color[] destinationColorData = new Microsoft.Xna.Framework.Color[copy.mAlphaMaps[map].Width * copy.mAlphaMaps[map].Height];
                copy.mAlphaMaps[map].GetData<Microsoft.Xna.Framework.Color>(destinationColorData);
                tempMap.SetData<Microsoft.Xna.Framework.Color>(destinationColorData);
                mAlphaMaps.Add(tempMap);
                Microsoft.Xna.Framework.Color[] tempVertices = new Microsoft.Xna.Framework.Color[tempMap.Width * tempMap.Height];
                for (int z = 0; z < tempMap.Height; z++)
                {
                    for (int x = 0; x < tempMap.Width; x++)
                    {
                        tempVertices[z + x * tempMap.Width] = copy.mTexelWeights[map][z + x * tempMap.Width];
                    }
                }
                mTexelWeights.Add(tempVertices);
            }
        }

        private void LoadData()
        {
            for (int map = 0; map < mAlphaMaps.Count; map++)
            {
                int width = mAlphaMaps[map].Width;
                int height = mAlphaMaps[map].Height;

                Microsoft.Xna.Framework.Color[] tempVertices = new Microsoft.Xna.Framework.Color[width * height];
                for (int z = 0; z < height; z++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        tempVertices[z + x * width] = Utils.GetTexture2DPixelColor(x, z, mAlphaMaps[map]);
                    }
                }
                mTexelWeights.Add(tempVertices);
            }
        }

        public void ModifyTexelWeights(Vector3 position, string texture, int radius, float alpha)
        {
            
            radius *= Utils.AlphaMapScale;

            if (!mTextures.Contains(texture))
            {
                int oldLength = mTextures.Count;
                mTextures.Add(texture);
                if (oldLength % 3 > mTextures.Count % 3)
                {
                    mAlphaMaps.Add(new Texture2D(GraphicsManager.Device, mAlphaMaps[mAlphaMaps.Count - 1].Width, mAlphaMaps[mAlphaMaps.Count - 1].Height));
                    mTexelWeights.Add(new Microsoft.Xna.Framework.Color[mAlphaMaps[mAlphaMaps.Count - 1].Width * mAlphaMaps[mAlphaMaps.Count - 1].Height]);
                }
            }

            int alphaMapIndex = mTextures.IndexOf(texture) / 3;

            int width = mAlphaMaps[alphaMapIndex].Width;
            int height = mAlphaMaps[alphaMapIndex].Height;

            // Adjust for the rendered location of the terrain
            position.X /= (Utils.WorldScale.X * (1.0f / (float)Utils.AlphaMapScale));
            position.Z /= (Utils.WorldScale.Z * (1.0f / (float)Utils.AlphaMapScale));
            position.X += width / 2;
            position.Z += height / 2;

            int dirtyMaps = -1;
            for (int z = (int)position.Z - radius; z <= (int)position.Z + radius; z++)
            {
                for (int x = (int)position.X - radius; x <= (int)position.X + radius; x++)
                {
                    if (x < 0 || x >= width || z < 0 || z >= height) continue;
                    int distance = (int)Math.Sqrt(Math.Pow((x - position.X), 2) + Math.Pow((z - position.Z), 2));
                    if (distance < radius)
                    {
                        for (int j = alphaMapIndex + 1; j < mAlphaMaps.Count; j++)
                        {
                            if (dirtyMaps < 0 && mTexelWeights[j][z * width + x] != Microsoft.Xna.Framework.Color.Black)
                            {
                                dirtyMaps = j;
                            }
                            mTexelWeights[j][z * width + x] = new Microsoft.Xna.Framework.Color(0.0f, 0.0f, 0.0f, 1.0f);
                        }

                        int colorIndex = mTextures.IndexOf(texture) % 3;
                        switch (colorIndex)
                        {
                            case 0:
                                mTexelWeights[alphaMapIndex][z * width + x].R = 255;
                                mTexelWeights[alphaMapIndex][z * width + x].G = 0;
                                mTexelWeights[alphaMapIndex][z * width + x].B = 0;
                                break;
                            case 1:
                                mTexelWeights[alphaMapIndex][z * width + x].G = 255;
                                mTexelWeights[alphaMapIndex][z * width + x].B = 0;
                                break;
                            case 2:
                                mTexelWeights[alphaMapIndex][z * width + x].B = 255;
                                break;
                        }
                    }
                }
            }
            mAlphaMaps[alphaMapIndex].SetData(mTexelWeights[alphaMapIndex]);

            if (dirtyMaps >= 0)
            {
                for (int i = 0; i < mAlphaMaps.Count; i++)
                {
                    mAlphaMaps[i].SetData(mTexelWeights[i]);
                }
            }
        }


        public void Save(string path)
        {
            path += "/AlphaMaps";
            System.IO.Directory.CreateDirectory(path);

            for (int map = 0; map < mAlphaMaps.Count; map++)
            {
                Bitmap bmp = new Bitmap(mAlphaMaps[map].Width, mAlphaMaps[map].Height);
                for (int z = 0; z < mAlphaMaps[map].Height; z++)
                {
                    for (int x = 0; x < mAlphaMaps[map].Width; x++)
                    {
                        bmp.SetPixel(x, z, System.Drawing.Color.FromArgb(
                            mTexelWeights[map][z + x * mAlphaMaps[map].Width].R,
                            mTexelWeights[map][z + x * mAlphaMaps[map].Width].G,
                            mTexelWeights[map][z + x * mAlphaMaps[map].Width].B,
                            mTexelWeights[map][z + x * mAlphaMaps[map].Width].A));
                    }
                }

                bmp.Save(path + "/" + "AlphaMap" + map + ".bmp");
            }
        }
    }
}