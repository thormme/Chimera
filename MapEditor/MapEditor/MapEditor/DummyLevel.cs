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
    public class DummyLevel
    {

        private KeyInputAction mTab;
        private bool mEditMode;

        private MapEditorDialog mMapEditorDialog;
        public MapEditorDialog MapEditor { get { return mMapEditorDialog; } set { mMapEditorDialog = value; } }
        private MapEntity mMapEntity;
        public MapEntity Entity { get { return mMapEntity; } set { mMapEntity = value; } }
        private GraphicsDevice mGraphics;
        public GraphicsDevice Graphics { get { return mGraphics; } set { mGraphics = value; } }
        private TerrainHeightMap mHeightMap;
        private TerrainRenderable mTerrain;
        private List<DummyObject> mDummies;
        private LevelManager mLevelManager;

        // Used at start
        public DummyLevel(MapEntity mapEntity, GraphicsDevice graphics)
        {

            mLevelManager = new LevelManager();
            mMapEntity = mapEntity;
            mGraphics = graphics;

            mTab = new KeyInputAction(0, InputAction.ButtonAction.Pressed, Keys.Tab);

            mMapEntity.Level = this;

            Load("default");

        }

        // Used to create new
        public DummyLevel(int width, int height, MapEntity mapEntity, GraphicsDevice graphics)
        {

            mLevelManager = new LevelManager();
            mMapEntity = mapEntity;
            mGraphics = graphics;

            mHeightMap = GraphicsManager.LookupTerrainHeightMap("default");
            mHeightMap.Resize(width, height);

            DummyObject root = new DummyObject();
            root.Type = "Root";
            root.Position = new Vector3(0, 0, 0);
            root.Orientation = new Vector3(0, 0, 0);
            root.Scale = new Vector3(0, 0, 0);
            Add(root);

        }

        public void Update(GameTime gameTime)
        {
            
            mMapEntity.Update(gameTime);

            if (mTab.Active)
            {
                mEditMode = !mEditMode;
                if (mEditMode)
                {
                    mMapEditorDialog.Disable();
                }
                else
                {
                    mMapEditorDialog.Enable();
                }
            }

        }

        public void Add(DummyObject obj)
        {
            mDummies.Add(obj);
        }

        public void Remove(DummyObject obj)
        {
            mDummies.Remove(obj);
        }

        public void Click(Vector3 position)
        {
            if (mEditMode)
            {
                if (mMapEditorDialog.State == States.None)
                {
                    mHeightMap.Resize(200, 200);
                }
                else if (mMapEditorDialog.State == States.Height)
                {
                    int size;
                    int intensity;
                    bool set;
                    if (mMapEditorDialog.GetInputs(out size, out intensity, out set))
                    {
                        mHeightMap.ModifyVertices(position, size, intensity, set);
                    }
                }
                else if (mMapEditorDialog.State == States.Object)
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
            mTerrain = new TerrainRenderable(file);

           // Load the rest of the level
            mDummies = mLevelManager.Load(file);

        }

        public void Render()
        {
            mTerrain.Render(new Vector3(0, 0, 0));
        }

    }
}
