using GameConstructLibrary;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GraphicsLibrary
{
    class TerrainDescription
    {
        private TerrainHeightMap mTerrain;
        public TerrainHeightMap Terrain
        {
            get { return this.mTerrain; }
            set { this.mTerrain = value; }
        }

        private Texture2D mTextureLow;
        public Texture2D TextureLow
        {
            get { return this.mTextureLow; }
            set { this.mTextureLow = value; }
        }

        private Texture2D mTextureMedium;
        public Texture2D TextureMedium
        {
            get { return this.mTextureMedium; }
            set { this.mTextureMedium = value; }
        }

        private Texture2D mTextureHigh;
        public Texture2D TextureHigh
        {
            get { return this.mTextureHigh; }
            set { this.mTextureHigh = value; }
        }

        private float mHeightLow;
        public float HeightLow
        {
            get { return this.mHeightLow; }
            set { this.mHeightLow = value; }
        }

        private float mHeightMedium;
        public float HeightMedium
        {
            get { return this.mHeightMedium; }
            set { this.mHeightMedium = value; }
        }

        private float mHeightHigh;
        public float HeightHigh
        {
            get { return this.mHeightHigh; }
            set { this.mHeightHigh = value; }
        }

        public TerrainDescription(TerrainHeightMap terrain, List<Texture2D> textures, List<float> heights)
        {
            this.mTerrain = terrain;

            mTextureLow    = null;
            mTextureMedium = null;
            mTextureHigh   = null;

            mHeightLow = -1.0f;
            mHeightMedium = -1.0f;
            mHeightHigh = -1.0f;

            int length = textures.Count;
            if (length > 0)
            {
                mTextureLow = textures[0];
                mHeightLow = heights[0];
            }
            if (length > 1)
            {
                mTextureMedium = textures[1];
                mHeightMedium = heights[1];
            }
            if (length > 2)
            {
                mTextureHigh = textures[2];
                mHeightHigh = heights[2];
            }
        }
    }
}
