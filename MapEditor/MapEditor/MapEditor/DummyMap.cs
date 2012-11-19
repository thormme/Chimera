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

        private const int scale = 2;
        private const float length = 2000.0f;

        private string mName;
        public string Name { get { return mName; } set { mName = value; } }

        private MapEditor mMapEditor;
        public MapEditor MapEditor { get { return mMapEditor; } set { mMapEditor = value; } }

        private TerrainHeightMap mHeightMap;
        private TerrainPhysics mTerrainPhysics;
        public TerrainPhysics TerrainPhysics { get { return mTerrainPhysics; } set { mTerrainPhysics = value; } }

        private List<DummyObject> mDummies;

        private KeyInputAction mAlt;
        private KeyInputAction mShift;
        private MouseButtonInputAction mLeftPressed;

        private bool mInverseMode;
        private bool mSmoothMode;

        // Used to create new map
        public DummyMap(MapEditor mapEditor, int width, int height)
        {

            mName = "default";

            mMapEditor = mapEditor;

            mHeightMap = GraphicsManager.LookupTerrainHeightMap("default");
            mTerrainPhysics = new TerrainPhysics("default", new Vector3(0, 0, 0), new Quaternion(), new Vector3(scale, 1, scale));

            mDummies = new List<DummyObject>();

            mHeightMap = GraphicsManager.LookupTerrainHeightMap("default");
            mHeightMap.Resize(width, height);

            mAlt = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.LeftAlt);
            mShift = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.LeftShift);
            mLeftPressed = new MouseButtonInputAction(0, InputAction.ButtonAction.Pressed, InputAction.MouseButton.Left);

        }

        public DummyMap(DummyMap copy)
        {
            mName = copy.mName;
            mMapEditor = copy.mMapEditor;
            mHeightMap = new TerrainHeightMap(copy.mHeightMap);
            mTerrainPhysics = new TerrainPhysics(mName, new Vector3(0, 0, 0), new Quaternion(), new Vector3(scale, 1, scale));
            mDummies = new List<DummyObject>();
            foreach (DummyObject obj in copy.mDummies)
            {
                mDummies.Add(new DummyObject(obj));
            }
            mAlt = copy.mAlt;
            mShift = copy.mShift;
            mLeftPressed = copy.mLeftPressed;
            mInverseMode = copy.mInverseMode;
            mSmoothMode = copy.mSmoothMode;
        }

        public void LinkHeightMap()
        {
            GraphicsManager.LookupTerrainHeightMap(mName).FixHeightMap(mHeightMap);
            mHeightMap = GraphicsManager.LookupTerrainHeightMap(mName);
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

                if (mLeftPressed.Active) mMapEditor.AddState(this);

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

                    mTerrainPhysics = new TerrainPhysics(mName, new Vector3(0, 0, 0), new Quaternion(), new Vector3(scale, 1, scale));

                }
                else if (mMapEditor.State == States.Object)
                {
                    string objectType;
                    string objectModel;
                    Vector3 objectScale;
                    Vector3 objectOrientation;
                    string[] objectParameters;
                    if (mMapEditor.MapEditorDialog.GetObjectEditorInput(out objectType, out objectModel, out objectScale, out objectOrientation, out objectParameters))
                    {
                        DummyObject temp = new DummyObject();
                        temp.Type = objectType;
                        temp.Model = objectModel;
                        temp.Parameters = objectParameters;
                        temp.Position = position;
                        temp.Orientation = objectOrientation;
                        temp.Scale = objectScale;
                        Add(temp);
                    }
                }
            }
        }

        public void Save(string file)
        {

            // Save the height map
            mHeightMap.Save(file);

            foreach (DummyObject obj in mDummies) obj.Position /= scale;

            // Save the rest of the level
            LevelManager.Save(file, mDummies);

            foreach (DummyObject obj in mDummies) obj.Position *= scale;

        }

        public void Load(string file)
        {

            mName = file;

            // Load the height map
            mHeightMap = GraphicsManager.LookupTerrainHeightMap(file);
            mTerrainPhysics = new TerrainPhysics(file, new Vector3(0, 0, 0), new Quaternion(), new Vector3(scale, 1, scale));

           // Load the rest of the level
            mDummies = LevelManager.Load(file);

            foreach (DummyObject obj in mDummies) obj.Position *= scale;

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
