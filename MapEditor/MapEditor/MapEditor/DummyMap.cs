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

        private const float length = 2000.0f;
        private const float moveSpeed = 0.1f;
        private const float scaleSpeed = 1.05f;
        private const float rotateSpeed = 0.1f;

        private string mName;

        private TerrainHeightMap mHeightMap;
        private TerrainPhysics mTerrainPhysics;
        public TerrainPhysics TerrainPhysics { get { return mTerrainPhysics; } set { mTerrainPhysics = value; } }

        private List<DummyObject> mDummies;

        private KeyInputAction mInverse;
        private KeyInputAction mSmooth;
        private KeyInputAction mFlatten;
        private MouseButtonInputAction mLeftPressed;

        private bool mInverseMode;
        private bool mSmoothMode;
        private bool mFlattenMode;

        // Used to create new map
        public DummyMap(int width, int height)
        {

            mName = "default";

            mHeightMap = GraphicsManager.LookupTerrainHeightMap("default");
            mTerrainPhysics = new TerrainPhysics("default", new Vector3(0, 0, 0), new Quaternion(), GameMapEditor.MapScale);

            mDummies = new List<DummyObject>();

            mHeightMap = GraphicsManager.LookupTerrainHeightMap("default");
            mHeightMap.Resize(width, height);

            mInverse = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.D1);
            mSmooth = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.D2);
            mFlatten = new KeyInputAction(0, InputAction.ButtonAction.Down, Keys.D3);
            mLeftPressed = new MouseButtonInputAction(0, InputAction.ButtonAction.Pressed, InputAction.MouseButton.Left);

        }

        public DummyMap(DummyMap copy)
        {
            mName = copy.mName;
            mHeightMap = new TerrainHeightMap(copy.mHeightMap);
            mTerrainPhysics = new TerrainPhysics(mName, new Vector3(0, 0, 0), new Quaternion(), GameMapEditor.MapScale);
            mDummies = new List<DummyObject>();
            foreach (DummyObject obj in copy.mDummies)
            {
                mDummies.Add(new DummyObject(obj));
            }
            mInverse = copy.mInverse;
            mSmooth = copy.mSmooth;
            mFlatten = copy.mFlatten;
            mLeftPressed = copy.mLeftPressed;
            mInverseMode = copy.mInverseMode;
            mSmoothMode = copy.mSmoothMode;
            mFlattenMode = copy.mFlattenMode;
        }

        public void LinkHeightMap()
        {
            GraphicsManager.LookupTerrainHeightMap(mName).FixHeightMap(mHeightMap);
            mHeightMap = GraphicsManager.LookupTerrainHeightMap(mName);
        }

        public void ModifyVertices()
        {
            mHeightMap.ModifyVertices(GameMapEditor.Position, GameMapEditor.Size, GameMapEditor.Intensity, GameMapEditor.Feather, GameMapEditor.Set, mInverseMode, mSmoothMode, mFlattenMode);
            mTerrainPhysics = new TerrainPhysics(mName, new Vector3(0, 0, 0), new Quaternion(), GameMapEditor.MapScale);
        }

        private void Add(DummyObject obj)
        {
            mDummies.Add(obj);
        }

        private void Remove(DummyObject obj)
        {
            mDummies.Remove(obj);
        }

        public void AddObject()
        {
            GameMapEditor.Dummy.Position = GameMapEditor.Position;
            Add(new DummyObject(GameMapEditor.Dummy));
        }

        public List<DummyObject> Select(Vector2 topLeft, Vector2 bottomRight)
        {
            List<DummyObject> selected = new List<DummyObject>();
            foreach (DummyObject obj in mDummies)
            {
                Vector3 cameraCoordinate = Vector3.Transform(obj.Position, GameMapEditor.Camera.ViewTransform * GameMapEditor.Camera.ProjectionTransform);
                Vector2 screenCoordinate = new Vector2(cameraCoordinate.X + GameMapEditor.Viewport.Width / 2, GameMapEditor.Viewport.Height / 2 - cameraCoordinate.Y );
                if (screenCoordinate.X > topLeft.X && screenCoordinate.X < bottomRight.X &&
                    screenCoordinate.Y > topLeft.Y && screenCoordinate.Y < bottomRight.Y)
                {
                    selected.Add(obj);
                }
            }
            return selected;
        }

        public void Delete(List<DummyObject> selected)
        {
            foreach (DummyObject obj in selected)
            {
                Remove(obj);
            }
        }

        public void Move(List<DummyObject> selected, Vector3 movement)
        {
            foreach (DummyObject obj in selected)
            {

                obj.Position = new Vector3(obj.Position.X + movement.X * moveSpeed,
                                           obj.Position.Y,
                                           obj.Position.Z + movement.Z * moveSpeed);

                obj.Height += movement.Y * moveSpeed;

            }
        }

        public void Scale(List<DummyObject> selected, Boolean direction)
        {
            foreach (DummyObject obj in selected)
            {
                if (direction) obj.Scale *= scaleSpeed;
                else obj.Scale /= scaleSpeed;
            }
        }

        public void Rotate(List<DummyObject> selected, Boolean direction)
        {
            foreach (DummyObject obj in selected)
            {
                if (direction) obj.Orientation = new Vector3(obj.Orientation.X + rotateSpeed, obj.Orientation.Y, obj.Orientation.Z);
                else obj.Orientation = new Vector3(obj.Orientation.X - rotateSpeed, obj.Orientation.Y, obj.Orientation.Z);
            }
        }

        public void Save(string file)
        {

            // Save the height map
            mHeightMap.Save(file);

            foreach (DummyObject obj in mDummies)
            {
                obj.Position = new Vector3(obj.Position.X / GameMapEditor.MapScale.X, (obj.Position.Y + obj.Height) / GameMapEditor.MapScale.Y, obj.Position.Z / GameMapEditor.MapScale.Z);
                obj.Scale = new Vector3(obj.Scale.X / GameMapEditor.MapScale.X, obj.Scale.Y / GameMapEditor.MapScale.Y, obj.Scale.Z / GameMapEditor.MapScale.Z);
            }

            // Save the rest of the level
            LevelManager.Save(file, mDummies);

            foreach (DummyObject obj in mDummies)
            {
                obj.Position = new Vector3(obj.Position.X * GameMapEditor.MapScale.X, (obj.Position.Y - obj.Height) * GameMapEditor.MapScale.Y, obj.Position.Z * GameMapEditor.MapScale.Z);
                obj.Scale = new Vector3(obj.Scale.X * GameMapEditor.MapScale.X, obj.Scale.Y * GameMapEditor.MapScale.Y, obj.Scale.Z * GameMapEditor.MapScale.Z);
            }

        }
        
        public void Load(string file)
        {

            mName = file;

            // Load the height map
            mHeightMap = GraphicsManager.LookupTerrainHeightMap(file);
            mTerrainPhysics = new TerrainPhysics(file, new Vector3(0, 0, 0), new Quaternion(), GameMapEditor.MapScale);

           // Load the rest of the level
            mDummies = LevelManager.Load(file);

            foreach (DummyObject obj in mDummies)
            {
                obj.Position = new Vector3(obj.Position.X * GameMapEditor.MapScale.X, (obj.Position.Y - obj.Height) * GameMapEditor.MapScale.Y, obj.Position.Z * GameMapEditor.MapScale.Z);
                obj.Scale = new Vector3(obj.Scale.X * GameMapEditor.MapScale.X, obj.Scale.Y * GameMapEditor.MapScale.Y, obj.Scale.Z * GameMapEditor.MapScale.Z);
            }

        }

        public void Update(GameTime gameTime)
        {

            mInverseMode = mInverse.Active;
            mSmoothMode = mSmooth.Active;
            mFlattenMode = mFlatten.Active;

            if (GameMapEditor.EditMode == Edit.Height)
            {
                foreach (DummyObject obj in mDummies)
                {
                    // Adjust each to the new height based on terrain modifications
                    Ray ray = new Ray(new Vector3(obj.Position.X, 1000.0f, obj.Position.Z), new Vector3(0, -1, 0));
                    RayHit result;
                    mTerrainPhysics.StaticCollidable.RayCast(ray, length, out result);
                    obj.Position = result.Location;
                }
            }

        }

        public void Render()
        {
            mTerrainPhysics.Render();
            foreach (DummyObject obj in mDummies)
            {
                InanimateModel model = new InanimateModel(obj.Model);
                model.Render(new Vector3(obj.Position.X, obj.Position.Y + obj.Height * GameMapEditor.MapScale.Y, obj.Position.Z), 
                                         Matrix.CreateFromYawPitchRoll(obj.Orientation.X, obj.Orientation.Y, obj.Orientation.Z), 
                                         obj.Scale);
            }
        }

    }
}
