using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using GameConstructLibrary;

namespace GraphicsLibrary
{
    public class TerrainDescription
    {
        private TerrainHeightMap mTerrain;
        public TerrainHeightMap Terrain
        {
            get { return this.mTerrain; }
            set { this.mTerrain = value; }
        }

        private List<Texture2D> mAlphaMaps;
        public List<Texture2D> AlphaMaps
        {
            get { return this.mAlphaMaps; }
            set { this.mAlphaMaps = value; }
        }

        private Texture2D mTexture;
        public Texture2D Texture
        {
            get { return this.mTexture; }
            set { this.mTexture = value; }
        }

        private List<string> mTextureNames;
        public List<string> TextureNames
        {
            get { return this.mTextureNames; }
            set { this.mTextureNames = value; }
        }

        public TerrainDescription(TerrainHeightMap terrain, List<Texture2D> alphaMaps, List<string> textureNames)
        {
            this.mTerrain      = terrain;
            this.mAlphaMaps    = alphaMaps;
            this.mTextureNames = textureNames;
            this.mTexture      = null;
        }
    }
}
