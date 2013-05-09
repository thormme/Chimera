using BEPUphysics;
using GameConstructLibrary;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldEditor
{
    public class ModifiableLevel : Level
    {
        #region Variables

        private Space mSpace;

        #endregion

        #region Public Interface

        public ModifiableLevel(string name, Space space)
            : base(name)
        {
            mSpace = space;
        }

        public ModifiableLevel(Level level, Space space)
            : base(level.Name)
        {
            mSpace = space;
            foreach (Vector3 block in level.Blocks.Keys)
            {
                AddNewBlock(block);
            }
        }

        public override void AddNewBlock(Vector3 blockCoordinate)
        {
            base.AddNewBlock(blockCoordinate);

            AttachMesh(blockCoordinate, new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST });
            mBlocks[blockCoordinate].HeightMap.UpdateStaticCollidable();
        }

        #endregion

        #region Enums and Constants

        private enum Direction { NORTH, EAST, SOUTH, WEST };
        
        // NORTH, EAST, SOUTH, WEST
        private Vector3[] mNeighborOffsets    = new Vector3[] { -Vector3.UnitZ, Vector3.UnitX, Vector3.UnitZ, -Vector3.UnitX };
        private Vector2[] mSinkStartIndices   = new Vector2[] {  Vector2.Zero , Vector2.UnitX, Vector2.UnitY,  Vector2.Zero  };
        private Vector2[] mSourceStartIndices = new Vector2[] {  Vector2.UnitY, Vector2.Zero , Vector2.Zero ,  Vector2.UnitX };
        private Vector2[] mIndexIncrements    = new Vector2[] {  Vector2.UnitX, Vector2.UnitY, Vector2.UnitX,  Vector2.UnitY };

        #endregion

        #region Block Modifiers

        public void RaiseTerrain(Vector3 center, float radius, float deltaHeight)
        {
            VertexModifier raise = RaiseVertex;
            IterateOverTerrainVertices(center, radius, raise, deltaHeight);
        }

        public void LowerTerrain(Vector3 center, float radius, float deltaHeight)
        {
            VertexModifier lower = LowerVertex;
            IterateOverTerrainVertices(center, radius, lower, deltaHeight);
        }

        public void SetTerrain(Vector3 center, float radius, float deltaHeight)
        {
            VertexModifier set = SetVertex;
            IterateOverTerrainVertices(center, radius, set, deltaHeight);
        }

        public void SmoothTerrain(Vector3 center, float radius, float deltaHeight)
        {
            VertexModifier smooth = SmoothVertex;
            IterateOverTerrainVertices(center, radius, smooth, deltaHeight);
        }

        public void IterateOverBlocksInRadius(Vector3 position, float radius, BlockModifier modifier, object[] parameters)
        {
            IterateOverBlocksInContainerInRadius(null, position, radius, modifier, parameters);
        }

        public void IterateOverEveryBlock(BlockModifier modifier, object[] parameters)
        {
            foreach (LevelBlock block in mBlocks.Values)
            {
                modifier(block, new Vector3(), 0, parameters);
            }
        }

        public void IterateOverBlocksInContainer(IEnumerable<Vector3> container, BlockModifier modifier, object[] parameters)
        {
            foreach (Vector3 coordinate in container)
            {
                if (!mBlocks.ContainsKey(coordinate))
                {
                    continue;
                }

                modifier(mBlocks[coordinate], new Vector3(), 0, parameters);
            }
        }

        public void IterateOverBlocksInContainerInRadius(IEnumerable<Vector3> container, Vector3 position, float radius, BlockModifier modifier, object[] parameters)
        {
            Vector3 offset = new Vector3(radius, 0, radius);
            Vector3 minCoordinate = CoordinateFromPosition(position - offset);
            Vector3 maxCoordinate = CoordinateFromPosition(position + offset);

            for (int z = (int)minCoordinate.Z; z <= (int)maxCoordinate.Z; z++)
            {
                for (int x = (int)minCoordinate.X; x <= (int)maxCoordinate.X; x++)
                {
                    Vector3 coordinate = new Vector3(x, minCoordinate.Y, z);
                    if (container == null || container.Contains(coordinate))
                    {
                        LevelBlock block;
                        if (mBlocks.TryGetValue(coordinate, out block))
                        {
                            modifier(block, (position - coordinate * BLOCK_SIZE) / BLOCK_SIZE, radius / BLOCK_SIZE, parameters);
                        }
                    }
                }
            }
        }

        public void ModifySingleBlock(Vector3 coordinate, BlockModifier modifier, object[] parameters)
        {
            LevelBlock block;
            if (mBlocks.TryGetValue(coordinate, out block))
            {
                modifier(block, new Vector3(), 0, parameters);
            }
        }

        public delegate void BlockModifier(LevelBlock block, Vector3 centerCoordinate, float radius, object[] parameters);

        public delegate void VertexModifier(Vector2 positionVertexSpace, float height);

        #endregion

        #region Helper Methods

        private Vector3 CoordinateFromPosition(Vector3 position)
        {
            return new Vector3((float)Math.Floor(position.X / BLOCK_SIZE),
                (float)Math.Floor(position.Y / BLOCK_SIZE + 0.00001f),
                (float)Math.Floor(position.Z / BLOCK_SIZE));
        }

        private Tuple<Vector3, Vector2> VertexAtPosition(Vector2 positionVertexSpace)
        {
            Vector3 coordinate = new Vector3((float)Math.Floor(positionVertexSpace.X / HeightMapMesh.NUM_SIDE_VERTICES), 0.0f, (float)Math.Floor(positionVertexSpace.Y / HeightMapMesh.NUM_SIDE_VERTICES));

            Vector2 verticesIndex = positionVertexSpace - new Vector2(coordinate.X, coordinate.Z) * HeightMapMesh.NUM_SIDE_VERTICES;

            if (mBlocks.ContainsKey(coordinate))
            {
                return new Tuple<Vector3, Vector2>(coordinate, verticesIndex);
            }

            return null;
        }

        private void AttachMesh(Vector3 coordinate, Direction[] attachmentSides)
        {
            HeightMapMesh sink = AssetLibrary.LookupHeightMap(mName + coordinate.ToString()).Mesh;

            foreach (Direction side in attachmentSides)
            {
                Vector3 neighborCoordinate = coordinate + mNeighborOffsets[(int)side];
                if (!mBlocks.ContainsKey(neighborCoordinate))
                {
                    continue;
                }

                HeightMapMesh source = AssetLibrary.LookupHeightMap(mName + neighborCoordinate.ToString()).Mesh;
                Vector2 sinkStartIndex = mSinkStartIndices[(int)side] * (HeightMapMesh.NUM_SIDE_VERTICES - 1);
                Vector2 sourceStartIndex = mSourceStartIndices[(int)side] * (HeightMapMesh.NUM_SIDE_VERTICES - 1);
                Vector2 indexIncrement = mIndexIncrements[(int)side];

                for (int count = 0; count < HeightMapMesh.NUM_SIDE_VERTICES; count++)
                {
                    Vector2 sinkIndex = sinkStartIndex + count * indexIncrement;
                    Vector2 sourceIndex = sourceStartIndex + count * indexIncrement;

                    Vector3 normal = Vector3.Normalize(sink.GetVertexNormal(sinkIndex) + source.GetVertexNormal(sourceIndex));

                    sink.SetVertexHeight(sinkIndex, source.GetVertexHeight(sourceIndex));
                    sink.SetVertexNormal(sinkIndex, normal);
                    source.SetVertexNormal(sourceIndex, normal);
                }
            }
        }

        private void RaiseVertex(Vector2 positionVertexSpace, float deltaHeight)
        {
            Tuple<Vector3, Vector2> vertex = VertexAtPosition(positionVertexSpace);
            if (vertex != null)
            {
                HeightMapMesh mesh = AssetLibrary.LookupHeightMap(mName + vertex.Item1.ToString()).Mesh;

                mesh.SetVertexHeight(vertex.Item2, mesh.GetVertexHeight(vertex.Item2) + deltaHeight);
            }
        }

        private void LowerVertex(Vector2 positionVertexSpace, float deltaHeight)
        {
            Tuple<Vector3, Vector2> vertex = VertexAtPosition(positionVertexSpace);
            if (vertex != null)
            {
                HeightMapMesh mesh = AssetLibrary.LookupHeightMap(mName + vertex.Item1.ToString()).Mesh;

                mesh.SetVertexHeight(vertex.Item2, mesh.GetVertexHeight(vertex.Item2) - deltaHeight);
            }
        }

        private void SetVertex(Vector2 positionVertexSpace, float newHeight)
        {
            Tuple<Vector3, Vector2> vertex = VertexAtPosition(positionVertexSpace);
            if (vertex != null)
            {
                HeightMapMesh mesh = AssetLibrary.LookupHeightMap(mName + vertex.Item1.ToString()).Mesh;

                mesh.SetVertexHeight(vertex.Item2, newHeight);
            }
        }

        private void SmoothVertex(Vector2 positionVertexSpace, float newHeight)
        {
            Tuple<Vector3, Vector2> centerVertex = VertexAtPosition(positionVertexSpace);
            if (centerVertex != null)
            {
                int count = 0;
                float neighborSum = 0.0f;

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            continue;
                        }

                        Vector2 neighborVertexSpace = new Vector2(positionVertexSpace.X + i, positionVertexSpace.Y + j);
                        Tuple<Vector3, Vector2> neighborVertex = VertexAtPosition(neighborVertexSpace);
                        if (neighborVertex != null)
                        {
                            neighborSum += AssetLibrary.LookupHeightMap(mName + neighborVertex.Item1.ToString()).Mesh.GetVertexHeight(neighborVertex.Item2);
                            count++;
                        }
                    }
                }

                HeightMapMesh mesh = AssetLibrary.LookupHeightMap(mName + centerVertex.Item1.ToString()).Mesh;
                float centerHeight = mesh.GetVertexHeight(centerVertex.Item2);
                neighborSum += centerHeight;
                count++;

                neighborSum /= (float)count;
                mesh.SetVertexHeight(centerVertex.Item2, centerHeight * 0.9f + neighborSum * 0.1f);
            }
        }

        private void IterateOverTerrainVertices(Vector3 center, float radius, VertexModifier modifier, float deltaHeight)
        {
            Vector2 centerVertexSpace = new Vector2(center.X / BLOCK_SIZE * HeightMapMesh.NUM_SIDE_VERTICES, center.Z / BLOCK_SIZE * HeightMapMesh.NUM_SIDE_VERTICES);

            float radiusVertexSpace = radius / BLOCK_SIZE * (float)Math.Sqrt(2 * HeightMapMesh.NUM_SIDE_VERTICES * HeightMapMesh.NUM_SIDE_VERTICES);
            float radiusSquared = radiusVertexSpace * radiusVertexSpace;

            int minX = (int)Math.Floor(centerVertexSpace.X - radiusVertexSpace), maxX = (int)Math.Ceiling(centerVertexSpace.X + radiusVertexSpace);
            int minZ = (int)Math.Floor(centerVertexSpace.Y - radiusVertexSpace), maxZ = (int)Math.Ceiling(centerVertexSpace.Y + radiusVertexSpace);

            for (int z = minZ; z <= maxZ; z++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    Vector2 positionVertexSpace = new Vector2(x, z);
                    if ((positionVertexSpace - centerVertexSpace).LengthSquared() <= radiusSquared)
                    {
                        modifier(positionVertexSpace, deltaHeight);
                    }
                }
            }

            Vector3 radiusOffset = new Vector3(radius, 0, radius);
            Vector3 minCoordinate = CoordinateFromPosition(center - radiusOffset);
            Vector3 maxCoordinate = CoordinateFromPosition(center + radiusOffset);

            for (int blockZ = (int)minCoordinate.Z; blockZ <= (int)maxCoordinate.Z; ++blockZ)
            {
                for (int blockX = (int)minCoordinate.X; blockX <= (int)maxCoordinate.X; ++blockX)
                {
                    Vector3 coordinate = new Vector3(blockX, minCoordinate.Y, blockZ);
                    if (!mBlocks.ContainsKey(coordinate))
                    {
                        continue;
                    }

                    AttachMesh(coordinate, new Direction[] { Direction.NORTH, Direction.EAST, Direction.SOUTH, Direction.WEST });

                    mSpace.Remove(mBlocks[coordinate].HeightMap.StaticCollidable);

                    mBlocks[coordinate].HeightMap.UpdateStaticCollidable();

                    mSpace.Add(mBlocks[coordinate].HeightMap.StaticCollidable);
                }
            }
        }

        #endregion
    }
}
