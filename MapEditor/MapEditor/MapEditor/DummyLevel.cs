using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;
using GraphicsLibrary;
using GameConstructLibrary;


namespace MapEditor
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class DummyLevel
    {
        private GraphicsDevice mGraphics;
        public GraphicsDevice Graphics { get { return mGraphics; } set { mGraphics = value; } }
        private TerrainHeightMap mHeightMap;
        private Terrain mTerrain;
        private List<DummyObject> mDummies;
        private LevelManager mLevelManager;

        public DummyLevel(int width, int height, GraphicsDevice graphics)
        {

            mLevelManager = new LevelManager();
            mGraphics = graphics;

            Load("default");
            
            DummyObject root = new DummyObject();
            root.Type = "Root";
            root.Position = new Vector3(0, 0, 0);
            root.Orientation = new Vector3(0, 0, 0);
            root.Scale = new Vector3(0, 0, 0);
            Add(root);

        }

        public void Add(DummyObject obj)
        {
            mDummies.Add(obj);
        }

        public void Remove(DummyObject obj)
        {
            mDummies.Remove(obj);
        }

        public void Save(string file)
        {

            // Save the height map
            mHeightMap.Save(file);

            // Save the rest of the level
            mLevelManager.Save(file, mDummies);

        }

        public void Load(string file)
        {
            // Load the height map
            mHeightMap = new TerrainHeightMap(file, mGraphics);
            mTerrain = new Terrain(file);

           // Load the rest of the level
            mDummies = mLevelManager.Load(file);

        }

        public void Render()
        {
            mTerrain.Render(new Vector3(0, 0, 0));
        }

    }
}
