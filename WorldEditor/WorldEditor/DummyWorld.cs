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
        //private TextureMap mTextureMap = null;

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

        public DummyWorld(Controls controls)
        {
            
            mName = "default";

            mControls = controls;

            mHeightMap = GraphicsManager.LookupTerrainHeightMap(mName);
            /*mTextureMap = new TextureMap(
                GraphicsManager.LookupTerrainTextures(mName), 
                GraphicsManager.LookupTerrainTextureNames(mName), 
                GraphicsManager.Device);*/

            mTerrainPhysics = new TerrainPhysics(mName, Vector3.Zero, new Quaternion(), Utils.WorldScale);

        }

        public DummyWorld(DummyWorld copy)
        {
            
            mName = copy.mName;

            mControls = copy.mControls;

            mHeightMap = new TerrainHeightMap(copy.mHeightMap);
            //mTextureMap = new TextureMap(copy.mTextureMap);
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

        public void ModifyHeightMap(Vector3 position, int radius, int intensity, bool feather, bool set, bool inverse, bool smooth, bool flatten)
        {
            mHeightMap.ModifyVertices(position, radius, intensity, feather, set, inverse, smooth, flatten);
            mTerrainPhysics = new TerrainPhysics(mName, Vector3.Zero, new Quaternion(), Utils.WorldScale);
        }

        public void ModifyTextureMap(Vector3 position, string texture, int radius, float alpha)
        {
            //mTextureMap.ModifyVertices(position, texture, radius, alpha);
        }

        public void Save(string path)
        {

            System.IO.Directory.CreateDirectory(path);

            mHeightMap.Save(path);
            //mTextureMap.Save(path);

            UnscaleObjects();
            LevelManager.Save(path, mDummies);
            ScaleObjects();

        }

        public void Load(string path)
        {

            mName = path;

            mHeightMap = GraphicsManager.LookupTerrainHeightMap(mName);
            //mTextureMap = new TextureMap(GraphicsManager.LookupTerrainTextures(mName), GraphicsManager.LookupTerrainTextureNames(mName), GraphicsManager.Device);
            mTerrainPhysics = new TerrainPhysics(mName, Vector3.Zero, new Quaternion(), Utils.WorldScale);

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
