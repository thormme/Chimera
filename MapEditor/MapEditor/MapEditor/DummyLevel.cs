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


namespace MapEditor
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class DummyLevel
    {
        
        private TerrainHeightMap mHeightMap;
        private Terrain mTerrain;
        private List<DummyObject> mDummies;
        private LevelManager mLevelManager;

        public DummyLevel(int width, int height)
        {

            /*
            mTerrain = new Terrain("test_level");
            mHeightMap = new TerrainHeightMap(width, height);
            mDummies = new List<DummyObject>();
             */
            mLevelManager = new LevelManager();
            

            Load("default");

            /*
            DummyObject trial = new DummyObject();
            trial.Type = "Lion";
            trial.Position = new Vector3(0, 0, 0);
            trial.Orientation = new Vector3(0, 0, 0);
            trial.Scale = new Vector3(0, 0, 0);
            Add(trial);
             */

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
            Console.WriteLine(file);
            // Load the height map
            mHeightMap = new TerrainHeightMap(file);
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
