﻿using System;
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
using Microsoft.Xna.Framework.Graphics;

namespace WorldEditor
{
    public class DummyWorld
    {
        #region Public Properties

        public bool DrawSkyBox = true;
        public bool DrawWater = false;

        public String Name
        {
            get { return mName; }
        }
        private string mName = String.Empty;

        #endregion

        #region HeightMap Modification Visualization

        public Vector3 CursorPosition { get; set; }
        public float CursorInnerRadius { get; set; }
        public float CursorOuterRadius { get; set; }
        public HeightMapRenderable.CursorShape DrawCursor { get; set; }

        public Vector4 TerrainLayerMask { get; set; }

        #endregion

        #region Block Selection

        public HashSet<Vector3> SelectedBlocks { get { return mSelectedBlocks; } }
        private HashSet<Vector3> mSelectedBlocks = new HashSet<Vector3>();

        #endregion

        #region Constants

        private const float MoveSpeed = 1.0f;
        private const float ScaleSpeed = 1.05f;
        private const float RotateSpeed = 0.1f;

        #endregion

        #region Level and Objects Representation

        private ModifiableLevel mLevel = null;
        private List<DummyObject> mDummies = new List<DummyObject>();

        private Space Space = new Space();

        private SkyBox mSkyBox = null;
        private Water mWater = null;
        private GridRenderable mGridRenderable = null;

        private int mBlockLayer = 0;

        #endregion

        public DummyWorld(Controls controls)
        {
            mName = null;
            mLevel = null;

            Texture2D gridAlphaMap = new Texture2D(GraphicsManager.Device, HeightMapMesh.NUM_SIDE_VERTICES * HeightMapMesh.NUM_SIDE_TEXELS_PER_QUAD, HeightMapMesh.NUM_SIDE_VERTICES * HeightMapMesh.NUM_SIDE_TEXELS_PER_QUAD);
            HeightMapMesh gridMesh = new HeightMapMesh(new float[HeightMapMesh.NUM_SIDE_VERTICES, HeightMapMesh.NUM_SIDE_VERTICES], gridAlphaMap, new string[0], null, null);
            AssetLibrary.AddGrid("BLOCK_GRID", gridMesh);

            mGridRenderable = new GridRenderable("BLOCK_GRID");
        }

        public void AddBlock(Vector3 coordinate)
        {
            if (LevelFileLoader.GenerateBlankLevelBlock(mLevel, coordinate, "default_terrain_detail"))
            {
                mLevel.ModifySingleBlock(coordinate, AddBlockToSpace, new object[] { Space });
            }
        }

        public void RemoveBlock(Vector3 coordinate)
        {
            mLevel.ModifySingleBlock(coordinate, RemoveBlockFromSpace, new object[] { Space });
            mLevel.RemoveBlock(coordinate);
        }

        public void RemoveSelectedBlocks()
        {
            foreach (Vector3 coordinate in mSelectedBlocks)
            {
                RemoveBlock(coordinate);
            }
            ClearSelectedBlocks();
        }

        public void SelectBlock(Vector3 coordinate)
        {
            if (!mLevel.Contains(coordinate))
            {
                mSelectedBlocks.Clear();
            }
            else if (!mSelectedBlocks.Contains(coordinate))
            {
                mSelectedBlocks.Add(coordinate);
            }
        }

        public void ClearSelectedBlocks()
        {
            mSelectedBlocks.Clear();
        }

        public bool ContainsBlock(Vector3 coordinate)
        {
            return mLevel.Contains(coordinate);
        }

        public void AddObject(DummyObject dummyObject)
        {
            mDummies.Add(dummyObject);
        }

        public void RemoveObject(DummyObject dummyObject)
        {
            mDummies.Remove(dummyObject);

            if (dummyObject.Type == Utils.PlayerTypeName)
            {
                foreach (DummyObject dummy in mDummies)
                {
                    if (dummy.Type == Utils.PlayerTypeName)
                    {
                        return;
                    }
                }
                mDummies.Add(dummyObject);
            }
        }

