using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BEPUphysics.MathExtensions;
using Microsoft.Xna.Framework;
using GraphicsLibrary;
using BEPUphysics;
using BEPUphysics.Collidables;

namespace GameConstructLibrary
{
    /// <summary>
    /// Physical and visual representation of a terrain heightmap.
    /// </summary>
    public class TerrainPhysics : Terrain
    {
        private TerrainRenderable mTerrainRenderable;

        /// <summary>
        /// Constuct a TerrainPhysics entity.
        /// </summary>
        /// <param name="terrainName">The name of the terrain.</param>
        /// <param name="scale">The amount to scale the terrain</param>
        /// <param name="orientation">The orientation of the terrain.</param>
        /// <param name="translation">The position of the terrain.</param>
        TerrainPhysics(String terrainName, float scale, Quaternion orientation, Vector3 translation)
            : base(
                GraphicsManager.LookupTerrain(terrainName).Terrain.GetHeights(), 
                new AffineTransform(new Vector3(scale), orientation, translation)
            )
        {
            mTerrainRenderable = new TerrainRenderable(terrainName);
        }

        /// <summary>
        /// Render the terrain to screen.
        /// </summary>
        public void Render()
        {
            mTerrainRenderable.Render(WorldTransform.Matrix);
        }
    }
}
