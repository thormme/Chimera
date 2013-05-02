using GraphicsLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameConstructLibrary
{
    public class LevelBlock
    {
        public string Name { get { return mName; } }
        private string mName;

        public Vector3 Coordinate { get { return mCoordinate; } }
        private Vector3 mCoordinate;

        // THIS SHOULD BE PRIVATE
        public HeightMapBlock HeightMap
        {
            get { return mHeightMapBlock; }
        }
        private HeightMapBlock mHeightMapBlock;

        public LevelBlock(string levelName, Vector3 coordinate, float scale)
        {
            mCoordinate = coordinate;
            mName = levelName + mCoordinate.ToString();

            mHeightMapBlock = new HeightMapBlock(mName, coordinate * scale, new Quaternion(), new Vector3(scale));
        }

        public void Render()
        {
            mHeightMapBlock.Render();
        }

        public void SerializeObjects()
        {

        }
    }
}
