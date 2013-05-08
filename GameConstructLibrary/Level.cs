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
        #region Constants

        public const float BLOCK_SIZE = 20.0f;

        #endregion

        #region Variables

        public Dictionary<Vector3, LevelBlock> Blocks
        {
            get { return mBlocks; }
        }
        protected Dictionary<Vector3, LevelBlock> mBlocks = new Dictionary<Vector3,LevelBlock>();

        public string Name { get { return mName; } }
        protected string mName;

        #endregion

        #region Public Interface

        public Level(string name)
        {
            mName = name;
        }

        public virtual void AddNewBlock(Vector3 blockCoordinate)
        {
            mBlocks.Add(blockCoordinate, new LevelBlock(mName, blockCoordinate, BLOCK_SIZE));
        }

        public void RemoveBlock(Vector3 blockCoordinate)
        {
            if (mBlocks.ContainsKey(blockCoordinate))
            {
                AssetLibrary.RemoveHeightMap(mBlocks[blockCoordinate].Name);
                mBlocks.Remove(blockCoordinate);
            }
        }

        public bool Contains(Vector3 blockCoordinate)
        {
            return mBlocks.ContainsKey(blockCoordinate);
        }

        public void AddBlocksToWorld(List<IGameObject> uncommitedGameObjects)
        {
            foreach (LevelBlock block in mBlocks.Values)
            {
                uncommitedGameObjects.Add(block.HeightMap);
            }
        }

        public void Render()
        {
            foreach (LevelBlock block in mBlocks.Values)
            {
                block.Render();
            }
        }

        #endregion

        #region Serialization

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

        #endregion

        #region Helper Methods

        private string Vector3ToString(Vector3 vector)
        {
            return "_" + vector.X + "_" + vector.Y + "_" + vector.Z + "_";
        }

        #endregion
    }
}
