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
using BEPUphysics.CollisionShapes;
using Nuclex.UserInterface;


namespace MapEditor
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class DummyMap
    {

        private const int scale = 1;

        private MapEditor mMapEditor;
        public MapEditor MapEditor { get { return mMapEditor; } set { mMapEditor = value; } }

        private TerrainHeightMap mHeightMap;
        private TerrainPhysics mTerrainPhysics;
        public TerrainPhysics TerrainPhysics { get { return mTerrainPhysics; } set { mTerrainPhysics = value; } }

        private List<DummyObject> mDummies;
        private LevelManager mLevelManager;

        // Used to create new map
        public DummyMap(MapEditor mapEditor, int width, int height)
        {

            mMapEditor = mapEditor;

            mHeightMap = GraphicsManager.LookupTerrainHeightMap("default");
            mTerrainPhysics = new TerrainPhysics("default", new Vector3(scale), new Quaternion(), new Vector3(0, 0, 0));

            mLevelManager = new LevelManager();
            mDummies = new List<DummyObject>();
            

            mHeightMap = GraphicsManager.LookupTerrainHeightMap("default");
            mHeightMap.Resize(width, height);

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

        public void Action(Vector3 position)
        {
            if (mMapEditor.EditMode)
            {
                if (mMapEditor.State == States.None)
                {
                    
                }
                else if (mMapEditor.State == States.Height)
                {
                    int size;
                    int intensity;
                    bool feather;
                    bool set;
                    if (mMapEditor.MapEditorDialog.GetInputs(out size, out intensity, out feather, out set))
                    {
                        mHeightMap.ModifyVertices(position, size, intensity, feather, set);
                    }
                }
                else if (mMapEditor.State == States.Object)
                {

                }
            }
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
            mHeightMap = GraphicsManager.LookupTerrainHeightMap(file);
            mTerrainPhysics = new TerrainPhysics(file, new Vector3(scale), new Quaternion(), new Vector3(0, 0, 0));

           // Load the rest of the level
            mDummies = mLevelManager.Load(file);

        }

        public void Render()
        {
            mTerrainPhysics.Render();
        }

    }
}
