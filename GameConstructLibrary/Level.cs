using GraphicsLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GameConstructLibrary
{
    public class Level
    {
        public const float BLOCK_SIZE = 20.0f;

        private Dictionary<Vector3, LevelBlock> mBlocks = new Dictionary<Vector3,LevelBlock>();

        public string Name { get { return mName; } }
        private string mName;

        public Level(string name)
        {
            mName = name;
        }

        public void AddNewBlock(Vector3 blockCoordinate)
        {
            mBlocks.Add(blockCoordinate, new LevelBlock(mName, blockCoordinate, BLOCK_SIZE));
        }

        public bool Contains(Vector3 blockCoordinate)
        {
            return mBlocks.ContainsKey(blockCoordinate);
        }

        public void Render()
        {
            foreach (LevelBlock block in mBlocks.Values)
            {
                block.Render();
            }
        }

        public List<Tuple<string, MemoryStream>> Serialize()
        {
            List<Tuple<string, MemoryStream>> serializedLevel = new List<Tuple<string, MemoryStream>>();

            foreach (KeyValuePair<Vector3, LevelBlock> block in mBlocks)
            {
                HeightMapMesh blockMesh = AssetLibrary.LookupHeightMap(block.Value.Name).Mesh;

                Tuple<string, MemoryStream> alphaMap = new Tuple<string, MemoryStream>("AlphaMap" + Vector3ToString(block.Key) + "_" + ".png", blockMesh.SerializeAlphaMap());
                Tuple<string, MemoryStream> detailTextures = new Tuple<string, MemoryStream>("DetailTextures" + Vector3ToString(block.Key) + ".ctl", blockMesh.SerializeDetailTextures());
                Tuple<string, MemoryStream> heightMap = new Tuple<string, MemoryStream>("HeightMap" + Vector3ToString(block.Key) + ".png", blockMesh.SerializeHeightMap());

                serializedLevel.Add(alphaMap);
                serializedLevel.Add(detailTextures);
                serializedLevel.Add(heightMap);
            }

            return serializedLevel;
        }

        #region Block Modifiers

        public void IterateOverBlocksInRadius(Vector3 position, float radius, BlockModifier modifier, object[] parameters)
        {
            if (position.X > 20.0f)
            {
                Console.WriteLine("");
            }
            Vector3 offset = new Vector3(radius, 0, radius);
            Vector3 minCoordinate = CoordinateFromPosition(position - offset);
            Vector3 maxCoordinate = CoordinateFromPosition(position + offset);

            for (int z = (int)minCoordinate.Z; z <= (int)maxCoordinate.Z; z++)
            {
                for (int x = (int)minCoordinate.X; x <= (int)maxCoordinate.X; x++)
                {
                    Vector3 coordinate = new Vector3(x, minCoordinate.Y, z);
                    LevelBlock block;
                    if (mBlocks.TryGetValue(coordinate, out block))
                    {
                        modifier(block, (position - coordinate * BLOCK_SIZE) / BLOCK_SIZE, radius / BLOCK_SIZE, parameters);
                    }
                }
            }
        }

        public void IterateOverEveryBlock(BlockModifier modifier, object[] parameters)
        {
            foreach (LevelBlock block in mBlocks.Values)
            {
                modifier(block, new Vector3(), 0, parameters);
            }
        }

        public void IterateOverBlockInContainer(IEnumerable<Vector3> container, BlockModifier modifier, object[] parameters)
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

        public void ModifySingleBlock(Vector3 coordinate, BlockModifier modifier, object[] parameters)
        {
            LevelBlock block;
            if (mBlocks.TryGetValue(coordinate, out block))
            {
                modifier(block, new Vector3(), 0, parameters);
            }
        }

        public delegate void BlockModifier(LevelBlock block, Vector3 centerCoordinate, float radius, object[] parameters);

        #endregion

        #region Helper Methods

        private Vector3 CoordinateFromPosition(Vector3 position)
        {
            return new Vector3((float)Math.Floor(position.X / BLOCK_SIZE),
                (float)Math.Floor(position.Y / BLOCK_SIZE + 0.00001f),
                (float)Math.Floor(position.Z / BLOCK_SIZE));
        }

        private string Vector3ToString(Vector3 vector)
        {
            return "_" + vector.X + "_" + vector.Y + "_" + vector.Z + "_";
        }

        #endregion
    }
}
