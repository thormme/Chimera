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

        private List<List<string>> mAlphaLayers = new List<List<string>>();
        private List<Texture2D> mAlphaMaps = new List<Texture2D>();
        private List<Microsoft.Xna.Framework.Color[]> mVertices = new List<Microsoft.Xna.Framework.Color[]>();

        public TextureMap(Texture2D[] heightTextures, string[] textureNames, GraphicsDevice device)
        {
            mDevice = device;
            mAlphaMaps = heightTextures.ToList<Texture2D>();
            MakeLayers(textureNames);
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
                        tempVertices[z + x * tempMap.Width] = copy.mVertices[map][z + x * tempMap.Width];
                    }
                }
                mVertices.Add(tempVertices);
            }
        }

        private void MakeLayers(string[] textureNames)
        {
            mAlphaLayers.Add(new List<string>());
            int layerNumber = 0;
            for (int textureNumber = 0; textureNumber < textureNames.Length; textureNumber++)
            {
                if (mAlphaLayers[layerNumber].Count >= 4)
                {
                    mAlphaLayers.Add(new List<string>());
                    layerNumber++;
                }
                mAlphaLayers[layerNumber].Add(textureNames[textureNumber]);
            }
        }

        private void LoadData()
        {
            for (int map = 0; map < mAlphaMaps.Count; map++)
            {
                Microsoft.Xna.Framework.Color[] tempVertices = new Microsoft.Xna.Framework.Color[mAlphaMaps[map].Width * mAlphaMaps[map].Height];
                for (int z = 0; z < mAlphaMaps[map].Height; z++)
                {
                    for (int x = 0; x < mAlphaMaps[map].Width; x++)
                    {
                        tempVertices[z + x * mAlphaMaps[map].Width] = Utils.GetTexture2DPixelColor(x, z, mAlphaMaps[map]);
                    }
                }
            }
        }

        public void ModifyVertices(Vector3 position, string texture, int radius, float alpha)
        {
            
            radius *= Utils.AlphaMapScale;

            int layerNumber = 0;
            int textureNumber = 0;
            bool textureFound = false;

            for (; layerNumber < mAlphaLayers.Count; layerNumber++)
            {
                for (; textureNumber < mAlphaLayers[layerNumber].Count; textureNumber++)
                {
                    if (mAlphaLayers[layerNumber][textureNumber] == texture)
                    {
                        textureFound = true;
                        break;
                    }
                }
            }

            if (!textureFound)
            {
                if (mAlphaLayers[mAlphaLayers.Count - 1].Count >= 4)
                {
                    mAlphaLayers.Add(new List<string>());
                }
                mAlphaLayers[mAlphaLayers.Count - 1].Add(texture);
                layerNumber = mAlphaLayers.Count - 1;
                textureNumber = mAlphaLayers[layerNumber].Count - 1;
            }

            // Adjust for the rendered location of the terrain
            position.X /= (Utils.WorldScale.X * (1.0f / (float)Utils.AlphaMapScale));
            position.Z /= (Utils.WorldScale.Z * (1.0f / (float)Utils.AlphaMapScale));
            position.X += mAlphaMaps[layerNumber].Width / 2;
            position.Z += mAlphaMaps[layerNumber].Height / 2;

            for (int z = (int)position.Z - radius; z <= (int)position.Z + radius; z++)
            {
                for (int x = (int)position.X - radius; x <= (int)position.X + radius; x++)
                {
                    if (x < 0 || x >= mAlphaMaps[layerNumber].Width || z < 0 || z >= mAlphaMaps[layerNumber].Height) continue;
                    int distance = (int)Math.Sqrt(Math.Pow((x - position.X), 2) + Math.Pow((z - position.Z), 2));
                    if (distance < radius)
                    {
                        if (textureNumber == 0)
                        {
                            mVertices[layerNumber][z + x * mAlphaMaps[layerNumber].Width] = new Microsoft.Xna.Framework.Color(0, 0, 0, 255);
                        }
                        else if (textureNumber == 1)
                        {
                            mVertices[layerNumber][z + x * mAlphaMaps[layerNumber].Width] = new Microsoft.Xna.Framework.Color(255, 0, 0, 255);
                        }
                        else if (textureNumber == 2)
                        {
                            mVertices[layerNumber][z + x * mAlphaMaps[layerNumber].Width] = new Microsoft.Xna.Framework.Color(0, 255, 0, 255);
                        }
                        else if (textureNumber == 3)
                        {
                            mVertices[layerNumber][z + x * mAlphaMaps[layerNumber].Width] = new Microsoft.Xna.Framework.Color(0, 0, 255, 255);
                        }
                    }
                }
            }
            mAlphaMaps[layerNumber].SetData(mVertices[layerNumber]);
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
                            mVertices[map][z + x * mAlphaMaps[map].Width].R,
                            mVertices[map][z + x * mAlphaMaps[map].Width].G,
                            mVertices[map][z + x * mAlphaMaps[map].Width].B, 
                            mVertices[map][z + x * mAlphaMaps[map].Width].A));
                    }
                }

                bmp.Save(path + "/" + "AlphaMap" + map + ".bmp");
            }
        }
    }
}