        public void ModifyHeightMap(
            Vector3 position, 
            float radius, 
            float intensity,
            EditorForm.Brushes brush,
            EditorForm.Tools tool)
        {
            intensity /= 1000.0f;
            bool isFeathered = brush == EditorForm.Brushes.CIRCLE_FEATHERED || brush == EditorForm.Brushes.BLOCK_FEATHERED;
            bool isBlock = brush == EditorForm.Brushes.BLOCK || brush == EditorForm.Brushes.BLOCK_FEATHERED;

            switch (tool)
            {
                case EditorForm.Tools.SET:
                    mLevel.SetTerrain(mSelectedBlocks, position, radius, intensity);
                    break;
                case EditorForm.Tools.SMOOTH:
                    mLevel.SmoothTerrain(mSelectedBlocks, position, radius, intensity);
                    break;
                case EditorForm.Tools.FLATTEN:
                    mLevel.RaiseTerrain(mSelectedBlocks, position, radius, intensity);
                    break;
                case EditorForm.Tools.LOWER:
                    mLevel.LowerTerrain(mSelectedBlocks, position, radius, intensity);
                    break;
                case EditorForm.Tools.RAISE:
                    mLevel.RaiseTerrain(mSelectedBlocks, position, radius, intensity);
                    break;
            }
        }

        public void UndoHeightMap()
        {
            //mHeightMap.Undo();

            //mTerrainPhysics = new TerrainPhysics(mName, Vector3.Zero, new Quaternion(), Utils.WorldScale);
        }

        public void RedoHeightMap()
        {
            //mHeightMap.Redo();

            //mTerrainPhysics = new TerrainPhysics(mName, Vector3.Zero, new Quaternion(), Utils.WorldScale);
        }

        public void ModifyTextureMap(
            Vector3 position, 
            string texture, 
            Vector2 UVOffset, 
            Vector2 UVScale, 
            float radius, 
            float alpha,
            EditorForm.Brushes brush,
            EditorForm.Tools tool, 
            HeightMapMesh.TextureLayer layer)
        {
            bool isFeathered = brush == EditorForm.Brushes.CIRCLE_FEATHERED || brush == EditorForm.Brushes.BLOCK_FEATHERED;
            bool isBlock = brush == EditorForm.Brushes.BLOCK || brush == EditorForm.Brushes.BLOCK_FEATHERED;

            ModifiableLevel.BlockModifier textureModifier = null;
            switch (tool)
            {
                case EditorForm.Tools.PAINT:
                    textureModifier = PaintTexture;
                    break;
                case EditorForm.Tools.ERASE:
                    textureModifier = EraseTexture;
                    break;
                case EditorForm.Tools.BLEND:
                    textureModifier = BlendTexture;
                    break;
            }

            mLevel.IterateOverBlocksInContainerInRadius(
                mSelectedBlocks.Count == 0 ? null : mSelectedBlocks,
                position, 
                radius, 
                textureModifier, 
                new object[] { (bool?)isFeathered, (bool?)isBlock, (float?)alpha, (HeightMapMesh.TextureLayer?)layer, texture, (Vector2?)UVOffset, (Vector2?)UVScale});
        }

        public void Save(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            LevelFileLoader.WriteLevel(mLevel, mDummies, fileInfo);
        }

        public void Open(FileInfo fileInfo)
        {
            AssetLibrary.ClearHeightMaps();

            mLevel.IterateOverEveryBlock(RemoveBlockFromSpace, new object[] { Space });

            mName = fileInfo.Name;

            Tuple<Level, List<DummyObject>> loadedLevel = LevelFileLoader.LoadLevelFromFile(fileInfo);
            mLevel = new ModifiableLevel(loadedLevel.Item1, Space);

            mLevel.IterateOverEveryBlock(AddBlockToSpace, new object[] { Space });

            mDummies = loadedLevel.Item2;
        }

