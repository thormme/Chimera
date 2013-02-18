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

        private Texture2D mTexture;
        public Texture2D Texture
        {
            get { return this.mTexture; }
            set { this.mTexture = value; }
        }

        private string[] mTextureNames;
        public string[] TextureNames
        {
            get { return this.mTextureNames; }
            set { this.mTextureNames = value; }
        }

        public TerrainDescription(TerrainHeightMap terrain, Texture2D texture, string[] textureNames)
        {
            this.mTerrain      = terrain;
            this.mTexture      = texture;
            this.mTextureNames = textureNames;
        }
    }
}
