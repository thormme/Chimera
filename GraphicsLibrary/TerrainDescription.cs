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

        private TerrainTexture mTexture;
        public TerrainTexture Texture
        {
            get { return this.mTexture; }
            set { this.mTexture = value; }
        }

        public TerrainDescription(TerrainHeightMap terrain, TerrainTexture texture)
        {
            this.mTerrain = terrain;
            this.mTexture = texture;
        }
    }
}