        public void New()
        {
            if (mLevel != null)
            {
                AssetLibrary.ClearHeightMaps();
                mLevel.IterateOverEveryBlock(RemoveBlockFromSpace, new object[] { Space });
            }

            mName = "default";
            mLevel = new ModifiableLevel(LevelFileLoader.GenerateNewLevel(mName, "default_terrain_detail"), Space);

            FileInfo fileInfo = new FileInfo(mName);

            mLevel.IterateOverEveryBlock(AddBlockToSpace, new object[] { Space });

            mSkyBox = new SkyBox("default");
            mWater = new Water("waterTexture", 0.1f);
        }

        public void Update(GameTime gameTime, Vector3 cameraPosition, int blockLayer)
        {
            mBlockLayer = blockLayer;

            if (mSkyBox != null)
            {
                mSkyBox.Position = cameraPosition;
            }

            if (mWater != null)
            {
                mWater.Update(gameTime);
            }

            if (mDummies != null)
            {
                foreach (DummyObject obj in mDummies)
                {
                    if (obj.Floating)
                    {
                        RayCastResult result;
                        if (Space.RayCast(new Ray(obj.Position + Vector3.Up * 1000.0f, Vector3.Down), 2000.0f, out result))
                        {
                            obj.Position = new Vector3(obj.Position.X, result.HitData.Location.Y, obj.Position.Z);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the DummyObject associated with a particular ID.
        /// </summary>
        /// <param name="objectID">The ID.</param>
        /// <returns>
        /// The DummyObject with the ID.
        /// null if none was found.
        /// </returns>
        public DummyObject GetDummyObjectFromID(UInt32 objectID)
        {
            List<DummyObject> objects = GetDummyObjectFromID(new UInt32[] { objectID });
            if (objects.Count > 0)
            {
                return objects[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the DummyObjects associated with particular IDs.
        /// </summary>
        /// <param name="objectIDs">The IDs.</param>
        /// <returns>
        /// The DummyObjects with the IDs.
        /// </returns>
        public List<DummyObject> GetDummyObjectFromID(UInt32[] objectIDs)
        {
            List<UInt32> objectIDList = objectIDs.ToList();
            List<DummyObject> selectedObjects = new List<DummyObject>();
            // TODO: Use some structure to efficiencize.
            foreach (DummyObject dummy in mDummies)
            {
                for (int i = 0; i < objectIDList.Count; i++)
                {
                    UInt32 id = objectIDList[i];
                    if (dummy.ObjectID == id)
                    {
                        selectedObjects.Add(dummy);
                        objectIDList.Remove(id);
                        break;
                    }
                }
            }
            return selectedObjects;
        }

        /// <summary>
        /// Find the position and object in a ray.
        /// </summary>
        /// <param name="ray">The ray.</param>
        /// <param name="maximumDistance">The maximum distance to look from the ray origin.</param>
        /// <param name="castResult">Tuple containing the rayHit result and the DummyObject if any (can be null).</param>
        /// <returns>Whether the ray hit anything.</returns>
        public bool RayCast(Ray ray, float maximumDistance, out Tuple<RayHit, DummyObject> castResult)
        {
            // Fill out default failed hit data
            castResult = new Tuple<RayHit, DummyObject>(new RayHit(), null);

            DummyObject selectedObject = GetDummyObjectFromID(GraphicsManager.GetPickingObject(ray));
            RayHit rayHit = new RayHit();

            if (selectedObject == null || !selectedObject.RayCast(ray, maximumDistance, out rayHit))
            {
                RayCastResult rayResult = new RayCastResult();
                if (!Space.RayCast(ray, maximumDistance, out rayResult))
                {
                    return false;
                }
                rayHit = rayResult.HitData;
            }

            castResult = new Tuple<RayHit, DummyObject>(rayHit, selectedObject);
            return true;
        }

        public void Draw(Vector3 cameraPosition)
        {
            Vector3 blockNearestCamera = new Vector3((int)(cameraPosition.X / Level.BLOCK_SIZE) * Level.BLOCK_SIZE, mBlockLayer * Level.BLOCK_SIZE, (int)(cameraPosition.Z / Level.BLOCK_SIZE) * Level.BLOCK_SIZE);
            int scale = (HeightMapMesh.NUM_SIDE_VERTICES - 1) * (int)Level.BLOCK_SIZE;

            mGridRenderable.Render(blockNearestCamera + new Vector3(-scale / 2, 0, -scale / 2), Matrix.Identity, new Vector3(scale, 1, scale));

            if (mLevel != null)
            {
                mLevel.IterateOverBlocksInRadius(CursorPosition, CursorOuterRadius, RenderCursor, new object[] { (HeightMapRenderable.CursorShape?)DrawCursor, (float?)(CursorInnerRadius), (float?)CursorOuterRadius, (Vector3?)CursorPosition });
                mLevel.IterateOverBlocksInContainer(mSelectedBlocks, SelectBlock, null);
                mLevel.Render();
            }

            if (mWater != null && DrawWater)
            {
                mWater.Render();
            }

            if (mSkyBox != null && DrawSkyBox)
            {
                mSkyBox.Render();
            }

            if (mDummies != null)
            {
            	foreach (DummyObject dummy in mDummies)
            	{
                	dummy.Draw();
            	}
            }
        }

        private void RenderCursor(LevelBlock block, Vector3 centerCoordinate, float radius, object[] parameters)
        {
            block.HeightMap.Renderable.DrawCursor = (parameters[0] as HeightMapRenderable.CursorShape?).Value;
            block.HeightMap.Renderable.CursorInnerRadius = (parameters[1] as float?).Value;
            block.HeightMap.Renderable.CursorOuterRadius = (parameters[2] as float?).Value;
            block.HeightMap.Renderable.CursorPosition = (parameters[3] as Vector3?).Value;
        }

        private void SelectBlock(LevelBlock block, Vector3 centerCoordinate, float radius, object[] parameters)
        {
            block.HeightMap.Renderable.Selected = true;
        }

        private void AddBlockToSpace(LevelBlock block, Vector3 centerCoordinate, float radius, object[] parameters)
        {
            Space space = parameters[0] as Space;
            space.Add(block.HeightMap.StaticCollidable);
        }

        private void RemoveBlockFromSpace(LevelBlock block, Vector3 centerCoordinate, float radius, object[] parameters)
        {
            Space space = parameters[0] as Space;
            space.Remove(block.HeightMap.StaticCollidable);
        }
        
        private void PaintTexture(LevelBlock block, Vector3 centerCoordinate, float radius, object[] parameters)
        {
            bool isFeathered = (parameters[0] as bool?).Value;
            bool isBlock = (parameters[1] as bool?).Value;
            float alpha = (parameters[2] as float?).Value;
            HeightMapMesh.TextureLayer layer = (parameters[3] as HeightMapMesh.TextureLayer?).Value;
            string texture = parameters[4] as string;
            Vector2 UVOffset = (parameters[5] as Vector2?).Value;
            Vector2 UVScale = (parameters[6] as Vector2?).Value;

            block.HeightMap.PaintTexture(centerCoordinate, radius, alpha, layer, texture, UVOffset, UVScale, isFeathered, isBlock);
        }

        private void EraseTexture(LevelBlock block, Vector3 centerCoordinate, float radius, object[] parameters)
        {
            bool isFeathered = (parameters[0] as bool?).Value;
            bool isBlock = (parameters[1] as bool?).Value;
            float alpha = (parameters[2] as float?).Value;
            HeightMapMesh.TextureLayer layer = (parameters[3] as HeightMapMesh.TextureLayer?).Value;
            string texture = parameters[4] as string;
            Vector2 UVOffset = (parameters[5] as Vector2?).Value;
            Vector2 UVScale = (parameters[6] as Vector2?).Value;

            block.HeightMap.EraseTexture(centerCoordinate, radius, alpha, layer, texture, UVOffset, UVScale, isFeathered, isBlock);
        }

        private void BlendTexture(LevelBlock block, Vector3 centerCoordinate, float radius, object[] parameters)
        {
            bool isFeathered = (parameters[0] as bool?).Value;
            bool isBlock = (parameters[1] as bool?).Value;

            block.HeightMap.BlendTexture(centerCoordinate, radius, isFeathered, isBlock);
        }
    }
}
