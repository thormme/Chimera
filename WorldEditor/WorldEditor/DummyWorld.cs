using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameConstructLibrary;
using GraphicsLibrary;
using Microsoft.Xna.Framework;
using Utility;
using BEPUphysics;
using WorldEditor.Dialogs;
using System.IO;

namespace WorldEditor
{
    public class DummyWorld
    {

        private const float MoveSpeed = 1.0f;
        private const float ScaleSpeed = 1.05f;
        private const float RotateSpeed = 0.1f;

        public String Name
        {
            get { return mName; }
        }
        private string mName = String.Empty;

        private TerrainHeightMap mHeightMap = null;
        private TerrainTexture mTextureMap = null;

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
            mName = null;
            mHeightMap = null;
            mTextureMap = null;
            mTerrainPhysics = null;
        }

        public DummyWorld(DummyWorld copy)
        {
            mName = copy.mName;

            //mHeightMap = new TerrainHeightMap(copy.mHeightMap);
            //mTextureMap = new TerrainTexture(copy.mTextureMap);
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
            mHeightMap = GraphicsManager.LookupTerrainHeightMap(mName);
            mTextureMap = GraphicsManager.LookupTerrainTexture(mName);
        }

        public void ModifyHeightMap(Vector3 position, float radius, float intensity, EditorForm.Brushes brush, EditorForm.HeightMapTools tool)
        {
            mHeightMap.IsFeathered = brush == EditorForm.Brushes.CIRCLE_FEATHERED || brush == EditorForm.Brushes.BLOCK_FEATHERED;

            mHeightMap.IsBlock = brush == EditorForm.Brushes.BLOCK || brush == EditorForm.Brushes.BLOCK_FEATHERED;

            switch (tool)
            {
                case EditorForm.HeightMapTools.SET:
                    mHeightMap.SetTerrain(new Vector2(position.X, position.Z), radius, intensity);
                    break;
                case EditorForm.HeightMapTools.SMOOTH:
                    mHeightMap.SmoothTerrain(new Vector2(position.X, position.Z), radius);
                    break;
                case EditorForm.HeightMapTools.FLATTEN:
                    mHeightMap.FlattenTerrain(new Vector2(position.X, position.Z), radius);
                    break;
                case EditorForm.HeightMapTools.LOWER:
                    mHeightMap.LowerTerrain(new Vector2(position.X, position.Z), radius, intensity);
                    break;
                case EditorForm.HeightMapTools.RAISE:
                    mHeightMap.RaiseTerrain(new Vector2(position.X, position.Z), radius, intensity);
                    break;
            }

            mTerrainPhysics = new TerrainPhysics(mName, Vector3.Zero, new Quaternion(), Utils.WorldScale);
        }

        public void ModifyTextureMap(Vector3 position, string texture, float radius, float alpha, EditorForm.Brushes brush, EditorForm.PaintingTools tool, GameConstructLibrary.TerrainTexture.TextureLayer layer)
        {
            mTextureMap.IsFeathered = brush == EditorForm.Brushes.CIRCLE_FEATHERED || brush == EditorForm.Brushes.BLOCK_FEATHERED;

            mTextureMap.IsBlock = brush == EditorForm.Brushes.BLOCK || brush == EditorForm.Brushes.BLOCK_FEATHERED;

            switch (tool)
            {
                case EditorForm.PaintingTools.BRUSH:
                    mTextureMap.PaintTerrain(new Vector2(position.X, position.Z), radius, alpha, layer, texture);
                    break;
                case EditorForm.PaintingTools.ERASER:
                    mTextureMap.EraseTerrain(new Vector2(position.X, position.Z), radius, alpha, layer, texture);
                    break;
            }
        }

        public void Save(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            LevelFileLoader.SaveLevelToFile(fileInfo);
            GraphicsManager.UpdateTerrain(fileInfo, ref mName);

            //UnscaleObjects();
            //LevelManager.Save(path, mDummies);
            //ScaleObjects();
        }

        public void Open(FileInfo fileInfo)
        {
            mName = fileInfo.Name;

            mHeightMap = LevelFileLoader.LoadHeightMapFromFile(fileInfo);
            mTextureMap = LevelFileLoader.LoadTextureFromFile(fileInfo);

            GraphicsManager.AddTerrain(fileInfo, mHeightMap, mTextureMap);

            mTerrainPhysics = new TerrainPhysics(mName, Vector3.Zero, new Quaternion(), Utils.WorldScale);

            mDummies = LevelFileLoader.LoadObjectsFromFile(fileInfo);
            ScaleObjects();
        }

        public void New()
        {
            mName = LevelFileLoader.GenerateBlankLevel(100, 100, 2700, 2700, 9, 9, "default_terrain_detail");

            FileInfo fileInfo = new FileInfo(mName);

            mHeightMap = LevelFileLoader.LoadHeightMapFromFile(fileInfo);
            mTextureMap = LevelFileLoader.LoadTextureFromFile(fileInfo);

            GraphicsManager.AddTerrain(fileInfo, mHeightMap, mTextureMap);

            mTerrainPhysics = new TerrainPhysics(mName, Vector3.Zero, new Quaternion(), Utils.WorldScale);
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
            if (mDummies != null)
            {
                foreach (DummyObject obj in mDummies)
                {
                    Ray ray = new Ray(new Vector3(obj.Position.X, mTerrainPhysics.StaticCollidable.BoundingBox.Max.Y + 200.0f, obj.Position.Z), -Vector3.Up);
                    RayHit result;
                    mTerrainPhysics.StaticCollidable.RayCast(ray, (mTerrainPhysics.StaticCollidable.BoundingBox.Max.Y + 200.0f) - (mTerrainPhysics.StaticCollidable.BoundingBox.Min.Y - 200.0f), out result);
                    obj.Position = result.Location;
                }
            }
        }

        public void Draw()
        {
            if (mTerrainPhysics != null)
            {
                mTerrainPhysics.Render();
            }

            if (mDummies != null)
            {
                foreach (DummyObject dummy in mDummies)
                {
                    InanimateModel model = new InanimateModel(dummy.Model);
                    model.Render(
                        new Vector3(dummy.Position.X, dummy.Position.Y + dummy.Height * Utils.WorldScale.Y, dummy.Position.Z),
                        Matrix.CreateFromYawPitchRoll(dummy.Orientation.X, dummy.Orientation.Y, dummy.Orientation.Z),
                        dummy.Scale);
                }
            }
        }

    }
}
