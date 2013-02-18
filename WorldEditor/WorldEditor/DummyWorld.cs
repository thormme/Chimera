using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using Utility;
using BEPUphysics;

namespace WorldEditor
{
    public class DummyWorld
    {

        private const float MoveSpeed = 1.0f;
        private const float ScaleSpeed = 1.05f;
        private const float RotateSpeed = 0.1f;

        private string mName = String.Empty;

        private Controls mControls = null;

        private TerrainHeightMap mHeightMap = null;

        public TerrainPhysics Terrain
        {
            get
            {
                return mTerrainPhysics;
            }
            private set
            {
                mTerrainPhysics = value;
            }
        }
        private TerrainPhysics mTerrainPhysics = null;

        private List<DummyObject> mDummies = new List<DummyObject>();

        public DummyWorld(Controls controls, int width, int height)
        {
            
            mName = "default";

            mControls = controls;

            mHeightMap = GraphicsManager.LookupTerrainHeightMap(mName);
            mHeightMap.Resize(width, height);

            mTerrainPhysics = new TerrainPhysics(mName, Vector3.Zero, new Quaternion(), Utils.WorldScale);

        }

        public DummyWorld(DummyWorld copy)
        {
            
            mName = copy.mName;

            mControls = copy.mControls;

            mHeightMap = new TerrainHeightMap(copy.mHeightMap);
            mTerrainPhysics = new TerrainPhysics(mName, Vector3.Zero, new Quaternion(), Utils.WorldScale);
            
            mDummies = new List<DummyObject>();
            foreach (DummyObject obj in copy.mDummies)
            {
                mDummies.Add(new DummyObject(obj));
            }

        }

        public void AddObject(DummyObject dummyObject)
        {
            mDummies.Add(dummyObject);
        }

        public void RemoveObject(DummyObject dummyObject)
        {
            mDummies.Remove(dummyObject);
        }

        public void LinkHeightMap()
        {
            GraphicsManager.LookupTerrainHeightMap(mName).FixHeightMap(mHeightMap);
            mHeightMap = GraphicsManager.LookupTerrainHeightMap(mName);
        }

        public void ModifyHeightMap()
        {
            mHeightMap.ModifyVertices(Vector3.Zero, 10, 10, false, false, mControls.Inverse.Active, mControls.Smooth.Active, mControls.Flatten.Active);
            mTerrainPhysics = new TerrainPhysics(mName, Vector3.Zero, new Quaternion(), Utils.WorldScale);
        }

        public void Save(string fileName)
        {
            
            mHeightMap.Save(fileName);

            UnscaleObjects();
            LevelManager.Save(fileName, mDummies);
            ScaleObjects();

        }

        public void Load(string fileName)
        {

            mName = fileName;

            mHeightMap = GraphicsManager.LookupTerrainHeightMap(mName);
            mTerrainPhysics = new TerrainPhysics(fileName, Vector3.Zero, new Quaternion(), Utils.WorldScale);

            mDummies = LevelManager.Load(mName);
            ScaleObjects();

        }

        private void UnscaleObjects()
        {
            foreach (DummyObject obj in mDummies)
            {
                obj.Position = new Vector3(obj.Position.X / Utils.WorldScale.X, obj.Position.Y / Utils.WorldScale.Y + obj.Height, obj.Position.Z / Utils.WorldScale.Z);
                obj.Scale = new Vector3(obj.Scale.X / Utils.WorldScale.X, obj.Scale.Y / Utils.WorldScale.Y, obj.Scale.Z / Utils.WorldScale.Z);
            }
        }

        private void ScaleObjects()
        {
            foreach (DummyObject obj in mDummies)
            {
                obj.Position = new Vector3(obj.Position.X * Utils.WorldScale.X, obj.Position.Y * Utils.WorldScale.Y - obj.Height, obj.Position.Z * Utils.WorldScale.Z);
                obj.Scale = new Vector3(obj.Scale.X * Utils.WorldScale.X, obj.Scale.Y * Utils.WorldScale.Y, obj.Scale.Z * Utils.WorldScale.Z);
            }
        }

        public void Update(GameTime gameTime)
        {

            foreach (DummyObject obj in mDummies)
            {
                Ray ray = new Ray(new Vector3(obj.Position.X, mTerrainPhysics.StaticCollidable.BoundingBox.Max.Y + 200.0f, obj.Position.Z), -Vector3.Up);
                RayHit result;
                mTerrainPhysics.StaticCollidable.RayCast(ray, (mTerrainPhysics.StaticCollidable.BoundingBox.Max.Y + 200.0f) - (mTerrainPhysics.StaticCollidable.BoundingBox.Min.Y - 200.0f), out result);
                obj.Position = result.Location;
            }
        }

        public void Draw()
        {
            mTerrainPhysics.Render();
            foreach (DummyObject dummy in mDummies)
            {
                dummy.Draw();
            }
        }

    }
}
