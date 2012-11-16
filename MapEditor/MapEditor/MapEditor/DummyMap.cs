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
using BEPUphysics;


namespace MapEditor
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class DummyMap
    {

        private const int scale = 1;
        private const float length = 2000.0f;

        private string mName;

        private MapEditor mMapEditor;
        public MapEditor MapEditor { get { return mMapEditor; } set { mMapEditor = value; } }

        private TerrainHeightMap mHeightMap;
        private TerrainPhysics mTerrainPhysics;
        public TerrainPhysics TerrainPhysics { get { return mTerrainPhysics; } set { mTerrainPhysics = value; } }

        private List<DummyObject> mDummies;
        private LevelManager mLevelManager;

        private KeyInputAction mAlt;
        private KeyInputAction mShift;

        private bool mInverseMode;
        private bool mSmoothMode;

        // Used to create new map
        public DummyMap(MapEditor mapEditor, int width, int height)
        {

            mName = "default";

            mMapEditor = mapEditor;

            mHeightMap = GraphicsManager.LookupTerrainHeightMap("default");
            mTerrainPhysics = new TerrainPhysics("default", new Vector3(0, 0, 0), new Quaternion(), scale);

            mLevelManager = new LevelManager();
            mDummies = new List<DummyObject>();

            mHeightMap = GraphicsManager.LookupTerrainHeightMap("default");
            mHeightMap.Resize(width, height);

            mAlt = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.LeftAlt);
            mShift = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.LeftShift);

            DummyObject root = new DummyObject();
            root.Type = "Root";
            root.Model = "sphere";
            root.Parameters = new string[0];
            root.Position = new Vector3(0, 0, 0);
            root.Orientation = new Vector3(0, 0, 0);
            root.Scale = new Vector3(0, 0, 0);
            Add(root);

        }

        public void Update()
        {
            if (mAlt.Active)
            {
                mInverseMode = true;
            }
            else
            {
                mInverseMode = false;
            }

            if (mShift.Active)
            {
                mSmoothMode = true;
            }
            else
            {
                mSmoothMode = false;
            }

            foreach (DummyObject obj in mDummies)
            {
                // Adjust each to the new height based on terrain modifications
                Ray ray = new Ray(new Vector3(obj.Position.X, 1000.0f * scale, obj.Position.Z), new Vector3(0, -1, 0));
                RayHit result;
                mTerrainPhysics.StaticCollidable.RayCast(ray, length * scale, out result);
                obj.Position = result.Location;
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
                    if (mMapEditor.MapEditorDialog.GetHeightEditorInput(out size, out intensity, out feather, out set))
                    {
                        if (mInverseMode)
                        {
                            intensity = -intensity;
                        }
                        mHeightMap.ModifyVertices(position, size, intensity, feather, set, mSmoothMode);
                    }

                    mTerrainPhysics = new TerrainPhysics(mName, new Vector3(scale), new Quaternion(), new Vector3(0, 0, 0));

                }
                else if (mMapEditor.State == States.Object)
                {
                    Console.WriteLine("here");
                    string objectType;
                    string objectModel;
                    string[] objectParameters;
                    if (mMapEditor.MapEditorDialog.GetObjectEditorInput(out objectType, out objectModel, out objectParameters))
                    {
                        DummyObject temp = new DummyObject();
                        temp.Type = objectType;
                        temp.Model = objectModel;
                        temp.Parameters = objectParameters;
                        temp.Position = position;
                        temp.Orientation = new Vector3(0, 0, 1);
                        temp.Scale = new Vector3(0.1f, 0.1f, 0.1f);
                        Add(temp);
                    }
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

            mName = file;

            // Load the height map
            mHeightMap = GraphicsManager.LookupTerrainHeightMap(file);
            mTerrainPhysics = new TerrainPhysics(file, new Vector3(0, 0, 0), new Quaternion(), scale);

           // Load the rest of the level
            mDummies = mLevelManager.Load(file);

        }

        public void Render()
        {
            mTerrainPhysics.Render();
            foreach (DummyObject obj in mDummies)
            {
                InanimateModel model = new InanimateModel(obj.Model);
                model.Render(obj.Position, obj.Orientation, obj.Scale);
            }
        }

    }
}
