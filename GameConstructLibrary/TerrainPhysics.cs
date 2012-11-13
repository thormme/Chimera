﻿using System;
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
    public class TerrainPhysics : Terrain, IGameObject
    {
        private TerrainRenderable mTerrainRenderable;

        /// <summary>
        /// Constuct a TerrainPhysics entity.
        /// </summary>
        /// <param name="terrainName">The name of the terrain.</param>
        /// <param name="scale">The amount to scale the terrain</param>
        /// <param name="orientation">The orientation of the terrain.</param>
        /// <param name="translation">The position of the terrain.</param>
        public TerrainPhysics(String terrainName, float scale, Quaternion orientation, Vector3 translation)
            : base(
                GraphicsManager.LookupTerrainHeightMap(terrainName).GetHeights(), 
                new AffineTransform(new Vector3(scale*1000.0f, 1.0f, 1000.0f), orientation, translation)
            )
        {
            Position = translation;
            XNAOrientationMatrix = Matrix.CreateFromQuaternion(orientation);
            Scale = scale/100.0f;

            mTerrainRenderable = new TerrainRenderable(terrainName);
        }

        /// <summary>
        /// Render the terrain to screen.
        /// </summary>
        public void Render()
        {
            mTerrainRenderable.Render(Position, XNAOrientationMatrix, Scale);
        }

        public Vector3 Position
        {
            get;
            private set;
        }

        public Matrix XNAOrientationMatrix
        {
            get;
            private set;
        }

        public float Scale
        {
            get;
            private set;
        }
    }
}