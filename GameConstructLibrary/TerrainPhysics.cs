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
    public class TerrainPhysics : IGameObject, IStaticCollidableOwner
    {
        private TerrainRenderable mTerrainRenderable;

        /// <summary>
        /// Constuct a TerrainPhysics entity.
        /// </summary>
        /// <param name="terrainName">The name of the terrain.</param>
        /// <param name="scale">The amount to scale the terrain</param>
        /// <param name="orientation">The orientation of the terrain.</param>
        /// <param name="translation">The position of the terrain.</param>
        public TerrainPhysics(String terrainName, Vector3 scale, Quaternion orientation, Vector3 translation)
        {
            float[,] heights = GraphicsManager.LookupTerrainHeightMap(terrainName).GetHeights();
            StaticCollidable = new Terrain(
                heights,
                new AffineTransform(scale, orientation, translation - new Vector3(scale.X * .5f * heights.GetLength(0), 0f, scale.Z * .5f * heights.GetLength(1)))
            );

            StaticCollidable.Tag = this;

            Position = translation;
            XNAOrientationMatrix = Matrix.CreateFromQuaternion(orientation);
            Scale = scale;

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

        public Vector3 Scale
        {
            get;
            private set;
        }

        public StaticCollidable StaticCollidable
        {
            get;
            protected set;
        }
    }
}
