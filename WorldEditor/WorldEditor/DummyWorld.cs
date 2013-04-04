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
        private SkyBox mSkyBox = null;
        private Water mWater = null;

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

        public Space Space = new Space();

        public bool NewHeightMapAction
        {
            get { return mHeightMap.NewAction; }
            set { mHeightMap.NewAction = value; }
        }
        
        public DummyWorld(Controls controls)
        {
            mName = null;
            mHeightMap = null;
            mTextureMap = null;
            mTerrainPhysics = null;
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
            mHeightMap = AssetLibrary.LookupTerrainHeightMap(mName);
            mTextureMap = AssetLibrary.LookupTerrainTexture(mName);
        }

        public void ModifyHeightMap(
            Vector3 position, 
            float radius, 
            float intensity, 
            EditorForm.Brushes brush, 
            EditorForm.HeightMapTools tool)
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

        public void UndoHeightMap()
        {
            mHeightMap.Undo();

            mTerrainPhysics = new TerrainPhysics(mName, Vector3.Zero, new Quaternion(), Utils.WorldScale);
        }

        public void RedoHeightMap()
        {
            mHeightMap.Redo();

            mTerrainPhysics = new TerrainPhysics(mName, Vector3.Zero, new Quaternion(), Utils.WorldScale);
        }

        public void ModifyTextureMap(
            Vector3 position, 
            string texture, 
            Vector2 UVOffset, 
            Vector2 UVScale, 
            float radius, 
            float alpha, 
            EditorForm.Brushes brush, 
            EditorForm.PaintingTools tool, 
            GameConstructLibrary.TerrainTexture.TextureLayer layer)
        {
            mTextureMap.IsFeathered = brush == EditorForm.Brushes.CIRCLE_FEATHERED || brush == EditorForm.Brushes.BLOCK_FEATHERED;

            mTextureMap.IsBlock = brush == EditorForm.Brushes.BLOCK || brush == EditorForm.Brushes.BLOCK_FEATHERED;

            switch (tool)
            {
                case EditorForm.PaintingTools.BRUSH:
                    mTextureMap.PaintTerrain(new Vector2(position.X, position.Z), radius, alpha, layer, texture, UVOffset, UVScale);
                    break;
                case EditorForm.PaintingTools.ERASER:
                    mTextureMap.EraseTerrain(new Vector2(position.X, position.Z), radius, alpha, layer, texture, UVOffset, UVScale);
                    break;
                case EditorForm.PaintingTools.SMOOTH:
                    mTextureMap.SmoothPaint(new Vector2(position.X, position.Z), radius);
                    break;
            }
        }

        public void Save(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            UnscaleObjects();
            LevelFileLoader.SaveLevelToFile(fileInfo);
            ScaleObjects();
            AssetLibrary.UpdateTerrain(fileInfo, ref mName);
        }

        public void Open(FileInfo fileInfo)
        {
            mName = fileInfo.Name;

            mHeightMap = LevelFileLoader.LoadHeightMapFromFile(fileInfo);
            mTextureMap = LevelFileLoader.LoadTextureFromFile(fileInfo);

            AssetLibrary.AddTerrain(fileInfo, mHeightMap, mTextureMap);

            mTerrainPhysics = new TerrainPhysics(mName, Vector3.Zero, new Quaternion(), Utils.WorldScale);

            mDummies = LevelFileLoader.LoadObjectsFromFile(fileInfo);
            ScaleObjects();
        }

        public void New()
        {
            mName = LevelFileLoader.GenerateBlankLevel(100, 100, 900, 900, 9, 9, "default_terrain_detail");

            FileInfo fileInfo = new FileInfo(mName);

            mHeightMap = LevelFileLoader.LoadHeightMapFromFile(fileInfo);
            mTextureMap = LevelFileLoader.LoadTextureFromFile(fileInfo);

            AssetLibrary.AddTerrain(fileInfo, mHeightMap, mTextureMap);

            mTerrainPhysics = new TerrainPhysics(mName, Vector3.Zero, new Quaternion(), Utils.WorldScale);

            mSkyBox = new SkyBox("overcastSkyBox");
            mWater = new Water("waterTexture", 100);
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

        public void Update(GameTime gameTime, Vector3 cameraPosition)
        {
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
                        Ray ray = new Ray(new Vector3(obj.Position.X, mTerrainPhysics.StaticCollidable.BoundingBox.Max.Y + 200.0f, obj.Position.Z), -Vector3.Up);
                        RayHit result;
                        mTerrainPhysics.StaticCollidable.RayCast(ray, (mTerrainPhysics.StaticCollidable.BoundingBox.Max.Y + 200.0f) - (mTerrainPhysics.StaticCollidable.BoundingBox.Min.Y - 200.0f), out result);
                        obj.Position = result.Location;
                    }
                }
            }
        }

        private void DrawTerrain(GraphicsDevice graphics, Matrix view, Matrix projection)
        {
            AnimationUtilities.SkinnedEffect effect = AssetLibrary.VertexBufferShader;

            effect.CurrentTechnique = effect.Techniques["NormalDepthShade"];

            effect.World = Matrix.Identity;
            effect.View = view;
            effect.Projection = projection;

            var heightMap = AssetLibrary.LookupTerrainHeightMap(mName);
            for (int chunkCol = 0; chunkCol < heightMap.NumChunksHorizontal; chunkCol++)
            {
                for (int chunkRow = 0; chunkRow < heightMap.NumChunksVertical; chunkRow++)
                {
                    VertexBuffer vertexBuffer = heightMap.VertexBuffers[chunkRow, chunkCol];
                    IndexBuffer indexBuffer = heightMap.IndexBuffers[chunkRow, chunkCol];

                    graphics.SetVertexBuffer(vertexBuffer);
                    graphics.Indices = indexBuffer;

                    effect.CurrentTechnique.Passes[0].Apply();

                    graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);
                }
            }

            for (int side = 0; side < 4; side++)
            {
                VertexBuffer vertexBuffer = heightMap.EdgeVertexBuffers[side];
                IndexBuffer indexBuffer = heightMap.EdgeIndexBuffers[side];

                graphics.SetVertexBuffer(vertexBuffer);
                graphics.Indices = indexBuffer;

                effect.CurrentTechnique.Passes[0].Apply();

                graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);
            }
        }

        public Tuple<Vector3, DummyObject> RayCast(GraphicsDevice graphics, Ray ray)
        {
            // Record original graphics device settings
            var oldRenderTargets = graphics.GetRenderTargets();

            // Set graphics device to render to texture
            RenderTarget2D depthTarget = new RenderTarget2D(
                graphics,
                1,
                1,
                false,
                graphics.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            DepthStencilState depthStencilState = new DepthStencilState();
            depthStencilState.DepthBufferFunction = CompareFunction.LessEqual;

            graphics.DepthStencilState = depthStencilState;
            graphics.SetRenderTarget(depthTarget);

            Viewport pickingViewport = new Microsoft.Xna.Framework.Graphics.Viewport(0, 0, 1, 1);

            FPSCamera camera = new FPSCamera(pickingViewport);
            camera.Position = ray.Position;
            camera.Target = camera.Position + ray.Direction;

            graphics.Clear(Color.CornflowerBlue);
            BasicEffect b = new BasicEffect(graphics);

            Single minDepth = camera.FarPlaneDistance;
            DummyObject closestObject = null;

            DrawTerrain(graphics, camera.ViewTransform, camera.ProjectionTransform);
            // Check whether terrain is closer than far clip
            {
                graphics.SetRenderTarget(null);

                Color[] depthColor = new Color[1];
                depthTarget.GetData(depthColor);

                if (depthColor[0].A < minDepth)
                {
                    minDepth = depthColor[0].A;
                }
                //Console.WriteLine(depthColor[0].A);
            }

            foreach (DummyObject dummy in mDummies)
            {
                graphics.SetRenderTarget(depthTarget);

                Model model = AssetLibrary.LookupModel(dummy.Model).Model;
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (AnimationUtilities.SkinnedEffect effect in mesh.Effects)
                    {
                        effect.View = camera.GetViewTransform();
                        effect.Projection = camera.GetProjectionTransform();
                        effect.World = Matrix.CreateScale(dummy.Scale) *
                                       Matrix.CreateFromYawPitchRoll(dummy.YawPitchRoll.X, dummy.YawPitchRoll.Y, dummy.YawPitchRoll.Z) *
                                       Matrix.CreateTranslation(dummy.Position);

                        effect.CurrentTechnique = effect.Techniques["NormalDepthShade"];
                    }
                    mesh.Draw();
                }

                graphics.SetRenderTarget(null);

                Color[] depthColor = new Color[1];
                depthTarget.GetData(depthColor);

                if (depthColor[0].A < minDepth)
                {
                    minDepth = depthColor[0].A;
                    closestObject = dummy;
                }
            }

            // Reset graphics device to previous settings
            graphics.SetRenderTargets(oldRenderTargets);
            //Console.WriteLine(minDepth);

            return new Tuple<Vector3, DummyObject>(ray.Position + ray.Direction * minDepth, closestObject);
        }

        public void Draw()
        {
            if (mTerrainPhysics != null)
            {
                mTerrainPhysics.Render();
            }

            if (mWater != null)
            {
                mWater.Render();
            }

            if (mSkyBox != null)
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

    }
}
